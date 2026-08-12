# Capture Element

{% include "../../../.gitbook/includes/beta-feature.md" %}

**Type**: `capture_element`

Returns the [innerHTML](https://developer.mozilla.org/en-US/docs/Web/API/Element/innerHTML) of the element matching your selector. Use it when you need an element's contents, rather than the entire page.

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
