# Type

**Type**: `type`

Request that the browser enter a specific piece of text into a field.

### Parameters

<table data-full-width="false"><thead><tr><th width="212">Name</th><th width="130">Type</th><th width="108" data-type="checkbox">Required</th><th>Description</th></tr></thead><tbody><tr><td><code>selector</code></td><td><code>string</code></td><td>true</td><td>The <a href="../selectors.md">selector</a> that defines the page element that the browser should click on.</td></tr><tr><td><code>text</code></td><td><code>string</code></td><td>true</td><td>The text the browser should enter into the text field.</td></tr></tbody></table>

See [universal parameters](./#universal-parameters).

{% hint style="info" %}
Sites that use more advanced bot detection often use keyboard events to detect unusual activity on their site, rather than immediately dropping all characters of the text into a field, our platform types the text in a human-like manner.
{% endhint %}

### Usage

#### Type into a text box

The following action will type into a particular text field.

```json
"actions": [
      {
            "type": "type",
            "selector": "#postform-text",
            "text": "Hello world!"
      }
]
```
