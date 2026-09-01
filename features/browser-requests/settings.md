# Settings

The `settings` object in your browser request allows you to configure various aspects of how your automation behaves. Below are all the available settings parameters you can use.

***

## Screen Recording

**Parameter:** `record_request` (boolean)

By specifying `record_request`, you can ask Gaffa to screen record your automation and return a video in the response, allowing you to view the magic happening or to debug your automation.

Recording requests come at an [additional cost](../../credits-and-pricing.md).

**Example:**

```json
{
  "url": "https://example.com",
  "settings": {
    "record_request": true,
    "actions": [...]
  }
}
```

***

## Max Media Bandwidth

**Parameter:** `max_media_bandwidth` (integer or null)

If you're using Gaffa on a site with lots of images and videos but are more interested in the text data on the page, you can cap how much media content a page loads using the `max_media_bandwidth` setting. This makes your automation faster and prevents spending credits on data you aren't interested in.

### Setting Options

You can set `max_media_bandwidth` in three ways:

* `"max_media_bandwidth": 0` — Block all images and videos completely
* `"max_media_bandwidth": 5` — Cap media downloads at 5MB (or any number you specify)
* `"max_media_bandwidth": null` — No limit (default)

### How It Works

When the `max_media_bandwidth` value is set, Gaffa monitors the data being downloaded by the page. When the downloaded media exceeds the specified MB limit, any further downloads of images or videos will be cancelled.

{% hint style="info" %}
**Important:** When enabled, only image and video downloads are blocked. HTML, CSS, JavaScript, and other essential page resources load normally, preserving functionality.
{% endhint %}

### Common Use Cases

This setting is particularly useful for:

* **Scraping news articles for text only** — Extract headlines and article content without downloading thumbnails
* **E-commerce price monitoring** — Track product prices and descriptions without loading product images
* **Extracting reviews and text content** — Capture customer reviews without profile pictures
* **SEO and content analysis** — Analyze page structure, headings, and text without media files

{% hint style="success" %}
**Performance Benefits:** Testing on image-heavy news sites showed up to **43% token savings** with no loss of text data. Sites with more media content see even greater savings in both cost and request speed.
{% endhint %}

{% hint style="warning" %}
**When NOT to Use: Not recommended for capturing screenshots, verifying images, or analysing visual content.**
{% endhint %}

### Getting Started

Start with `max_media_bandwidth: 0` for maximum savings, then adjust upward only if you encounter issues with specific sites. Setting a value of `0` will cause no images to load, which works well on most sites, but on some could lead to the site thinking you are using an ad blocker.

**Example:**

```json
{
  "url": "https://www.bbc.com/",
  "settings": {
    "max_media_bandwidth": 0,
    "actions": [
      {
        "type": "generate_markdown"
      }
    ]
  }
}
```

