using Automation.Shared.Browser.Captcha;
using Automation.Shared.Browser.Models;
using Automation.Shared.Extensions;
using Automation.Shared.Helpers;
using Automation.Shared.Models.Functions;
using Automation.Shared.Services;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Newtonsoft.Json;
using System.Text.Json;

namespace Automation.Shared.Browser
{
    public class BrowserControllerService : BaseService, IBrowserControllerService
    {
        private IBrowser _browser;
        private IBrowserContext _context;
        private IPlaywright _playwright;
        private ICaptchaDetector _captchaDetector;

        private long _totalBytesSent;
        private long _totalBytesReceived;
        private ProxyInfo _proxyData;

        const int MAX_DOWNLOAD_SIZE = 1024 * 1024 * 50; // 100MB default
        public int? MaxMediaBandwidthBytes = null;
        public IPage CurrentPage { get; set; }

        public int ProxyUsageBytes => (int)(_totalBytesSent + _totalBytesReceived);

        public string CurrentUrl => CurrentPage.Url;

        public event EventHandler<string> FileDownload;

        public int? StatusCode { get; private set; }

        public BrowserControllerService(IServiceProvider provider, ILoggingService loggingService) : base(provider, loggingService)
        {
            _captchaDetector = provider.GetRequiredService<ICaptchaDetector>();
        }

