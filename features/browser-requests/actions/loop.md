# Loop

{% hint style="danger" %}
Beta feature: This action is available on all plans but is still in beta, so the behaviour and parameters are subject to change.If you have any feedback, please email [support](https://gaffa.dev/contact).
{% endhint %}

**Type**: `loop`

Repeat a sequence of nested actions, either for a fixed number of iterations or until one of them fails.

This is how you capture multiple pages in a single browser request. Rather than sending a request per page, you nest your actions inside a `loop` and Gaffa runs them over and over, so a single request can paginate through a set of results, click through a numbered list, or keep interacting with dynamic content until it runs out.

<details>

<summary><strong>Tips for using the <code>loop</code> action</strong></summary>

* Set `timeout` explicitly. The 20-second default covers the whole loop, not each iteration, and most pagination runs need considerably more than that.
* Put capture actions before the action that navigates. A failure skips everything after it in that pass, so a capture placed after the click never runs on the final page.
* Give each nested action a `custom_id`. Everything comes back in one flat list, and an id is the most reliable way to tell which step produced which output.
* Handle cookie banners and consent popups outside the loop, with `continue_on_fail: true`. They only appear once, so there's no reason to retry them on every pass.
* Set `continue_on_fail: true` on the loop if you have actions queued after it; otherwise, a normal pagination-ending stop the request.
* Wait for the pagination control to exist before the loop starts. If the first iteration runs before the page has rendered, the click fails, and the loop exits on pass one.
* Keep `max_iterations` close to the real page count. A high value combined with a bad selector means the browser keeps clicking until the timeout catches it.

</details>

### Parameters

<table><thead><tr><th width="173.61932373046875">Name</th><th width="139.83172607421875">Type</th><th width="110.1640625" data-type="checkbox">Required</th><th>Description</th></tr></thead><tbody><tr><td><code>actions</code></td><td><code>action[]</code></td><td>true</td><td>The ordered list of actions to run on every iteration. Nested actions are written exactly like the actions in your request and support the full set of action types, with the exception of <code>loop</code> itself.</td></tr><tr><td><code>max_iterations</code></td><td><code>integer</code></td><td>false</td><td>The upper bound on how many times the loop can run. Also acts as the iteration count when <code>iterations</code> is not set.<br><strong>Default: 10</strong><br><strong>Min:</strong> 1<br><strong>Max:</strong> 1,000</td></tr><tr><td><code>iterations</code></td><td><code>integer</code></td><td>false</td><td>A fixed number of times to run the nested actions. If you send this alongside <code>max_iterations</code>, the loop runs whichever of the two is lower. <strong>Min:</strong> 1<br><strong>Max:</strong> 100</td></tr><tr><td><code>timeout</code></td><td><code>integer</code></td><td>false</td><td><p>The maximum amount of time the whole loop can run before it is cancelled, in milliseconds. This covers every iteration combined, not each one. </p><p><strong>Default: 20,000 (20s)</strong></p></td></tr><tr><td><code>stop_on_fail</code></td><td><code>boolean</code></td><td>false</td><td>Whether the loop should exit when a nested action fails. A nested action with its own <code>continue_on_fail</code> set to <code>true</code> does not trigger this.<br><strong>Default:</strong> <code>true</code></td></tr><tr><td><code>continue_on_fail</code></td><td><code>boolean</code></td><td>false</td><td>Whether the loop itself is reported as a success when it exits early. Set it to <code>false</code> to pass the failure up to the parent action list, or <code>true</code> to treat an early exit as a clean finish.<br><strong>Default:</strong> <code>false</code></td></tr></tbody></table>

See [universal parameters](./#universal-parameters).

{% hint style="warning" %}
The default `timeout` is 20 seconds for the entire loop, not per iteration. That is short for most pagination jobs, so set it explicitly to something higher than the combined time of all your iterations. Bear in mind the whole request is still capped by your plan's [`time_limit`](../settings.md#time-limit), so a loop `timeout` above that won't buy you extra time.
{% endhint %}

### How the loop ends

A loop stops for one of three reasons:

* It reaches its iteration count.
* A nested action fails while `stop_on_fail` is `true`.
* It hits its `timeout`.

The second one is what makes pagination work. You don't need to know how many pages a site has. You point `click` at the "next" control, set a safe `max_iterations`, and when there is no next page left, the selector matches nothing, the click fails, and the loop exits.

### What happens when a nested action fails

When a nested action fails without `continue_on_fail: true` of its own, the rest of that iteration is abandoned. Every action positioned after it is skipped for that pass, and `stop_on_fail` then decides what the loop does next: exit, or start the following iteration.

Setting `continue_on_fail: true` on a nested action changes this. The failure is treated as expected, so the remaining actions in that pass still run and the loop carries on regardless of `stop_on_fail`.

This is why capture actions belong before the action that navigates, never after it. A capture placed after the click that ends the loop never runs on the final page.

<table data-header-hidden><thead><tr><th width="166.59515380859375"></th><th width="162.758544921875"></th><th width="172.38421630859375"></th><th></th></tr></thead><tbody><tr><td>Nested action <code>continue_on_fail</code></td><td>Loop <code>stop_on_fail</code></td><td>Loop <code>continue_on_fail</code></td><td>Result</td></tr><tr><td><code>true</code></td><td>either</td><td>either</td><td>The failure is ignored. The rest of the iteration runs and the loop continues.</td></tr><tr><td><code>false</code></td><td><code>true</code></td><td><code>false</code></td><td>The rest of the iteration is skipped. The loop exits as failed. The parent action list stops unless it has <code>continue_on_fail: true</code>.</td></tr><tr><td><code>false</code></td><td><code>true</code></td><td><code>true</code></td><td>The rest of the iteration is skipped. The loop exits successfully. The parent action list continues.</td></tr><tr><td><code>false</code></td><td><code>false</code></td><td>either</td><td>The rest of the iteration is skipped. The loop moves to the next iteration and eventually succeeds.</td></tr></tbody></table>

### When a loop times out

A loop that hits its `timeout` is reported with `action_timed_out` and treated the same as any other failure, so its `continue_on_fail` decides whether the request carries on. Iterations already completed are kept, along with their outputs, and any actions finished during the final incomplete pass are recorded too. The `iterations` count only includes passes that ran to completion.

### Actions that come after the loop

A pagination loop normally ends on a failed click, which is a successful run rather than a broken one. If you have further actions queued after the loop, leaving `continue_on_fail` as `false` means those actions never execute and the request returns an `action_failed` error. Set it to `true` when you want the request to carry on, for example to screenshot the final page once the loop is done:

```json
"actions": [
    {
      "type": "loop",
      "max_iterations": 20,
      "timeout": 300000,
      "stop_on_fail": true,
      "continue_on_fail": true,
      "actions": [
        { "type": "capture_dom" },
        { "type": "click", "selector": "a.next-page", "timeout": 15000 }
      ]
    },
    { "type": "capture_screenshot" }
]
```

Here the loop runs until the next-page link is gone, exits cleanly, and the screenshot is still taken. But with `continue_on_fail` set to `false` the screenshot would appear in the response marked `action_cancelled` instead.

### Choosing max\_iterations

<table><thead><tr><th width="330.438232421875">Scenario</th><th>Suggested value</th></tr></thead><tbody><tr><td>Known, fixed page count</td><td>The exact count</td></tr><tr><td>Unknown but bounded site</td><td>A conservative upper bound, for example 50 to 100</td></tr><tr><td>Open-ended, as a safety net</td><td>1,000, combined with a tight <code>timeout</code></td></tr></tbody></table>

Set it high enough to cover the real page count, but not so high that a bad selector leaves the browser clicking for minutes. The `timeout` is your primary safety valve for open-ended loops, and requests are also bound by the [maximum running time](../settings.md#time-limit) on your account, which varies by plan.

### Usage

#### Paginate until the next button disappears

The pattern below is the one you'll reach for most often. Capture the page, click through to the next one, and let the failing click end the loop.

```json
"actions": [
    {
      "type": "loop",
      "max_iterations": 20,
      "timeout": 300000,
      "stop_on_fail": true,
      "continue_on_fail": false,
      "actions": [
        { "type": "wait", "time": 1000 },
        { "type": "scroll", "percentage": 100 },
        { "type": "capture_dom" },
        {
          "type": "click",
          "selector": "a.next-page",
          "timeout": 15000
        }
      ]
    }
]
```

Note the order. Each iteration takes a snapshot of the page it is currently on and then navigates away. A capture placed after the click would never run on the final pass, because the click that ends the loop takes the rest of the iteration with it.

#### Dismiss banners, then paginate

Consent banners and cookie notices only appear once, so handle them outside the loop rather than on every pass. Give each one `continue_on_fail: true` so the request keeps going when the banner isn't there.

```json
{
  "url": "https://www.airfleets.net/listing/a350-1.htm",
  "async": false,
  "max_cache_age": 0,
  "settings": {
    "record_request": true,
    "actions": [
      {
        "type": "click",
        "selector": "input.buttonform[value=\"I'am not a robot\"]",
        "timeout": 10000,
        "continue_on_fail": true
      },
      {
        "type": "click",
        "selector": "button.qc-usp-close-icon",
        "timeout": 10000,
        "continue_on_fail": true
      },
      {
        "type": "click",
        "selector": "button[aria-label='Close success modal']",
        "timeout": 10000,
        "continue_on_fail": true
      },
      {
        "type": "loop",
        "stop_on_fail": true,
        "continue_on_fail": false,
        "max_iterations": 2,
        "timeout": 300000,
        "actions": [
          { "type": "wait", "time": 1000 },
          { "type": "capture_dom" },
          {
            "type": "click",
            "selector": "a.page:has-text('Next page')",
            "timeout": 15000
          }
        ]
      }
    ],
    "time_limit": 300000,
    "block_ads": true
  }
}
```

The three clicks before the loop each time out on this site because the banners don't appear, but `continue_on_fail: true` means the request carries on regardless. Inside the loop, `a.page:has-text('Next page')` matches the next-page link by its visible text, and the request finishes on page three after two iterations.

#### Click through numbered pagination

Not every site labels its next-page control. Where pagination is a row of numbers, a CSS sibling [`selector`](../selectors.md) takes you from the active page to the next page.

```json
{
  "url": "https://repertoire.hebergement.tourisme.gouv.qc.ca",
  "proxy_location": "us",
  "async": false,
  "max_cache_age": 0,
  "settings": {
    "record_request": true,
    "actions": [
      {
        "type": "wait",
        "selector": "[alt=\"Suivant\"]",
        "timeout": 10000,
        "continue_on_fail": true
      },
      {
        "type": "loop",
        "stop_on_fail": true,
        "continue_on_fail": false,
        "max_iterations": 5,
        "timeout": 300000,
        "actions": [
          { "type": "wait", "time": 1000 },
          { "type": "scroll", "percentage": 100 },
          {
            "type": "click",
            "selector": "#pagination span.active + span",
            "timeout": 15000
          }
        ]
      }
    ],
    "time_limit": 300000,
    "block_ads": true
  }
}
```

`#pagination span.active + span` selects the span immediately after the currently active page indicator. On the last page, that sibling doesn't exist, the click fails, and the loop exits. The `wait` before the loop makes sure the pagination has rendered before the first iteration runs.

This site paginates without changing the URL, so `actual_url` in the response stays on the original address no matter how many pages you move through.

### Response format

The loop is returned as a single action containing an `iterations` count and a nested `actions` array. Adding a `custom_id` to each nested action makes the result far easier to read, since it comes back on every entry unchanged:

```json
{
  "id": "act_VuCHkaVpgkmKZsZ7YwomELdtbEkZCz",
  "type": "loop",
  "custom_id": "pagination-loop",
  "timestamp": "2026-08-11T09:51:27.7874109Z",
  "iterations": 2,
  "actions": [
    {
      "id": "act_VuCHkmUpTy9mZodXsL3hTZvVPUpsnZ",
      "type": "wait",
      "custom_id": "settle",
      "timestamp": "2026-08-11T09:51:17.2365316Z"
    },
    {
      "id": "act_VuCHmS83cSrkua3Wo6wWqCUocE3yZA",
      "type": "capture_dom",
      "custom_id": "page-html",
      "timestamp": "2026-08-11T09:51:20.4515024Z",
      "output": "https://storage.gaffa.dev/brq/dom/brq_VuCHkdpZFrk6SaFnb3Zxw8iskoGaKt/act_VuCHmS83cSrkua3Wo6wWqCUocE3yZA_raw.txt"
    },
    {
      "id": "act_VuCHmSdCjKQ3DSrooqFUp5EHNk6fTh",
      "type": "click",
      "custom_id": "next-page",
      "timestamp": "2026-08-11T09:51:20.9377327Z"
    },
    {
      "id": "act_VuCHmXEa7CUpkb19itmkYDYEWk3R5X",
      "type": "wait",
      "custom_id": "settle",
      "timestamp": "2026-08-11T09:51:24.1053288Z"
    },
    {
      "id": "act_VuCHnE6Z1TkdTE8UBGyyxgn26nauAB",
      "type": "capture_dom",
      "custom_id": "page-html",
      "timestamp": "2026-08-11T09:51:24.1206194Z",
      "output": "https://storage.gaffa.dev/brq/dom/brq_VuCHkdpZFrk6SaFnb3Zxw8iskoGaKt/act_VuCHnE6Z1TkdTE8UBGyyxgn26nauAB_raw.txt"
    },
    {
      "id": "act_VuCHnJVQ3etENxshyaUeHTNqZzem4N",
      "type": "click",
      "custom_id": "next-page",
      "timestamp": "2026-08-11T09:51:24.6279232Z"
    }
  ]
}
```

Every action from every pass is recorded in execution order as a single flat list rather than grouped by iteration. Outputs sit on the individual nested actions, so reading the [`capture_dom`](capture-dom.md) entries, top to bottom, gives you page one, then page two, and so on.

Don't rely on counting entries to work out where one iteration ends and the next begins. A pass that failed partway through, or one interrupted by the loop's `timeout` records only the actions it completed, which leaves an uneven final group.

### FAQs

#### When do I use the loop action?

Use it when you need more than one page from a single browser request. Common cases include paginated search results, numbered lists, and feeds where you repeat the same few actions until the content runs out.

#### How do I paginate a site when I don't know how many pages there are?

Point a [`click`](click.md) at the next-page control and leave `stop_on_fail` as `true`. When you reach the last page, the selector matches nothing, the click fails, and the loop exits on its own. Use `max_iterations` as a safety ceiling rather than an exact count.

#### How do I capture each page inside a loop?

Add a capture action such as [`capture_dom`](capture-dom.md) to the nested `actions` array, positioned before the action that navigates. It runs once per iteration, and each capture appears as a separate entry with its own output URL in the response.

#### Why does my request return action\_failed when the loop did what I wanted?

Because the loop ended on a failed click, which is the normal way pagination finishes, and the loop's `continue_on_fail` defaults to `false`. Set it to `true` to treat a clean early exit as a success. Your captured pages are in the response either way.

#### What is the difference between iterations and max\_iterations?

`max_iterations` is the ceiling and defaults to 10. `iterations` is an optional fixed count. Send both, and the loop runs whichever is lower. Send neither, and the loop runs 10 times.

#### Why did an action inside my loop not run?

An action is skipped when something before it in the same iteration failed. That happens regardless of `stop_on_fail`, which only controls whether the loop then exits or starts the next pass. To keep the remaining actions running, set `continue_on_fail: true` on the action that is failing.

#### How do I stop a loop from running too long?

Set the loop's `timeout`, which caps the total time across all iterations. Combine it with a realistic `max_iterations` and a `timeout` on each nested action so no single step can stall the whole run.

#### How do I tell which captured page came from which iteration?

Give each nested action a `custom_id`. It's returned unchanged on every entry, so you can read the flat list as repeating groups. Counting entries alone isn't reliable, because an iteration cut short by a failure or by the loop's `timeout` records only the actions it completed.

#### Can I put a loop inside another loop?

No. Nesting a `loop` inside another `loop` is rejected before the request runs, with an `invalid_action_param` error. For two-level pagination, such as a list of categories, each with its own paged results, send a separate request per category, with a single loop inside each.
