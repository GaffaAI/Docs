# Click

**Type**: `click`

Request that the browser click a particular element on the page.

### Parameters

<table data-full-width="false"><thead><tr><th width="212">Name</th><th width="130">Type</th><th width="108" data-type="checkbox">Required</th><th>Description</th></tr></thead><tbody><tr><td><code>selector</code></td><td><code>string</code></td><td>true</td><td>The <a href="https://www.w3schools.com/cssref/css_selectors.php">selector </a>that defines the page element that the browser should click on.</td></tr><tr><td><code>timeout</code></td><td><code>integer</code></td><td>false</td><td>The maximum amount of time the browser should wait for the element defined by the selector to appear. <strong>Default: 5000 (5s)</strong></td></tr></tbody></table>

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

The following waits up to 5 seconds for the logo, then clicks it. It continues if the click fails.

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