        public async Task InstallAsync(CancellationToken? ct = null)
        {
            Console.WriteLine("Starting Playwright installation...");
            try
            {
                // Determine installation path based on build configuration
                string browserInstallPath =
#if DEBUG
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "playwright-browsers");
#else
                @"C:\Gaffa\playwright-browsers";

                Environment.SetEnvironmentVariable("PLAYWRIGHT_BROWSERS_PATH", browserInstallPath, EnvironmentVariableTarget.Machine);
#endif
                Console.WriteLine($"Desired Playwright browser install location: {browserInstallPath}");
                // Add these lines to log both the set path and what Playwright sees
                Console.WriteLine($"PLAYWRIGHT_BROWSERS_PATH Machine level: {Environment.GetEnvironmentVariable("PLAYWRIGHT_BROWSERS_PATH", EnvironmentVariableTarget.Machine)}");
                Console.WriteLine($"PLAYWRIGHT_BROWSERS_PATH Process level: {Environment.GetEnvironmentVariable("PLAYWRIGHT_BROWSERS_PATH", EnvironmentVariableTarget.Process)}");
                Console.WriteLine($"PLAYWRIGHT_BROWSERS_PATH User level: {Environment.GetEnvironmentVariable("PLAYWRIGHT_BROWSERS_PATH", EnvironmentVariableTarget.User)}");

                // Install Playwright CLI tools
                Console.WriteLine("Installing Playwright CLI tools...");

                Microsoft.Playwright.Program.Main(new string[] { "install", "chrome" });
                Console.WriteLine("Playwright CLI tools installation completed.");

                // Install browser(s)
                Console.WriteLine("Installing Chromium browser...");
                Microsoft.Playwright.Program.Main(new string[] { "install", "chromium" });
                Console.WriteLine("Chromium browser installation completed.");

                _playwright = await Playwright.CreateAsync().MaybeWaitAsync(ct);
                Console.WriteLine("Playwright initialization successful.");

                // Specify the local folder path where you want to save the files
                //string localFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TempFiles");
                //Console.WriteLine($"Creating temp directory at: {localFolderPath}");

                //// Ensure the directory exists
                //Directory.CreateDirectory(localFolderPath);
                //Console.WriteLine("Temp directory created successfully.");

                // Verify installations
                if (Directory.Exists(browserInstallPath))
                {
                    Console.WriteLine($"Playwright installed at: {browserInstallPath}");
                    var browsers = Directory.GetDirectories(browserInstallPath);
                    foreach (var browser in browsers)
                    {
                        Console.WriteLine($"Found browser installation: {Path.GetFileName(browser)}");
                    }
                }
                Console.WriteLine("Installation completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Installation failed - {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        private async Task<string> InstallScript(string localFolder, string url, CancellationToken? ct = null)
        {
            string localFilePath = Path.Combine(localFolder, Path.GetFileName(url));

            // Use the helper method to download the file
            bool downloadSuccess = await GenericHelpers.DownloadFileAsync(url, localFilePath).MaybeWaitAsync(ct);
            if (downloadSuccess)
            {
                return localFilePath;
            }
            else
            {
                throw new Exception($"Failed to download file from {url}");
            }
        }

        public async Task StartSessionAsync(ProxyInfo? proxy = null, int? maxMediaBandwidthInMB = null, CancellationToken? ct = null)
        {
            if (maxMediaBandwidthInMB != null)
            {
                MaxMediaBandwidthBytes = 1024 * 1024 * maxMediaBandwidthInMB;
            }
            else
            {
                MaxMediaBandwidthBytes = null;
            }

            if (_context != null)
            {
                await _context.CloseAsync();
                _context = null;
            }

            var launchArgs = new List<string>()
            {
                "--start-maximized"
            };
            var launchOptions = new BrowserTypeLaunchOptions
            {
                Headless = false,
                Args = launchArgs.ToArray(),
                ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe"
            };

            if (proxy != null)
            {
                launchOptions.Proxy = new Proxy
                {
                    Server = proxy.Url,
                    Username = proxy.Credentials.Username,
                    Password = proxy.Credentials.Password,
                };

                //Needed for cpatcha service
                _proxyData = proxy;
            }
            else
            {
                _proxyData = null;
            }

            _browser = await _playwright.Chromium.LaunchAsync(launchOptions).MaybeWaitAsync(ct);

            var contextOptions = new BrowserNewContextOptions()
            {
                //TODO: SSL errors when downloading PDFs, need to handle this better
                //FIX: Potential security issues with this, but needed for PDF downloads
                IgnoreHTTPSErrors = true,
                ViewportSize = ViewportSize.NoViewport,
            };

            if (proxy != null)
            {
                contextOptions.HttpCredentials = proxy.Credentials;
            }

            // Creating a new incognito browser context
            _context = await _browser.NewContextAsync(contextOptions).MaybeWaitAsync(ct);
            _context.SetDefaultTimeout(45000);

            _context.Page += PageAdded;

            //Possible improvements discussed here: https://www.notion.so/PDF-Downloading-1d1a9249679b8021acfaf3b56fc05fd5?pvs=4

            // PDF download routes
            await _context.RouteAsync("**/*.pdf", HandleFileDownloadRoute);
            await _context.RouteAsync("**/download/*", HandleFileDownloadRoute);
            await _context.RouteAsync("**/*pdf*/**", HandleFileDownloadRoute);
            await _context.RouteAsync("**/*PDF*/**", HandleFileDownloadRoute);

            // Image download routes
            await _context.RouteAsync("**/*.jpg", HandleFileDownloadRoute);
            await _context.RouteAsync("**/*.jpeg", HandleFileDownloadRoute);
            await _context.RouteAsync("**/*.png", HandleFileDownloadRoute);
            await _context.RouteAsync("**/*.gif", HandleFileDownloadRoute);
            await _context.RouteAsync("**/*.bmp", HandleFileDownloadRoute);
            await _context.RouteAsync("**/*.webp", HandleFileDownloadRoute);
            await _context.RouteAsync("**/*.svg", HandleFileDownloadRoute);
            await _context.RouteAsync("**/*.tiff", HandleFileDownloadRoute);
            await _context.RouteAsync("**/*.tif", HandleFileDownloadRoute);
        }

        public async void BlockImagesAndVideos()
        {
            if (_context != null)
            {
                await _context.RouteAsync("**/*", async route =>
                {
                    var url = route.Request?.Url?.ToLower();

                    if (url == null)
                        return;

                    if (route.Request?.ResourceType == "image" ||
                        route.Request?.ResourceType == "media" ||
                        url.Contains(".jpg") ||
                        url.Contains(".mp4"))
                    {
                        await route.AbortAsync();
                        return;
                    }


                    await route.ContinueAsync();
                });
            }
        }

        private async Task HandleFileDownloadRoute(IRoute route)
        {
            try
            {
                // Fetch the original response
                var response = await route.FetchAsync();

                // Get content type, if exits
                response.Headers.TryGetValue("content-type", out var contentType);

                // Check if it's a downloadable file type (PDF or image)
                string fileName = null;
                string fileExtension = null;

                if (contentType != null)
                {
                    if (contentType.Contains("application/pdf"))
                    {
                        fileExtension = ".pdf";
                        fileName = $"download_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                    }
                    else if (contentType.Contains("image/jpeg"))
                    {
                        fileExtension = ".jpg";
                        fileName = $"download_{DateTime.Now:yyyyMMddHHmmss}.jpg";
                    }
                    else if (contentType.Contains("image/png"))
                    {
                        fileExtension = ".png";
                        fileName = $"download_{DateTime.Now:yyyyMMddHHmmss}.png";
                    }
                    else if (contentType.Contains("image/gif"))
                    {
                        fileExtension = ".gif";
                        fileName = $"download_{DateTime.Now:yyyyMMddHHmmss}.gif";
                    }
                    else if (contentType.Contains("image/bmp"))
                    {
                        fileExtension = ".bmp";
                        fileName = $"download_{DateTime.Now:yyyyMMddHHmmss}.bmp";
                    }
                    else if (contentType.Contains("image/webp"))
                    {
                        fileExtension = ".webp";
                        fileName = $"download_{DateTime.Now:yyyyMMddHHmmss}.webp";
                    }
                    else if (contentType.Contains("image/svg+xml"))
                    {
                        fileExtension = ".svg";
                        fileName = $"download_{DateTime.Now:yyyyMMddHHmmss}.svg";
                    }
                    else if (contentType.Contains("image/tiff"))
                    {
                        fileExtension = ".tiff";
                        fileName = $"download_{DateTime.Now:yyyyMMddHHmmss}.tiff";
                    }
                    else if (contentType.Contains("image/"))
                    {
                        // Generic image fallback
                        fileExtension = ".img";
                        fileName = $"download_{DateTime.Now:yyyyMMddHHmmss}.img";
                    }
                }

                // If we have a valid file type, save it
                if (!string.IsNullOrEmpty(fileName))
                {
                    var body = await response.BodyAsync();

                    // Find latest Playwright temp folder
                    var tempPath = PlaywrightHelpers.GetPlaywrightTempDirectory();
                    var fullPath = Path.Combine(tempPath, fileName);

                    using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await fileStream.WriteAsync(body);
                        await fileStream.FlushAsync();
                    }

                    FileDownload?.Invoke(this, fullPath);
                }
            }
            catch (Exception ex)
            {
                // Log error and continue with original response to prevent blocking
#if !DEBUG
                SentrySdk.CaptureException(ex);
#endif
            }
            finally
            {

                await route.ContinueAsync();
            }
        }

        public async Task EndSessionAsync(CancellationToken? ct = null)
        {
            if (_context != null)
            {
                await _context.CloseAsync().MaybeWaitAsync(ct);
                imageBlocked = false;
                _context = null;
            }

            if (_browser != null)
            {
                await _browser.CloseAsync();
            }
        }

        private async void PageAdded(object? sender, IPage page)
        {
            //Logger.Log(LogLevel.Information, $"New page created in context: {page?.Url}");

            page.Request += OnRequest;
            page.Response += OnResponse;
            page.RequestFinished += OnRequestFinished;
            page.Download += Page_Download;

            //set new page as current
            CurrentPage = page;
        }

        private async void Page_Download(object? sender, IDownload e)
        {
            //TODO: add check here for file size?
            var path = await e.PathAsync();

            //check has file extension, otherwise add using url
            if (string.IsNullOrEmpty(Path.GetExtension(path)))
            {
                var url = e.Url;
                var extension = Path.GetExtension(url);
                if (!string.IsNullOrEmpty(extension))
                {
                    var newPath = Path.ChangeExtension(path, extension);
                    File.Move(path, newPath);
                    path = newPath;
                }
            }

            FileDownload?.Invoke(this, path);
        }

        public async Task FocusTab(int tabIndex, CancellationToken? ct = null)
        {
            if (_context != null)
            {
                var pages = _context.Pages;
                if (pages.Count > tabIndex)
                {
                    await _context.Pages[tabIndex].BringToFrontAsync().MaybeWaitAsync(ct);
                    CurrentPage = _context.Pages[tabIndex];
                }
            }
        }

        public async Task OpenNewTabAsync(string url = null, CancellationToken? ct = null)
        {
            if (_context != null)
            {
                CurrentPage = await _context.NewPageAsync().MaybeWaitAsync(ct);

                _totalBytesSent = 0;
                _totalBytesReceived = 0;

                if (url != null)
                {
                    await CurrentPage.GotoAsync(url).MaybeWaitAsync(ct);
                }
            }
        }

        static bool imageBlocked = false;
        private async void OnRequestFinished(object? sender, IRequest request)
        {
            await HandleNetworkEventAsync(async () =>
            {
                var response = await request.ResponseAsync();
                if (response == null) return;

                if (response.Status >= 300 && response.Status <= 399)
                {
                    //Don't check for redirects
                    return;
                }

                var headers = response.Headers;
                if (headers.GetValueOrDefault("accept-ranges", "").ToLower() == "bytes")
                {
                    // For downloads, find the content length
                    // Handle byte-range content using content-length
                    var contentLength = headers.GetValueOrDefault("content-length", "0");
                    if (long.TryParse(contentLength, out long size))
                    {
                        if (size == 0)
                        {
                            size = (await response.BodyAsync()).Length;
                        }

                        _totalBytesReceived += size;
                        //LogInformation($"[Bytes] Size: {size} Total bytes received: {_totalBytesReceived}, {request.Url}");
                    }
                }
                else
                {
                    // Original logic for non byte-range content
                    try
                    {
                        var contentLength = headers.GetValueOrDefault("content-length", "0");
                        if (long.TryParse(contentLength, out long size))
                        {
                            if (size == 0)
                            {
                                size = (await response.BodyAsync()).Length;
                            }

                            _totalBytesReceived += size;
                            //log request info and total bytes received
                            //LogInformation($"[Non-byte] Size: {size} Total bytes received: {_totalBytesReceived}, {request.Url}");
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }

                //Block media requests if max bandwidth reached
                if (MaxMediaBandwidthBytes != null && !imageBlocked)
                {
                    if (_totalBytesReceived + _totalBytesSent > MaxMediaBandwidthBytes)
                    {
                        imageBlocked = true;
                        BlockImagesAndVideos();
                    }
                }
            });
        }

        public async Task<(BrowserOperationStatusType, int?)> OpenPageAsync(string url, CancellationToken? ct)
        {
            if (CurrentPage != null)
            {
                try
                {
                    var response = await CurrentPage.GotoAsync(url, new PageGotoOptions()).MaybeWaitAsync(ct);

                    return (BrowserOperationStatusType.Success, response.Status);
                }
                //Proxy blocked connection
                catch (Microsoft.Playwright.PlaywrightException ex) when (ex.Message.Contains("ERR_TUNNEL_CONNECTION_FAILED"))
                {
                    return (BrowserOperationStatusType.SiteUnavailable, 503);
                }
                catch (TimeoutException ex)
                {
                    return (BrowserOperationStatusType.SiteUnavailable, 504);
                }
            }
            return (BrowserOperationStatusType.Unknown, null);
        }

        private async void OnRequest(object? sender, IRequest request)
        {
            //TODO: probably need to check accuracy
            //TODO: Handle upload usage here later
            await HandleNetworkEventAsync(async () =>
            {
                _totalBytesSent += request.Headers.Sum(header => header.Key.Length + header.Value.Length);
            });
        }

        private async void OnResponse(object? sender, IResponse response)
        {
            StatusCode = response.Status;

            await HandleNetworkEventAsync(async () =>
            {
            });
        }

        private async Task HandleNetworkEventAsync(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception ex) when (ex.Message.Contains("Network.getResponseBody"))
            {
                Logging.Log($"Network event no longer available: {ex.Message}", LogLevel.Warning);
            }
            catch (BrowserControllerException ex)
            {
                Logging.Log($"Download too large: {ex.Message}", LogLevel.Error);
                await _context.SetOfflineAsync(true);
            }
            catch (Exception ex)
            {
#if !DEBUG
                SentrySdk.CaptureException(ex);
#endif
            }
        }

        public async Task<string> GetSingleFileAsync(CancellationToken? ct = null)
        {
            if (CurrentPage != null)
            {
                //prime page, change srcsets so the images load
                var presnapshotScript = await GenericHelpers.LoadFileAsync("Automation.Shared", "Browser.Scripts.PreSnapshotProcessing.js").MaybeWaitAsync(ct);

                //string wrappedScript = $@"
                //(() => {{
                //    {presnapshotScript}
                //}})();
                //";

                await CurrentPage.EvaluateAsync(presnapshotScript);
                // Optionally, wait a bit to ensure images have time to load
                await Task.Delay(2000);

                var session = await CurrentPage.Context.NewCDPSessionAsync(CurrentPage);
                var doc = await session.SendAsync("Page.captureSnapshot", new Dictionary<string, object>
                {
                    ["format"] = "mhtml"
                }).MaybeWaitAsync(ct);
                await session.DisposeAsync();

                var result = doc.Value;
                if (result.TryGetProperty("data", out JsonElement mhtmlBase64Element) && mhtmlBase64Element.ValueKind == JsonValueKind.String)
                {
                    var mhtmlContent = mhtmlBase64Element.GetString();

                    return await MhtmlConverter.ConvertMhtmlToHtml(mhtmlContent);
                }
            }

            return null;
        }

        public async Task<string> GetDomJsonAsync(CancellationToken? ct = null)
        {
            if (CurrentPage != null)
            {
                //load dom parser script
                var domParserScript = await GenericHelpers.LoadFileAsync("Automation.Shared", "Browser.Scripts.DOMParser.js").MaybeWaitAsync(ct);

                //run annotation
                await CurrentPage.EvaluateAsync(domParserScript).MaybeWaitAsync(ct);
                await CurrentPage.EvaluateAsync("window.customMethods.annotate()").MaybeWaitAsync(ct);
                string jsonResult = await CurrentPage.EvaluateAsync<string>("window.customMethods.getFullHierachy()").MaybeWaitAsync(ct);
                return jsonResult;
            }

            return null;
        }

        public async Task CloseTabAsync(CancellationToken? ct = null)
        {
            if (CurrentPage != null)
            {
                CurrentPage.Request -= OnRequest;
                CurrentPage.Response -= OnResponse;
                await CurrentPage.CloseAsync().MaybeWaitAsync(ct);
                CurrentPage = null;
            }
        }

        public async Task<string> TakeScreenshotAsync(bool fullPage = false, CancellationToken? ct = null)
        {
            if (CurrentPage != null)
            {
                var file = GenericHelpers.GetTempFile(".png");
                await CurrentPage.ScreenshotAsync(new PageScreenshotOptions
                {
                    Path = file,
                    FullPage = fullPage
                }).MaybeWaitAsync(ct);
                return file;
            }

            return string.Empty;
        }

        public async Task NavigateToAsync(string url, CancellationToken? ct = null)
        {
            if (CurrentPage != null)
            {
                await CurrentPage.GotoAsync(url).MaybeWaitAsync(ct);
            }
        }

        //TODO: consider offloading all processing to server
        public async Task<string> GetDomContentAsync(CancellationToken? ct = null)
        {
            if (CurrentPage != null)
            {
                var domContent = await CurrentPage.ContentAsync().MaybeWaitAsync(ct);
                return await HtmlConverter.PrettifyHtmlAsync(domContent).MaybeWaitAsync(ct);
            }

            return string.Empty;
        }

        public async Task ShutdownAsync(CancellationToken? ct = null)
        {
            if (CurrentPage != null)
            {
                await CurrentPage.CloseAsync().MaybeWaitAsync(ct);
            }
            _playwright?.Dispose();
        }

        public async Task BlockDOMRemovalsAsync(CancellationToken? ct = null)
        {
            if (CurrentPage != null)
            {
                var blockingScript = await GenericHelpers.LoadFileAsync("Automation.Shared", "Browser.Scripts.BlockDOMRemoval.js").MaybeWaitAsync(ct);
                await CurrentPage.EvaluateAsync(blockingScript).MaybeWaitAsync(ct);
            }
        }

        public async Task<BrowserActionOperationStatusType> ScrollAsync(double? targetPercentage, int? waitTime = 10000, int? maxScrollTime = null, ScrollSpeed? speed = null, int? interval = null, int? timeout = null, CancellationToken? ct = null)
        {
            var metrics = new BrowserScrollMetrics(CurrentPage);
            var scrollService = new ScrollService(Logging);

            if (speed == null)
            {
                speed = ScrollSpeed.Medium;
            }

            int? lastExpansionCount = null;
            ScrollResult result = null;

            while (true)
            {
                await metrics.UpdateMetricsAsync().MaybeWaitAsync(ct);

                try
                {
                    result = await scrollService.CalculateScrollStep(metrics, targetPercentage.Value, waitTime, maxScrollTime, speed.Value, timeout).MaybeWaitAsync(ct);
                }
                catch (NotScrollableException)
                {
                    return BrowserActionOperationStatusType.NotScrollable;
                }

                if (result.ReachedTarget)
                {
                    //Logging.Log(LogLevel.Information, $"Scroll complete. Final position: {result.FinalPosition}, Expansions: {result.ExpansionCount}");
                    break;
                }

                if (result.TimeoutReached)
                {
                    //Logger.Log(LogLevel.Information, $"Scroll timeout at {maxScrollTime}ms. Position: {result.FinalPosition}");
                    break;
                }

                if (result.WaitCompleted)
                {
                    //Logger.Log(LogLevel.Information, $"Wait completed. Position: {result.FinalPosition}");
                    break;
                }

                if (result.NextScroll != null)
                {
                    //don't scroll if has expanded since
                    if (lastExpansionCount == null || lastExpansionCount == result.ExpansionCount)
                    {
                        await CurrentPage.Mouse.WheelAsync(0, result.NextScroll.ScrollAmount).MaybeWaitAsync(ct);
                    }

                    //wait for pause or normal delay time
                    await Task.Delay(interval != null ?
                        interval.Value :
                        result.NextScroll.DelayMs).MaybeWaitAsync(ct);

                    lastExpansionCount = result.ExpansionCount;
                }
            }

            return BrowserActionOperationStatusType.Success;
        }

        public async Task<string> PrintAsync(string pageSize, int margin, string orientation, CancellationToken? ct = null)
        {
            var tempFile = GenericHelpers.GetTempFile(".pdf");

            if (CurrentPage != null)
            {
                await CurrentPage.EmulateMediaAsync(new PageEmulateMediaOptions
                {
                    Media = Media.Print
                }).MaybeWaitAsync(ct);

                await CurrentPage.PdfAsync(new PagePdfOptions
                {
                    Path = tempFile,
                    Format = pageSize,
                    Landscape = orientation == "landscape", // Set to true for landscape, false for portrait
                    Margin = new Margin
                    {
                        Top = $"{margin}px",
                        Bottom = $"{margin}px",
                        Left = $"{margin}px",
                        Right = $"{margin}px",
                    },
                    PrintBackground = true
                }).MaybeWaitAsync(ct);

                await CurrentPage.EmulateMediaAsync(new PageEmulateMediaOptions
                {
                    Media = Media.Screen
                }).MaybeWaitAsync(ct);
            }

            return tempFile;
        }

        public async Task<BrowserActionOperationStatusType> WaitAsync(string selector, int? timeout, CancellationToken? ct = null)
        {
            if (CurrentPage == null)
                return BrowserActionOperationStatusType.Failed;

            try
            {
                await FindElementWithRetryAsync(selector, timeout, ct);
                return BrowserActionOperationStatusType.Success;
            }
            catch (PlaywrightException)
            {
                return BrowserActionOperationStatusType.Failed;
            }
            catch (TimeoutException)
            {
                return BrowserActionOperationStatusType.TimedOut;
            }
        }

        public async Task<BrowserActionOperationStatusType> ClickAsync(string selector, int? timeout, CancellationToken? ct = null)
        {
            if (CurrentPage == null)
                return BrowserActionOperationStatusType.Failed;

            try
            {
                // Get the locator that matches the selector
                var locator = await FindElementWithRetryAsync(selector, timeout, ct);

                // Count how many elements match the selector
                int count = await locator.CountAsync().MaybeWaitAsync(ct);

                if (count == 0)
                    return BrowserActionOperationStatusType.Failed;

                // If there's only one element, just click it
                if (count == 1)
                {
                    await locator.ClickAsync(new LocatorClickOptions()
                    {
                        Delay = GenericHelpers.RandGenerator.Next(201, 654),
                        Timeout = 0
                    }).MaybeWaitAsync(ct);

                    return BrowserActionOperationStatusType.Success;
                }

                // For multiple elements, try clicking each one until one succeeds
                PlaywrightException? lastPlaywrightException = null;
                TimeoutException? lastTimeoutException = null;

                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        // Get locator for the specific element at index i 
                        var elementLocator = locator.Nth(i);

                        // Try to click this specific element
                        await elementLocator.ClickAsync(new LocatorClickOptions()
                        {
                            Delay = GenericHelpers.RandGenerator.Next(201, 654),
                            Timeout = 1000
                        }).MaybeWaitAsync(ct);

                        // If we got here, clicking succeeded
                        return BrowserActionOperationStatusType.Success;
                    }
                    catch (PlaywrightException ex)
                    {
                        // Store the exception but continue to the next element
                        lastPlaywrightException = ex;
                    }
                    catch (TimeoutException ex)
                    {
                        // Store the exception but continue to the next element
                        lastTimeoutException = ex;
                    }
                }

                // If we get here, all click attempts failed
                if (lastTimeoutException != null)
                    return BrowserActionOperationStatusType.TimedOut;

                return BrowserActionOperationStatusType.Failed;
            }
            catch (PlaywrightException)
            {
                return BrowserActionOperationStatusType.Failed;
            }
            catch (TimeoutException)
            {
                return BrowserActionOperationStatusType.TimedOut;
            }
        }

        public async Task MoveAndClickAsync(int toX, int toY, CancellationToken? ct = null)
        {
            if (CurrentPage != null)
            {
                try
                {
                    await CursorControl.MoveCursorAndClickAsync(toX, toY);

                    Logging.Log($"Successfully clicked at ({toX},{toY})", LogLevel.Information);
                    return;
                }
                catch (Exception ex)
                {
                    Logging.Log($"Failed to click at point ({toX},{toY}): {ex.Message}", LogLevel.Error);
                }
            }
            throw new InvalidOperationException("No active page available");
        }

        public async Task<BrowserActionOperationStatusType> TypeAsync(string selector, string text, int? timeout, CancellationToken? ct = null)
        {
            if (CurrentPage == null)
                return BrowserActionOperationStatusType.Failed;

            try
            {
                // Get the locator that matches the selector
                var locator = await FindElementWithRetryAsync(selector, timeout, ct);

                // Count how many elements match the selector
                int count = await locator.CountAsync().MaybeWaitAsync(ct);

                if (count == 0)
                    return BrowserActionOperationStatusType.Failed;

                // If there's only one element, just type in it
                if (count == 1)
                {
                    foreach (var c in text)
                    {
                        await locator.PressAsync(c.ToString()).MaybeWaitAsync(ct);
                        await Task.Delay(GenericHelpers.RandGenerator.Next(60, 150)).MaybeWaitAsync(ct);
                    }

                    return BrowserActionOperationStatusType.Success;
                }

                // For multiple elements, try typing in each one until one succeeds
                PlaywrightException? lastPlaywrightException = null;
                TimeoutException? lastTimeoutException = null;

                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        // Get locator for the specific element at index i
                        var elementLocator = locator.Nth(i);

                        // Try to type text in this specific element
                        foreach (var c in text)
                        {
                            await elementLocator.PressAsync(c.ToString()).MaybeWaitAsync(ct);
                            await Task.Delay(GenericHelpers.RandGenerator.Next(60, 150)).MaybeWaitAsync(ct);
                        }

                        // If we got here, typing succeeded
                        return BrowserActionOperationStatusType.Success;
                    }
                    catch (PlaywrightException ex)
                    {
                        // Store the exception but continue to the next element
                        lastPlaywrightException = ex;
                    }
                    catch (TimeoutException ex)
                    {
                        // Store the exception but continue to the next element
                        lastTimeoutException = ex;
                    }
                }

                // If we get here, all typing attempts failed
                if (lastTimeoutException != null)
                    return BrowserActionOperationStatusType.TimedOut;

                return BrowserActionOperationStatusType.Failed;
            }
            catch (PlaywrightException)
            {
                return BrowserActionOperationStatusType.Failed;
            }
            catch (TimeoutException)
            {
                return BrowserActionOperationStatusType.TimedOut;
            }
        }

