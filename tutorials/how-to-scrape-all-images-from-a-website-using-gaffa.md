---
description: >-
  This tutorial will show you how you can use Gaffa to retrieve all images from
  a site and then download all images across those pages.
---

# How to scrape all images from a website using Gaffa

Automating the collection of images from a website can save hours of manual work. Whether you're a marketer building a competitor analysis, a developer creating a dataset, or an archiver preserving digital content, doing this manually is tedious and error-prone.

In this tutorial, you'll learn how to use Gaffa's powerful [Mapping](../features/mapping-requests.md) and [Browser Requests](../features/browser-requests/) endpoints to automatically find, extract, and download every image from a website in a short Python script. We'll leverage features like the [`capture_dom`](../features/browser-requests/actions/capture-dom.md) action, [intelligent sitemap parsing](../features/mapping-requests.md), and the [`download_file`](../features/browser-requests/actions/download-file.md) action to handle this efficiently and responsibly.

By the end of this guide, you'll be able to:

* Use Gaffa's [`site/map`](../features/mapping-requests.md) endpoint to discover every page on a site.
* Render each page with a headless browser to capture its full DOM.
* Parse and download all images using Gaffa's [`download_file`](../features/browser-requests/actions/download-file.md) action.
* Run the process at scale with built-in proxy rotation and caching.

### Prerequisites

* **Python 3.10+** is installed on your machine.
* A **Gaffa API key.** [Sign up for a free account](https://gaffa.dev/sign-up) and get your API key from the dashboard.
* Basic familiarity with the command line.

{% stepper %}
{% step %}
### Set Up Your Environment

First, create a new project directory and install the required Python libraries.

```sh
# Create a new directory and navigate into it
mkdir gaffa-image-scraper && cd gaffa-image-scraper

# Create a virtual environment (optional but recommended)
python -m venv venv
source venv/bin/activate
```

Next, set your Gaffa API key as an environment variable to keep it secure.

```sh
# On macOS/Linux
export GAFFA_API_KEY='your_gaffa_api_key_here'
```
{% endstep %}

{% step %}
### The Core Script Explained

Let's build the script step-by-step. The core logic consists of three main parts: mapping the site, capturing the DOM for each page, and extracting images using Gaffa's download system.

**Fetch All URLs from the Sitemap**

The `site/map` endpoint is our starting point. It does the heavy lifting of discovery by reading the sitemap, traversing potential link-outs, and retrieving every page on the website you want to scrape.

```python
def get_sitemap_urls(site_url, max_cache_age=86400):
    payload = {
        "url": site_url,
        "max_cache_age": max_cache_age
    }
    print("Retrieving sitemap URLs.")
    response = requests.post("https://api.gaffa.dev/v1/site/map", 
        json=payload, headers=HEADERS)
    return response.json()["data"]["links"]
```

**Capture the Rendered DOM of a Page**

For each URL, we use Gaffa to fully render the page (including JavaScript execution) and capture the final DOM. This is an important step since many websites are actually not fully rendered when we receive them. They contain links to JavaScript files that need to be executed first. These scripts will load further content from the backend, load images and other data. It’s necessary to first generate a fully rendered page before diving deeper into scraping it; otherwise, we would only scrape the content already provided in the initial HTML.

```python
def get_dom(url):
    payload = {
        "url": url,
        "async": False,
        "settings": {
            "actions": [
                {"type": "wait", "selector": "img", "timeout": 20000},
                {"type": "capture_dom"}
            ],
            "time_limit": 40000
        }
    }
    print("Capturing DOM URL.")
    response = requests.post("https://api.gaffa.dev/v1/browser/requests", 
        json=payload, headers=HEADERS)
    dom_url = response.json()["data"]["actions"][1]["output"]
    print("Retrieving DOM.")
    dom_response = requests.get(dom_url)
    return dom_response.text
```

**Extract Images and Download with Gaffa**

With the real HTML in hand, we extract image URLs using a simple regex pattern and use Gaffa's [`download_file`](../features/browser-requests/actions/download-file.md) action for secure, reliable downloads. This also allows us to use caching, which avoids downloading the same image over and over again and putting a load on the target server.

```python
def extract_image_urls(dom_content, base_url):
    image_urls = []
    src_pattern = r'<img[^>]+(?:src|data-src)=["\']([^"\']+)["\']'
    matches = re.findall(src_pattern, dom_content)
    
    for src in matches:
        if not src.startswith(('http:', 'https:')):
            src = urljoin(base_url, src)
        image_urls.append(src)
    
    return image_urls

def download_image(image_url, filename):
    payload = {
        "url": image_url,
        "async": False,
        "settings": {
            "actions": [{"type": "download_file"}]
        }
    }
    print("Retrieving download URL.")
    response = requests.post("https://api.gaffa.dev/v1/browser/requests", json=payload, headers=HEADERS)
    actions = response.json()["data"]["actions"]
    download_url = actions[0]["output"]
    download_ext = os.path.splitext(download_url)[1]
    
    print("Downloading image.")
    img_response = requests.get(download_url)
    filepath = f"{filename}{download_ext}"
    with open(filepath, 'wb') as f:
        f.write(img_response.content)

```
{% endstep %}

{% step %}
### Bringing It All Together

The main() function orchestrates the entire workflow: mapping the site, processing each page, and downloading the images using Gaffa's infrastructure.

```python
def main():
    site_url = "https://gaffa.dev"
    sitemap_urls = get_sitemap_urls(site_url)[:3]
    
    for i, url in enumerate(sitemap_urls, 1):
        dom_content = get_dom(url)
        image_urls = extract_image_urls(dom_content, url)
        
        if image_urls:
            download_image(image_urls[0], f"image_{i}")

if __name__ == "__main__":
    main()
```
{% endstep %}

{% step %}
### Run the Script

Save the complete code to a file like `gaffa_scrape_images.py` and run it from your terminal:

```sh
python3 gaffa_scrape_images.py
```

Sit back and watch as Gaffa automatically discovers, renders, and scrapes every image from the site using real browsers. The script will create timestamped folders and save all the images there.
{% endstep %}
{% endstepper %}

### Why This Gaffa-Powered Approach is Superior

* **Handles JavaScript-Rendered Content:** Unlike simple HTTP scrapers, Gaffa uses a real browser, so it captures anything that is lazy-loaded by JavaScript.
* **Intelligent Caching:** With \`max\_cache\_age\` set to 24 hours, repeated requests for the same image are served from cache, reducing load on target servers and improving efficiency.
* **Built-in Reliability:** Gaffa's infrastructure handles proxy rotation, request pacing, retries automatically and provides the correct file format directly.
* **Respectful Scraping:** Gaffa's infrastructure is designed for responsible automation. Always check a website's robots.txt and terms of service before scraping, and respect reasonable rate limits.

### Use Cases and Ideas

This technique is useful for far more than just downloading pictures. Here are a few ideas:

* **Competitive Analysis**: Analyze competitors' product photography styles using real browsers.
* **AI/ML Datasets**: Build large, curated image datasets for training computer vision models using ethically sourced images.
* **Website Migration & Audits**: Download all assets from an old site before a migration while minimizing server impact through caching.
* **Archival & Documentation**: Preserve visual evidence for journalism or create backups of a site's visual content using proxies for access.

#### Next Steps

The full script is available on our [GitHub repository](https://github.com/GaffaAI/GaffaPythonExamples/tree/main/scripts/ScrapeAllImages).

Ready to automate your image collection with enterprise-grade infrastructure? [Sign up for Gaffa](https://gaffa.dev/sign-up) and start building today.