**Learn more:** See our detailed [guide](https://gaffa.dev/blog/how-to-slash-your-gaffa-credit-costs-by-40-percent) on optimizing browser requests with max\_media\_bandwidth, including real-world testing, use cases, and best practices.

***

## Time Limit

**Parameter:** `time_limit` (integer)

Using the `time_limit` setting caps the maximum running time of the request in milliseconds. If this time expires, all incomplete actions will be cancelled, and the request will return an error.

This value cannot exceed your plan's maximum request runtime. If omitted, it defaults to that maximum.

### Plan limits

| Plan          | Maximum `time_limit`   |
| ------------- | ---------------------- |
| Pay As You Go | 60,000 ms (60 seconds) |
| Starter       | 60,000 ms (60 seconds) |
| Startup       | 120,000 ms (2 minutes) |
| Growth        | 300,000 ms (5 minutes) |

**Example:**

```json
{
  "url": "https://example.com",
  "settings": {
    "time_limit": 30000,
    "actions": [...]
  }
}
```

***

## Ad Blocking

**Parameter:** `block_ads` (boolean)

{% hint style="danger" %}
Beta feature: Ad blocking is available to all users but is currently in beta. If you encounter ad networks that aren't being blocked, [get in touch](https://gaffa.dev/contact), and we'll add them.
{% endhint %}

If you are automating or scraping content on ad-heavy websites, third-party ad network requests can slow down your page load significantly, even though you don't need them. By enabling `block_ads` , Gaffa intercepts and immediately aborts requests to known ad-serving domains before they load, reducing page load times without affecting the core page content.

### Setting options

You can set `block_ads` in two ways:

* `"block_ads": false` — Ad blocking disabled (default)
* `"block_ads": true` — Ad blocking enabled

**Example:**

```json
{
  "url": "https://www.allrecipes.com",
  "settings": {
    "block_ads": true,
    "actions": [
      {
        "type": "capture_dom"
      }
    ]
  }
}
```

***

## Redirect Logging

**Parameter:** `log_redirects` (boolean)

If you're automating a flow that passes through one or more redirects before landing on a final page such as an affiliate link, a shortened URL, a tracking pixel, or a marketing campaign link, `log_redirects` captures every URL the browser was sent through along the way, not just the final destination.

By enabling `log_redirects`, Gaffa records each redirect hop encountered during the request, whether triggered by the initial page load or by an action such as a [`click`](actions/click.md), and returns them in the `redirects` field of the response.

### Setting options

* `"log_redirects": false` — Redirect logging disabled (default). The `redirects` field is omitted from the response entirely.
* `"log_redirects": true` — Redirect logging enabled. `redirects` is populated with every hop captured during the request.

**Example**

Tracking the redirect chain behind the marketing email link, from the shortened click-tracking URL through to the final landing page:

```json
{
  "url": "https://link.example-brand.com/e/click?upn=abc123-XYZ",
  "max_cache_age": 0,
  "settings": {
    "log_redirects": true,
    "actions": [
      { "type": "wait", "time": 3000 }
    ]
  }
}
```

The relevant part of the **response**:

```json
{
  "actual_url": "https://www.example-brand.com/products/new-arrivals?utm_source=email&utm_medium=campaign&utm_campaign=spring_launch&subscriber_id=987654",
  "redirects": [
    "https://click.example-esp.com/track/click?upn=abc123-XYZ&sub_id=987654",
    "https://link.example-brand.com/e/click?upn=abc123-XYZ",
    "https://www.example-brand.com/products/new-arrivals?utm_source=email&utm_medium=campaign&utm_campaign=spring_launch&subscriber_id=987654"
  ]
}
```

### Common use cases

* **Marketing campaign QA** — check that UTM parameters and click IDs on an email, ad, or social link survive the full redirect chain instead of getting dropped or overwritten partway through.
* **Affiliate and voucher link auditing** — confirm that a "get code" or "shop now" link actually routes through the correct affiliate network and arrives at the intended retailer page, with tracking parameters (like `utm_source`, click IDs) intact at each hop. Useful for verifying a partner integration is wired up correctly, or diagnosing why commission tracking isn't crediting properly.
* **Redirect chain / SEO auditing** — after a site migration or URL restructure, confirm old URLs resolve to the correct new destination in as few hops as possible. Long or looping redirect chains hurt both page speed and SEO.
* **Link safety verification** — see every intermediate domain a link passes through before landing on its final destination, useful for checking that shortened or obfuscated links (in emails, ads, QR codes) aren't routing through anything unexpected.
* **Debugging unexpected destinations** — when a request ends up somewhere you didn't expect (a geo-redirect, an A/B test split, a broken campaign link), the full chain shows exactly which hop diverged, rather than just the final URL.

***

## Actions

**Parameter:** `actions` (array)

The `actions` parameter defines the specific tasks you want Gaffa to perform on the page once it loads. Actions are executed in the order they appear in your array and can include tasks such as waiting for elements, capturing screenshots, generating Markdown, printing to PDF, and more.

We support different types of actions, each designed for specific automation needs. [Learn more about all available actions here](actions/).

**Example:**

```json
{
  "url": "https://example.com",
  "settings": {
    "actions": [
      {
        "type": "wait",
        "selector": "table"
      },
      {
        "type": "print",
        "size": "A4",
        "margin": 20,
        "orientation": "portrait"
      }
    ]
  }
}
```

***

## Complete Example

Here's a browser request using multiple settings parameters:

```json
{
  "url": "https://www.bbc.com/",
  "proxy_location": "us",
  "async": false,
  "max_cache_age": 0,
  "settings": {
    "record_request": false,
    "max_media_bandwidth": 0,
    "time_limit": 60000,
    "block_ads": true,
    "actions": [
      {
        "type": "wait",
        "selector": "table"
      },
      {
        "type": "print",
        "size": "A4",
        "margin": 20,
        "orientation": "portrait"
      }
    ]
  }
}
```
