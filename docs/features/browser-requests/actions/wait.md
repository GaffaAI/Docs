# Wait

**Type**: `wait`

The wait action tells the browser to pause for a specified time or until a particular element appears on the page.

<details>

<summary><strong>Tips for using <code>wait</code> action</strong></summary>

* Wait for a [`selector`](../selectors.md) rather than a fixed `time` wherever you can. It moves on as soon as the element appears.
* Never set both `time` and [`selector`](../selectors.md). If you do, time wins and the timeout is ignored completely.
* Set `continue_on_fail: true` when the element you're waiting for might not appear.
* You don't need a `wait` before [`click`](click.md), [`capture_element`](capture-element.md) or [`parse_table`](parse-table.md), since they wait for their own selectors.
* A fixed `time` is still the right choice for animations settling or for slowing a sequence down on purpose.

</details>

### Parameters

{% hint style="info" %}
**Parameter Requirements:** You must provide either `time` or `selector`.&#x20;
{% endhint %}

<table data-full-width="false"><thead><tr><th width="149.085205078125">Name</th><th width="130">Type</th><th>Description</th></tr></thead><tbody><tr><td><code>time</code></td><td><code>integer</code></td><td>The time in milliseconds the browser should wait.</td></tr><tr><td><code>selector</code></td><td><code>string</code></td><td>The <a href="../selectors.md">selector</a> for the element to wait for.</td></tr><tr><td><code>timeout</code></td><td><code>integer</code></td><td>The maximum time in milliseconds to wait for <code>selector</code> to appear. Default: 5,000 (5s). </td></tr></tbody></table>

See [universal parameters](./#universal-parameters).

### Usage

#### Wait for a particular amount of time

The following code will wait 1 second, then continue to the next action.

**Wait 1 second:**

```json
"actions": [
      {
        "type": "wait",
        "time": 1000
      }
]
```

#### Wait for a particular element to appear

The following code will wait for a table to appear on the page for up to 5 seconds. If the table has not appeared after 5 seconds, the next action will be executed.

**Wait for table to appear:**

```json
"actions": [
      {
        "type": "wait",
        "selector": "table",
        "timeout": 5000,
        "continue_on_fail": true
      }
]
```

`timeout` is shown explicitly above for clarity, but it isn't required. Leaving it out gives you the same 5-second wait:

```json
"actions": [
      {
        "type": "wait",
        "selector": "table",
        "continue_on_fail": true
      }
]
```

{% hint style="warning" %}
Using both `time` and `selector` + `timeout` together does not result in an error. But note that when both are provided, `time` takes priority and the browser will proceed after the `time` duration expires, regardless of whether `selector` has appeared, and the `timeout` is ignored.
{% endhint %}

### FAQs

#### When do I use the wait action?

Use it when you need to pause between actions — waiting for content to load after a click, for an animation to finish, or for a slow page to finish rendering before you capture it.

#### How do I wait for an element to appear?

Add a wait action with a [`selector`](../selectors.md) for the element. Gaffa waits up to 5 seconds by default and continues as soon as the element appears, or set `timeout` to change how long it waits.

#### How do I pause for a fixed amount of time?

Add a `wait` action with `time` set in milliseconds. Use this for animations or deliberate pacing, rather than for content that might take a variable amount of time to load.

#### What happens if I set both time and selector?

`time` takes over and `timeout` is ignored. The browser moves on once the time is up, whether or not the element appeared. Use one or the other, never both.

#### Should I wait for a selector or a fixed time?

Wait for a [`selector`](../selectors.md) where you can. It moves on as soon as the element exists, instead of always using the full time, which makes sequences quicker and more reliable.

#### What if the element never appears?

The action fails once the timeout is up, and by default the request stops. Set `continue_on_fail: true` if the element is optional and the rest should still run.

#### Do I need to set a timeout when using selector?

No. `timeout` defaults to 5,000ms if you leave it out. Set it explicitly only when you want a shorter or longer wait than that.&#x20;

#### Do I need a wait before every capture?

No. Add one only when the content loads after the previous action finishes. [`click`](click.md), [`capture_element`](capture-element.md) and [`parse_table`](parse-table.md) already wait for their own [`selectors`](../selectors.md).
