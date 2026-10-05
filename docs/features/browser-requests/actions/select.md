# Select

**Type**: `select`&#x20;

Use `select` to choose one or more options from a native HTML `<select>` dropdown.

{% hint style="warning" %}
`select` only works with native `<select>` elements. For custom dropdowns built with elements such as `div` or `input[role="combobox"]`, use `click` instead.
{% endhint %}

<details>

<summary><strong>Tips for using <code>select</code> action</strong></summary>

* If the dropdown loads after the page, add a [`wait`](wait.md) before the `select` action.
* Point the `selector` at the `<select>` element itself, not a wrapper around it.
* For each item in `values`, `value` is tried first. `label` is only used when `value` is absent or an empty string. Sending both means `label` is ignored.
* For a multi-select field, every item in `values` is applied to the same `<select>`, picking several options in one dropdown. It doesn't pick one option per matching element on the page.
* A non-matching item is skipped silently rather than failing the whole action. The action succeeds if at least one item matched, and fails only if none did.
* An empty or null `value`/`label` on an item counts as not provided, not as a value to match against.
* `select` only works on a native HTML `<select>`. Custom dropdowns built from `divs` or an `input[role="combobox"]` aren't real select elements; use `click` to open them instead.
* Browsers don't update the HTML `selected` attribute when an option is chosen programmatically; only the live element property changes. So [`capture_dom`](capture-dom.md) and [`capture_element`](capture-element.md) will look unchanged even after a successful select. Use [`capture_screenshot`](capture-screenshot.md) to confirm which option is selected.

</details>

### Parameters

<table><thead><tr><th width="124.78619384765625">Name</th><th width="124.4971923828125">Type</th><th width="108.98291015625" data-type="checkbox">Required</th><th>Description</th></tr></thead><tbody><tr><td><code>selector</code></td><td><code>string</code></td><td>true</td><td>The selector that defines the <code>&#x3C;select></code> element the browser should choose an option from.</td></tr><tr><td><code>values</code></td><td><code>array</code></td><td>true</td><td>A list of objects describing which option(s) to select. See <a href="select.md#values">Values</a> below.</td></tr><tr><td><code>timeout</code></td><td><code>integer</code></td><td>false</td><td>The maximum amount of time the browser should wait for the element defined by the selector to appear. <strong>Default: 5000 (5s)</strong></td></tr></tbody></table>

See [universal parameters](./#universal-parameters).

### Values

The `values` array tells Gaffa which option or options to select. Each item identifies one option using either its HTML `value` attribute or its visible label.

<table><thead><tr><th width="131.6612548828125">Name</th><th width="123.796875">Type</th><th width="112.39984130859375" data-type="checkbox">Required</th><th>Description</th></tr></thead><tbody><tr><td><code>value</code></td><td><code>string</code></td><td>false</td><td>Matches an option using its HTML <code>value</code> attribute.</td></tr><tr><td><code>label</code></td><td><code>string</code></td><td>false</td><td>Matches an option by its visible text. Only used when <code>value</code> is absent or empty.</td></tr></tbody></table>

* Each item must include at least one non-empty `value` or `label`. If both are provided, `value` is used and `label` is ignored.
* For a single-select field, provide one item in `values`. For a multi-select field, provide one item for each option you want selected. All items are applied to the same `<select>` element.
* If an item doesn't match an option, it is skipped. The action succeeds if at least one item matches and fails if none match.

{% hint style="info" %}
Since a browser doesn't rewrite the selected attribute in the underlying HTML when an option is chosen, capturing the DOM won't show you the result of a `select` action. Add a [`capture_screenshot`](capture-screenshot.md) afterwards if you want to confirm which option ended up selected.
{% endhint %}

### Usage

#### Select an option by value

The following selects the option with value `108` from a dropdown.

```json
"actions": [
      {
        "type": "select",
        "selector": "#legislation-legislativeActions",
        "values": [{ "value": "108" }]
      }
]
```

#### Select an option by label

The following selects the option whose visible text is `Laws` .

```json
"actions": [
      {
        "type": "select",
        "selector": "#legislation-legislativeActions",
        "values": [{ "label": "Laws" }]
      }
]
```

#### Select using both value and label

If an item includes both `value` and `label`, `value` is used and `label` is ignored. The following selects the option with the value `108` ("Passed/agreed to in House"), not the option labelled `Laws`, even though both are supplied.

```json
"actions": [
      {
        "type": "select",
        "selector": "#legislation-legislativeActions",
        "values": [{ "value": "108", "label": "Laws" }]
      }
]
```

#### Select multiple options in a multi-select field

The following selects two options, `118` and `117`, in the same multi-select dropdown.

```json
"actions": [
      {
        "type": "select",
        "selector": "#legislation-congresses",
        "values": [{ "value": "118" }, { "value": "117" }]
      }
]
```

### FAQs

#### When do I use the select action?

Use it to choose an option, or several options, in an HTML `<select>` dropdown or multi-select field. For text fields, use [`type`](type.md), and for checkboxes, radio buttons, or custom dropdowns built without a real `<select>`, use [`click`](click.md).

#### How do I select an option from a dropdown?

Add a `select` action with a `selector` for the `<select>` element and a `values` array containing either the option's `value` or its `label`.

#### How do I select multiple options in a multi-select field?

Submit multiple items in `values`, one per option, all pointed at the same [`selector`](../selectors.md). Each one applies to the same field, so you end up with several options selected in one dropdown rather than one option per field.

#### What's the difference between selecting by value and by label?

`value` matches the option's underlying `value` attribute, `label` matches its visible text. If an item includes both, `value` is used and `label` is ignored, `label` only comes into play when `value` is missing or empty.

#### Why did my select action fail?

Either the `selector` didn't match a `<select>` element in time, or none of the items in `values` matched an option on the page. Non-matching items are skipped rather than failing individually; the whole action only fails if nothing in `values` matched anything.

#### How do I know if a select worked?

Add a [`capture_screenshot`](capture-screenshot.md) after the `select` action. Browsers don't update the `selected` attribute in the raw HTML when an option is chosen programmatically, so [`capture_dom`](capture-dom.md) or [`capture_element`](capture-element.md) will still look like nothing changed, even when the select succeeded.

#### Can I use select on a custom dropdown that isn't a real `<select>` element?

No. Many modern sites replace the native control with a JavaScript widget, often an `input[role="combobox"]` with its own list of options. `select` only works against a genuine `<select>` element; for a custom widget, use [`click`](click.md) to open it and click each option instead.
