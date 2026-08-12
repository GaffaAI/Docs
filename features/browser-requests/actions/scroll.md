# Scroll

**Type**: `scroll`

Request that the browser scrolls to a certain point on the page or, in the case of pages with infinite scrolling, scrolls for a particular amount of time.

<details>

<summary><strong>Tips for using <code>scroll</code> action</strong></summary>

* Use `wait_time` for pages that load more content when you reach the bottom. Gaffa will keep scrolling as the page grows.
* Use `max_scroll_time` to stop an infinite scroll after a set time. It stops the action without failing it.
* Match the speed to the site: `instant` when there's no rate limiting, `slow` when the site throttles fast scrolling, and `instant` with a large `interval` when limits are strict.
* Use [`selector`](../selectors.md) to scroll inside a modal or side panel, since scrolling the page body won't move them.
* Add [`block_dom_removals`](block-dom-removals.md) first if the site drops rows as you scroll past them.
* `percentage` is a position on the page, not a distance. 50 takes you halfway down, 100 to the bottom.
* Scrolling only goes down. There's no way to scroll back up within a request, so plan your action order accordingly.

</details>

### Parameters

<table data-full-width="false"><thead><tr><th width="215">Name</th><th width="130">Type</th><th width="108" data-type="checkbox">Required</th><th>Description</th></tr></thead><tbody><tr><td><code>percentage</code></td><td><code>integer</code></td><td>true</td><td>The percentage the page should scroll up or down (+/-) <br><strong>Range: [-100 - 0 - 100]</strong><br><strong>Default: 100 (% - scroll to bottom)</strong></td></tr><tr><td><code>wait_time</code></td><td>integer</td><td>false</td><td>After arriving at the desired scroll location this the time Gaffa should monitor for changes to the page height before marking the action as succeeded. Read more <a href="scroll.md#wait-time">below</a>.<br><strong>Default: 0</strong></td></tr><tr><td><code>max_scroll_time</code></td><td><code>integer</code></td><td>false</td><td>The maximum amount of time the page should be scrolled for, in milliseconds. After this time passes, the action will be cancelled. This doesn't cause the action to fail.<br><strong>Default: 20,000 (20s)</strong><br></td></tr><tr><td><code>scroll_speed</code></td><td><code>string</code></td><td>false</td><td>The speed which the page should scroll to the desired point. You can read more about this <a href="scroll.md#scroll-speed">below</a>.<br><strong>Default:</strong> <code>medium</code><br><strong>Accepted</strong>: [<code>slow</code>, <code>medium</code>, <code>instant</code>]</td></tr><tr><td><code>interval</code></td><td><code>integer</code></td><td>false</td><td>The amount of time, in milliseconds, that scrolling should pause between scroll events. Read more about this <a href="scroll.md#scroll-speed-and-interval">below</a>.<br><strong>Default</strong>: 0</td></tr><tr><td><code>timeout</code></td><td><code>integer</code></td><td>false</td><td>The maximum amount of time Gaffa will wait for the page to become scrollable <br><strong>Default: 0</strong></td></tr><tr><td><code>selector</code></td><td><code>string</code></td><td>false</td><td>The <a href="../selectors.md">selector</a> that identifies the element to scroll. If not provided, the page body will be scrolled.</td></tr></tbody></table>

