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

This value cannot exceed your plan's maximum request runtime. If omitted, it defaults to 60 seconds irrespective of your plan.

### Plan limits

<table data-header-hidden><thead><tr><th width="150.75994873046875"></th><th width="212.4432373046875"></th><th></th></tr></thead><tbody><tr><td><strong>Plan</strong></td><td><strong>Maximum <code>time_limit</code> (async)</strong></td><td><strong>Maximum <code>time_limit</code> (sync)</strong></td></tr><tr><td>Pay As You Go</td><td>60,000 ms (60s)</td><td>60,000 ms (60s)</td></tr><tr><td>Starter</td><td>60,000 ms (60s)</td><td>60,000 ms (60s)</td></tr><tr><td>Startup</td><td>120,000 ms (2 min)</td><td>120,000 ms (2 min)</td></tr><tr><td>Growth</td><td>300,000 ms (5 min)</td><td>120,000 ms (2 min) — clamped</td></tr></tbody></table>

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

<details>

<summary><strong>Time limit edge cases with examples</strong></summary>

#### Scenario 1: a request hits \`time\_limit\` before finishing

If an action is still running when `time_limit` is reached, the request is cancelled where it stands rather than left to finish. This applies identically whether the request is sync or async — for async, you'll see it on your next poll; for sync, it's what comes back directly on the call.

Request:

```json
{
  "url": "https://demo.gaffa.dev/simulate/article?loadTime=1&paragraphs=5&images=1",
  "async": false,
  "max_cache_age": 0,
  "settings": {
    "time_limit": 60000,
    "actions": [
      { "type": "wait", "time": 90000 }
    ]
  }
}
```

Response:

```json
{
  "data": {
    "id": "brq_VwPVD9DXWJg6ogGFkK2mYmDJhhGrB9",
    "url": "https://demo.gaffa.dev/simulate/article?loadTime=1&paragraphs=5&images=1",
    "state": "completed",
    "credit_usage": 0,
    "error": "request_timeout",
    "error_reason": "The request hit the maximum allowed time and was cancelled.",
    "http_status_code": 200,
    "from_cache": false,
    "started_at": "2026-09-07T11:06:19.7474958Z",
    "completed_at": "2026-09-07T11:07:20.7592414Z",
    "running_time": "00:01:01.0117456",
    "page_load_time": "00:00:00.4019099",
    "actions": [
      { "id": "act_VwPVD4y9uUEFnDAh8RYf9RF7seMEUG", "type": "wait", "error": "action_cancelled" }
    ]
  }
}
```

`running_time` (61.01s) runs slightly past the declared `time_limit` (60s) before the cutoff registers. So, expect a small grace margin of roughly a second, not an exact-millisecond cutoff. A request cancelled this way isn't billed.

#### Scenario 2: an explicit `time_limit` above your plan's cap

Setting `time_limit` higher than your plan allows is rejected before the request runs at all. It returns a `400`, not a cancelled request later.

Request:

```json
{
  "url": "https://demo.gaffa.dev/simulate/table?loadTime=1&rowCount=20",
  "async": false,
  "max_cache_age": 0,
  "settings": {
    "time_limit": 90000,
    "actions": [
      { "type": "wait", "selector": "table", "timeout": 5000 }
    ]
  }
}
```

Response:

```json
{
  "error": {
    "type": "browser_request",
    "id": "60000",
    "code": "time_limit_too_long",
    "message": "The time limit should be under the ms for your plan"
  }
}
```

#### Scenario 3: no `time_limit` set

Omitting `time_limit` defaults to 60,000 ms (60 seconds) for both sync and async requests, regardless of your plan's actual maximum. You opt in to a longer run explicitly; you don't get it automatically.

</details>

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