        public async Task<CaptchaResultType> CheckForCaptchaAsync(CancellationToken ct)
        {
            if (CurrentPage != null)
            {
                try
                {
                    await _captchaDetector.DetectAndSolveCaptcha(this, ct);

                    return CaptchaResultType.Success;
                }
                catch (CaptchaException e)
                {
                    return e.Type;
                }
            }
            return CaptchaResultType.CaptchaDismissFailed;
        }

        public async Task<(BrowserActionOperationStatusType, string)> CaptureElementAsync(string selector, int? timeout, CancellationToken? ct = null)
        {
            if (CurrentPage == null)
                return (BrowserActionOperationStatusType.Failed, null);

            try
            {
                // Wait for element to be present in DOM
                var locator = await FindElementWithRetryAsync(selector, timeout, ct);

                // Get the outerHTML of the element
                var html = await CurrentPage.EvaluateAsync<string>(@"
                    (selector) => {
                        const element = document.querySelector(selector);
                        return element ? element.outerHTML : null;
                    }
                ", selector).MaybeWaitAsync(ct);

                return (BrowserActionOperationStatusType.Success, html);
            }
            catch
            {
                return (BrowserActionOperationStatusType.Failed, null);
            }
        }

        public async Task<(BrowserActionOperationStatusType, string?)> ParseTableAsync(string selector, int? timeout = 20000, CancellationToken? ct = null)
        {
            if (CurrentPage == null)
                return (BrowserActionOperationStatusType.Failed, null);
            try
            {
                // Wait for the table to be present in DOM
                var table = await CurrentPage.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions()
                {
                    Timeout = timeout,
                    State = WaitForSelectorState.Attached
                }).MaybeWaitAsync(ct);
                if (table == null)
                    return (BrowserActionOperationStatusType.Failed, null);

                var domParserScript = await GenericHelpers.LoadFileAsync("Automation.Shared", "Browser.Scripts.TableParser.js").MaybeWaitAsync(ct);

                var result = await CurrentPage.EvaluateAsync<string>(domParserScript, selector).MaybeWaitAsync(ct);

                if (!result.IsNullOrDefault())
                {
                    // Configure Json.NET serialization settings
                    //Use so we can output indented JSON
                    var settings = new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                        Formatting = Formatting.Indented,
                        StringEscapeHandling = StringEscapeHandling.Default,
                        NullValueHandling = NullValueHandling.Include
                    };

                    // Deserialize using Json.NET
                    var data = JsonConvert.DeserializeObject<PageDataResponse>(result, settings);

                    if (data.Data == null)
                    {
                        return (BrowserActionOperationStatusType.Failed, null);
                    }

                    // Serialize back with the same settings
                    return (BrowserActionOperationStatusType.Success,
                            JsonConvert.SerializeObject(data.Data, settings));
                }

                return (BrowserActionOperationStatusType.Failed, null);
            }
            catch (TimeoutException)
            {
                return (BrowserActionOperationStatusType.TimedOut, null);
            }
            catch (Exception ex)
            {
                return (BrowserActionOperationStatusType.Failed, null);
            }
        }

        public async Task<(BrowserActionOperationStatusType, string?)> GetCookiesAsync(CancellationToken? ct = null)
        {
            if (CurrentPage != null)
            {
                var cookies = await CurrentPage.Context.CookiesAsync().MaybeWaitAsync(ct);
                var settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Include
                };
                return (BrowserActionOperationStatusType.Success, JsonConvert.SerializeObject(cookies, settings));
            }

            return (BrowserActionOperationStatusType.Failed, null);
        }

        public async Task<bool> IsPdfPageAsync()
        {
            if (CurrentPage == null)
                return false;

            // Check if URL ends with .pdf
            if (CurrentUrl.ToLower().EndsWith(".pdf"))
                return true;

            // Check content-type response header
            try
            {
                // Use JavaScript to get the content type from response headers or meta tags
                var contentType = await CurrentPage.EvaluateAsync<string>(@"() => {
                    // Check for PDF viewer elements
                    const hasPdfViewer = !!document.querySelector('embed[type=""application/pdf""], object[type=""application/pdf""]');
                    if (hasPdfViewer) return 'application/pdf';
            
                    // Check for content-type meta tag
                    const meta = document.querySelector('meta[http-equiv=""Content-Type""]');
                    return meta && meta.content.includes('application/pdf') ? meta.content : '';
                }");

                return !string.IsNullOrEmpty(contentType) && contentType.Contains("application/pdf");
            }
            catch
            {
                return false;
            }
        }

        private async Task<ILocator> FindElementWithRetryAsync(string selector, int? timeout = null, CancellationToken? ct = null)
        {
            if (CurrentPage == null)
                throw new InvalidOperationException("No active page available");

            selector = System.Text.RegularExpressions.Regex.Unescape(selector);

            // Default timeout if not specified
            int timeoutMs = timeout ?? 5000;
            DateTime startTime = DateTime.Now;

            // Try to find the element with retries until timeout
            while ((DateTime.Now - startTime).TotalMilliseconds < timeoutMs)
            {
                ct?.ThrowIfCancellationRequested();

                // Check in main frame first
                ILocator mainFrameLocator = CurrentPage.Locator(selector);
                int mainFrameCount = await mainFrameLocator.CountAsync();

                if (mainFrameCount > 0)
                    return mainFrameLocator;

                // If not found in main frame, check all iframes
                var frames = CurrentPage.Frames;
                foreach (var frame in frames.Where(f => f.ParentFrame != null)) // Skip main frame
                {
                    try
                    {
                        // Try to find the element in this iframe
                        ILocator frameLocator = frame.Locator(selector);
                        int frameCount = await frameLocator.CountAsync();

                        if (frameCount > 0)
                            return frameLocator.First;
                    }
                    catch (Exception)
                    {
                        // Continue to next frame if there's an error accessing this one
                        continue;
                    }
                }

                // If we reach here, the element wasn't found in any frame
                // Wait a bit before trying again
                await Task.Delay(100, ct ?? CancellationToken.None);
            }

            // If we get here, we've timed out - use a short Playwright timeout to get its native exception
            // This will throw the native Playwright timeout exception
            await CurrentPage.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions
            {
                State = WaitForSelectorState.Attached,
                Timeout = 100 // Short timeout just to get the exception
            });

            // This line should never be reached, but added for completeness
            throw new TimeoutException(selector);
        }
    }

    public class PageDataResponse
    {
        public PageDataResponse()
        {

        }

        public dynamic Data { get; set; }
    }

    public interface IBrowserControllerService
    {
        int ProxyUsageBytes { get; }
        int? StatusCode { get; }
        event EventHandler<string> FileDownload;
        IPage CurrentPage { get; set; }

        string CurrentUrl { get; }
        Task InstallAsync(CancellationToken? ct = null);
        Task StartSessionAsync(ProxyInfo? proxy = null, int? maxMediaBandwidthInMB = null, CancellationToken? ct = null);
        Task<string> GetSingleFileAsync(CancellationToken? ct = null);
        Task EndSessionAsync(CancellationToken? ct = null);
        Task FocusTab(int tabIndex, CancellationToken? ct = null);
        Task NavigateToAsync(string url, CancellationToken? ct = null);
        Task OpenNewTabAsync(string url = null, CancellationToken? ct = null);
        Task<(BrowserOperationStatusType, int?)> OpenPageAsync(string url, CancellationToken? ct = null);
        Task CloseTabAsync(CancellationToken? ct = null);
        Task<string> GetDomContentAsync(CancellationToken? ct = null);
        Task<string> TakeScreenshotAsync(bool fullPage = false, CancellationToken? ct = null);
        Task ShutdownAsync(CancellationToken? ct = null);
        Task<BrowserActionOperationStatusType> ScrollAsync(double? targetPercentage, int? waitTime = 10000, int? maxScrollTime = null, ScrollSpeed? speed = ScrollSpeed.Medium, int? interval = null, int? timeout = null, CancellationToken? ct = null);
        Task<string> PrintAsync(string pageSize, int margin, string orientation, CancellationToken? ct = null);
        Task<BrowserActionOperationStatusType> WaitAsync(string selector, int? timeout, CancellationToken? ct = null);
        Task<BrowserActionOperationStatusType> TypeAsync(string selector, string text, int? timeout, CancellationToken? ct = null);
        Task<BrowserActionOperationStatusType> ClickAsync(string selector, int? timeout, CancellationToken? ct = null);
        Task MoveAndClickAsync(int toX, int toY, CancellationToken? ct = null);
        Task<string> GetDomJsonAsync(CancellationToken? ct = null);
        Task<CaptchaResultType> CheckForCaptchaAsync(CancellationToken ct);
        Task<(BrowserActionOperationStatusType, string)> CaptureElementAsync(string selector, int? timeout, CancellationToken? ct = null);
        Task<(BrowserActionOperationStatusType, string?)> ParseTableAsync(string selector, int? timeout = 100, CancellationToken? ct = null);
        void BlockImagesAndVideos();
        Task<bool> IsPdfPageAsync();
        Task<(BrowserActionOperationStatusType, string?)> GetCookiesAsync(CancellationToken? ct);
        Task BlockDOMRemovalsAsync(CancellationToken? ct = null);
    }
}
