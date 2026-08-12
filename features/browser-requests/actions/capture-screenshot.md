# Capture Screenshot

**Type:** `capture_screenshot`

Takes a screenshot of the current page. You can take a full-screen screenshot of the entire page or just the current view.

<details>

<summary><strong>Tips for using <code>capture_screenshot</code> action</strong></summary>

* Use `fullscreen` to capture the whole page, and `view` to capture only what's visible on screen. `view` is the default.
* Scroll to the bottom first if the page loads images as you scroll (lazy-loading), or they'll appear blank.
* Add a screenshot after each step when a sequence isn't working. It's the quickest way to see where it went wrong.
* When debugging a [`parse_json`](parse-json.md) action, take one first to check the content is actually on the page.

</details>

### Parameters

<table data-full-width="false"><thead><tr><th width="212">Name</th><th width="130">Type</th><th width="108" data-type="checkbox">Required</th><th>Description</th></tr></thead><tbody><tr><td><code>size</code></td><td><code>string</code></td><td>false</td><td>The size of paper the page should be printed to.<br><strong>Default:</strong> <code>view</code><br><strong>Accepted</strong>: <code>["view", "fullscreen"]</code></td></tr></tbody></table>

See [universal parameters](./#universal-parameters).

### Usage

The following captures the current section of the page currently visible in the browser.

```json
"actions": [
    {
        "type": "capture_screenshot",
        "size": "view"
    }
]
```

### Example Output

An example screenshot in `fullscreen` mode.

<figure><img src="../../../.gitbook/assets/GaffaFullHeightScreenshotExample.png" alt=""><figcaption></figcaption></figure>

### FAQs

#### When do I use capture\_screenshot?

Use it when you need to see the page rather than read it — to check that an action worked, monitor how a page looks over time, or feed an image to a vision model.

#### How do I take a full-page screenshot with Gaffa?

Set "`size": "fullscreen"` on the `capture_screenshot` action. The default is `view`, which captures only what's visible on screen and excludes content below the fold.

#### Why are images missing from my full-page screenshot?

The page is probably lazy-loading them as you scroll. Add a [`scroll`](scroll.md) action to 100% with a `wait_time` before the screenshot, so every image comes into view and loads first.

#### Can I screenshot a single element?

Not directly. Screenshots cover the viewport or the full page. To isolate one element, use [`capture_element`](capture-element.md) for its HTML, or scroll it into view and capture with `size: "view"`.
