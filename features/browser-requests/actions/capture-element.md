# Capture Element

**Type**: `capture_element`

Returns the [outerHTML](https://developer.mozilla.org/en-US/docs/Web/API/Element/outerHTML) of the element matching your selector, the element itself, and everything inside it. Use it when you need an element's contents, rather than the entire page.

<details>

<summary><strong>Tips for using <code>capture_element</code> action</strong></summary>

* Use this instead of [`capture_dom`](capture-dom.md) when you know where the content is. Pointing at main or article cuts out navigation and footers for you.
* You don't need a [`wait`](wait.md) first. This action already waits for its own selector, up to 5 seconds by default, and you can change that with `timeout`.
* The action captures as soon as the element appears, so a longer `timeout` doesn't slow anything down. It's a ceiling, not a delay.
* Increase the `timeout` for pages that load content after the initial render, so the action doesn't give up before the element exists.
* It returns the element and everything inside it, including its opening and closing tags.
* If your selector matches more than one element, only the first is captured.

</details>

### Parameters

<table data-full-width="false"><thead><tr><th width="212">Name</th><th width="130">Type</th><th width="108" data-type="checkbox">Required</th><th>Description</th></tr></thead><tbody><tr><td><code>selector</code></td><td><code>string</code></td><td>true</td><td>The <a href="../selectors.md">selector</a> that defines the element whose contents you want to capture.</td></tr><tr><td><code>timeout</code></td><td><code>integer</code></td><td>false</td><td>The maximum amount of time the browser should wait for the element defined by the selector to appear. <strong>Default: 5000 (5s)</strong></td></tr></tbody></table>

See [universal parameters](./#universal-parameters).

### Usage

The following code will wait 1 second for the `.page_contents` element to appear and return an HTML file containing the div's innerHTML.

```json
"actions": [
    {
      "type": "capture_element",
      "selector": ".page_contents",
      "timeout": 1000
    }
]
```

### FAQs

#### When do I use the capture\_element action?

Use it when you want the HTML for a specific part of a page and already know its [`selector`](../selectors.md): a results list, an article body, or a product panel. It's smaller and cleaner than capturing the whole DOM.

#### What does the capture\_element action return?

It returns the matched element as an HTML file, including the element's own opening and closing tags. Capturing a `table` gives you the complete `<table>` element, not just its contents.

#### How long does capture\_element wait for the element?

Up to five seconds by default, configurable with `timeout` in milliseconds. It captures as soon as the element appears, so increasing the timeout incurs no cost on pages where the element is already present.

#### Do I need a wait action before capture\_element?

No. It already waits for its selector using `timeout`. Adding a separate wait on the same element just slows the request.

#### What happens if my selector doesn't match anything?

The action fails once the timeout is reached, and by default that stops the request. Set `continue_on_fail: true` if the element is optional, and use [`capture_dom`](capture-dom.md) to check what the [`selector`](../selectors.md) should be.

#### When should I use generate\_markdown instead of capture\_element?

Use [`generate_markdown`](generate-markdown.md) with a [`selector`](../selectors.md) when you want that part of the page as readable text. Use `capture_element` when you need the HTML exactly as it is, with the tags and attributes intact.

#### What happens if my selector matches more than one element?

Only the first match is captured. To get several elements, select a shared container instead, or add a separate `capture_element` action for each one.
