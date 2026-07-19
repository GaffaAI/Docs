# Wait

**Type**: `wait`

The wait action tells the browser to pause for a specified time or until a particular element appears on the page.

### Parameters

{% hint style="info" %}
**Parameter Requirements:** You must provide either `time` or `selector`. When using `selector`, `timeout` is required.
{% endhint %}

<table data-full-width="false"><thead><tr><th width="214">Name</th><th width="130">Type</th><th>Description</th></tr></thead><tbody><tr><td><code>time</code></td><td><code>integer</code></td><td>The time in milliseconds the browser should wait.</td></tr><tr><td><code>selector</code></td><td><code>string</code></td><td>The css <a href="https://www.w3schools.com/cssref/css_selectors.php">selector</a> for the element to wait for. You must provide a <code>timeout</code> when using <code>selector</code>.</td></tr><tr><td><code>timeout</code></td><td><code>integer</code></td><td>The maximum time in milliseconds to wait for <code>selector</code> to appear. Default: 5,000 (5s). Required when using <code>selector</code>.</td></tr></tbody></table>

See [universal parameters](./#universal-parameters).

### Usage

#### Wait for a particular amount of time

The following code will wait 1 second, then continue to the next action.

**Wait 1 second:**

```json
"actions": [
      {
        "type": "wait",
        "time": 1000,
      }
]
```

#### Wait for a particular element to appear

The following code will wait for a table to appear on the page for up to 5 seconds. If the table has not appeared after 5 seconds, the next action will be executed. If the table has not appeared after 5 seconds, the next action still executes.

**Wait for table to appear:**

```json
"actions": [
      {
        "type": "wait",
        "selector": "table",
        "timeout": 5000,
        "continueOnFail": true
      }
]
```

{% hint style="warning" %}
Using both `time` and `selector` + `timeout` together does not result in an error. But note that when both are provided, `time` takes priority and the browser will proceed after the `time` duration expires, regardless of whether `selector` has appeared, and the `timeout` is ignored.
{% endhint %}
