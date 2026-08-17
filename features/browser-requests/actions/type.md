# Type

**Type**: `type`

Request that the browser enter a specific piece of text into a field.

<details>

<summary><strong>Tips for using <code>type</code> action</strong></summary>

* Typing doesn't submit the form. Add a `click` for the submit button afterwards.
* Add a [`wait`](wait.md) for the input before typing if the form loads after the page does.
* Point the [`selector`](../selectors.md) at the input itself, not the div around it.
* Use single quotes inside attribute [`selectors`](../selectors.md) so your JSON stays valid: `"input[name='email']"`. You can also escape double quotes with backslashes.
* Close any cookie banner or modal first. An overlay can block the field even when your selector is correct.
* Gaffa types character by character rather than pasting the whole string, which suits sites that watch for unusual input.

</details>

### Parameters

<table data-full-width="false"><thead><tr><th width="212">Name</th><th width="130">Type</th><th width="108" data-type="checkbox">Required</th><th>Description</th></tr></thead><tbody><tr><td><code>selector</code></td><td><code>string</code></td><td>true</td><td>The <a href="../selectors.md">selector</a> that defines the text field where the browser should type.</td></tr><tr><td><code>text</code></td><td><code>string</code></td><td>true</td><td>The text the browser should enter into the text field.</td></tr></tbody></table>

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

### FAQs

#### When do I use the type action?

Use it to enter text into a field — a search box, a login form, a filter. For checkboxes, radio buttons and dropdowns, use [`click`](click.md) instead.

#### How do I fill in a form field with Gaffa?

Add a `type` action with a [`selector`](../selectors.md) for the input and the text you want entered. Gaffa types the characters one at a time, as a person would.

#### Does the type action submit the form?

No. It only enters the text. Add a [`click`](click.md) for the submit button afterwards, then a [`wait`](wait.md) for something on the next page before you capture anything.

#### How do I fill in more than one field?

Add one [`type`](type.md) action per field, in the order someone would fill them in. Finish with a [`click`](click.md) on the submit button and a [`wait`](wait.md) for the result.

#### Why isn't my type action working?

Usually the [`selector`](../selectors.md) points at the wrapper rather than the input, the field hasn't loaded yet, or a cookie banner or modal is sitting over it. Target the `input` directly, add a [`wait`](wait.md) for slow forms, and close any overlay first.

#### How do I write an attribute selector inside JSON?

Use single quotes inside the [`selector`](../selectors.md), like `"input[name='email']"`, or escape the double quotes with backslashes: `"input[name=\"email\"]"`. Unescaped double quotes will break the request.

#### Does Gaffa's typing avoid bot detection?

Gaffa types character by character with real keyboard events instead of dropping the whole string in at once, which handles sites that watch for that. It won't defeat every detection method.
