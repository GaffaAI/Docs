# May

### API Updates

#### **`Scroll` action now supports a selector parameter**&#x20;

[`scroll`](https://app.gitbook.com/s/yUba6osOT5MkKiV0wmgr/features/browser-requests/actions/scroll) action now accepts an optional `selector` parameter, allowing you to scroll a specific element on the page rather than the page body.

#### **`block_ads` parameter added**&#x20;

A new [`block_ads`](https://gaffa.dev/docs/features/browser-requests/settings#a-d-blocking) parameter is now available in request settings. When set to `true`, Gaffa intercepts and aborts requests to known ad domains before they load, reducing page load times on ad-heavy sites.&#x20;

#### **`generate_markdown` output improved**

We've made behind-the-scenes improvements to the [`generate_markdown`](https://gaffa.dev/docs/features/browser-requests/actions/generate-markdown) action, refining the output response format and expanding support for a greater variety of HTML elements. The result is cleaner, more consistent markdown across a broader range of pages.

### Samples, Blog & Tutorials

#### Speed up your scraping with ad blocking

We published a breakdown of how enabling [`block_ads`](https://gaffa.dev/docs/features/browser-requests/settings#a-d-blocking) cuts unnecessary ad network requests before they load, reducing page load times by up to 50% on ad-heavy sites. We tested it across The Guardian, Allrecipes, and Daily Mail, and also showed how combining it with [`max_media_bandwidth`](https://gaffa.dev/docs/features/browser-requests/settings#max-media-bandwidth) pushes performance even further. [Read the blog.](https://gaffa.dev/blog/speed-up-your-scraping-with-ad-blocking)