See [universal parameters](./#universal-parameters).

### Scroll Speed & Interval

Gaffa gives you flexibility over how fast you scroll down the page, which can be really useful to get around restrictions enforced by some sites that detect and limit fast scrolling. By experimenting with `scroll_speed` and `interval`, you will be able to create the perfect scrolling action for your scenario. The speed settings are as follows:

* `instant`- The page will smoothly scroll to the desired position immediately, useful for sites with no rate limits or loading events caused by scroll actions.
* `medium` - Human-like scrolling at a normal speed to the desired position. Gaffa will scroll in much the same way as you would using a mouse.
* `slow`- Human-like scrolling at a very slow speed to the desired position. The speed is comparable to scrolling while reading a page.

`interval`allows you to adjust the scroll speed further by inserting pauses between scroll events.

{% hint style="info" %}
We've found some sites with infinite scrolling and strict rate limits respond better to `instant` speed scroll events to the bottom of the page with large `intervals` between these scrolls to keep within rate limits.
{% endhint %}

### Wait Time

If `wait_time` is set to 0, and Gaffa arrives at the desired location, then Gaffa will immediately mark the action as succeeded. However, if another value is set, the page will be monitored for the specified duration to check for further expansions. If, during this period, the page expands again, then Gaffa will continue scrolling to the desired location, and the wait will reset.

{% hint style="info" %}
This can be really useful if you find that the site takes some time to load additional items when you reach the bottom of the page, and more items load after the action has succeeded.
{% endhint %}

### Usage

#### Scroll a particular percentage down the page

The following code will scroll halfway down the page.

```json
"actions": [
      {
        "type": "scroll",
        "percentage": 50
      }
]
```

#### Scroll an infinitely scrolling webpage

The following code will scroll to the bottom of the page and then keep scrolling when new content loads for a maximum of 25 seconds, waiting 1 second for new content and scrolling at a slow pace with 1 second between scroll actions.

```json
"actions": [
      {
        "type": "scroll",
        "percentage": 100,
        "scroll_speed": "slow",
        "max_scroll_time": 25000,
        "interval": 1000,
        "wait_time": 1000
      }
]
```

### Read more

<table data-view="cards"><thead><tr><th></th><th data-hidden data-card-target data-type="content-ref"></th><th data-hidden data-card-cover data-type="image">Cover image</th></tr></thead><tbody><tr><td>How to Handle Infinite Scrolling and Dynamic Loading with Gaffa’s Scroll Action</td><td><a href="https://gaffa.dev/blog/how-to-handle-infinite-scrolling-and-dynamic-loading-with-gaffas-scroll-action">https://gaffa.dev/blog/how-to-handle-infinite-scrolling-and-dynamic-loading-with-gaffas-scroll-action</a></td><td><a href="../../../.gitbook/assets/scroll_illustration_1920x1080_V4.jpg">scroll_illustration_1920x1080_V4.jpg</a></td></tr></tbody></table>

### FAQs

#### When do I use scroll action?

Use it when content loads as you move down the page, when you need to reach something further down before clicking it, or when images only load once they're on screen.

#### How do I scrape an infinite scroll page?

Use a `scroll` action with `"percentage": 100`, a `wait_time` so Gaffa keeps going as new content loads, and a `max_scroll_time` to stop it after a set period.

#### What does wait\_time do?

After reaching the position you asked for, Gaffa watches the page for that duration to see if it grows. If it does, scrolling continues, and the wait starts again, until the page stops growing.

#### What does max\_scroll\_time do?

It sets the maximum duration the scroll can run, defaulting to 20 seconds. When that time is up, the action stops. It doesn't fail, so the rest of your actions still run.

#### Which scroll speed should I use?

Use `instant` when the site has no rate limits, `medium` for normal human-like scrolling, and `slow` for sites that throttle fast scrolling. For strict limits, use `instant` with a large `interval`.

#### Can I scroll back up a page with Gaffa?

No. Scrolling only moves down the page. `percentage` accepts values from 0 to 100, and negative values are rejected. Order your actions so you capture what you need on the way down.

#### What does the percentage parameter do on a scroll action?

It sets an absolute position on the page rather than a distance to travel. `"percentage": 50` scrolls halfway down and `100` scrolls to the bottom, wherever the page currently is.

#### How do I scroll inside a modal or side panel?

Set [`selector`](../selectors.md) to the element that scrolls. Without it, Gaffa scrolls the page body, which does nothing when the scrollable part is a panel inside the page.

#### Why does my capture only show the last few rows?

The site is removing rows as they scroll out of view. Add a [`block_dom_removals`](block-dom-removals.md) action before the scroll so every loaded row stays on the page.
