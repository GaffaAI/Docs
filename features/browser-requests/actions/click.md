# Click

**Type**: `click`

Request that the browser click a particular element on the page.

<details>

<summary><strong>Tips for using <code>click</code> action</strong></summary>

* Pick [`selectors`](../selectors.md) that won't break, like id, data-testid or aria-label. Avoid generated class names and nth-child.
* You don't need a [`wait`](wait.md) first. `click` already waits up to 5 seconds for its element.
* Set `continue_on_fail: true` when closing cookie banners and pop-ups, since they aren't always there.
* After a click that loads new content, add a [`wait`](wait.md) for something in that new content before you capture.
* If a click isn't working, add a screenshot after it to see what the page did.

</details>

### Parameters

<table data-full-width="false"><thead><tr><th width="212">Name</th><th width="130">Type</th><th width="108" data-type="checkbox">Required</th><th>Description</th></tr></thead><tbody><tr><td><code>selector</code></td><td><code>string</code></td><td>true</td><td>The <a href="../selectors.md">selector</a> that defines the page element that the browser should click on.</td></tr><tr><td><code>timeout</code></td><td><code>integer</code></td><td>false</td><td>The maximum amount of time the browser should wait for the element defined by the selector to appear. <strong>Default: 5000 (5s)</strong></td></tr></tbody></table>

See [universal parameters](./#universal-parameters).

### Usage

#### Click an element on the page

The following clicks the page logo.

```json
"actions": [
    {
      "type": "click",
      "selector": "a.header__logo"
    }
]
```

#### Click an element when it appears

The following waits up to 5 seconds for the logo to appear, then clicks it. It continues if the click fails.

```json
"actions": [
      {
        "type": "click",
        "selector": "a.header__logo",
        "timeout": 5000,
        "continue_on_fail": true
      }
]
```

### FAQs

#### When do I use the click action?

Use it whenever you need the browser to interact with something — a button, a link, a tab, a checkbox, or a "load more" control. Anything a user would click.

#### How do I click a button with Gaffa?

Add a `click` action with a CSS selector for the button. Gaffa waits up to five seconds for it to appear before clicking. Change that with the `timeout` parameter.

#### What selector should I use for a click?

Use something stable like `id`, `data-testid` or `aria-label`. Generated class names and positional selectors like `nth-child` break as soon as the site changes.

#### How do I close a cookie banner before scraping a page?

Add a `click` for the accept button with `continue_on_fail: true`, before your other actions. That flag stops the request failing on pages where the banner doesn't show.

#### How do I wait for content to load after a click?

Follow the click with a [`wait`](wait.md) action using a selector that only exists in the new content, plus a `timeout`. That's more reliable than waiting a fixed number of seconds.

#### Why did my click fail?

Usually the [`selector`](../selectors.md) didn't match in time, or the element was there but covered by a pop-up. [Capture the DOM](capture-dom.md) to check the selector, and close any overlay first.
