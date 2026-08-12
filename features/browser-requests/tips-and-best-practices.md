# Tips & Best Practices

### Tips

#### Cost and speed

* Set `max_media_bandwidth: 0` when you only want text. It blocks images and videos, keeps the page working, and reduces token usage by up to 43% on media-heavy sites.
* Don't block media when you're screenshotting, printing or snapshotting. Those need the images.
* Turn on `block_ads` on ad-heavy sites, and add a [`selector`](selectors.md) to [`generate_markdown`](actions/generate-markdown.md) and [`parse_json`](actions/parse-json.md) so you only pay for the part you want.
* Use `max_cache_age` while you're developing, so re-running the same request doesn't cost you each time.

#### Proxies

* Leave `proxy_location` unset, and the request goes out from a data centre IP. Set it when a site blocks those, or serves different content by country.
* Match the location to the content you want: us for `US` pricing, `fr` for French listings.
* Assume a different IP on every request. They rotate, and proxies need a paid account.

#### Reliability and debugging

* Add up your action timeouts before setting `time_limit`. A single [`scroll`](actions/scroll.md) can run for 20 seconds.
* Set `continue_on_fail: true` on optional actions, so one missing element doesn't cost you the whole request.
* Add a [`capture_screenshot`](actions/capture-screenshot.md) between actions to find the step that broke.
* Turn on `record_request` when something fails, and you can't tell why, then turn it back off. It costs extra.

### FAQs

#### How do I reduce my Gaffa credit usage?

Set `max_media_bandwidth: 0` to skip images and video, which saves up to 43% of tokens on media-heavy sites. Add a [`selector`](selectors.md) to narrow captures, and use [`parse_table`](actions/parse-table.md) instead of [`parse_json`](actions/parse-json.md) for tables.

#### When should I not block media?

Don't block it when you're capturing screenshots, printing to PDF, taking a snapshot, or checking anything visual. All of those need the images to actually load.

#### Why does a site behave differently when I block media?

Setting `max_media_bandwidth: 0` stops images from loading, and some sites read that as an ad blocker. Raise the limit to a small figure like 5 rather than blocking outright.

#### How does caching work in Gaffa?

Set `max_cache_age` in milliseconds to say how old a result you'll accept. If anyone has made the same request within that window, you get their response immediately, at no additional bandwidth cost.

#### Why didn't my request come back from the cache?

A cached result is returned only when the URL, parameters, and actions all match a previous request exactly. Any difference, however small, means the request runs fresh.

#### When do I need to use a proxy?

Use one when a site blocks datacenter traffic, or when content changes by country — regional pricing, local search results, geo-restricted pages. Set `proxy_location` to the country you want to appear from.

#### Which proxy locations can I use?

United States (us), Ireland (ie), Singapore (sg) and France (fr). All proxy IPs are residential addresses. Proxies require a paid account.

#### **Can I keep the same IP across several requests?**

No. Each request is treated separately, and IPs rotate, so assume a different address every time. If a task requires continuity, do it all in a single browser request.

#### What does the time\_limit setting do?

It caps the maximum duration the whole request can run, in milliseconds. When the time runs out, unfinished actions are cancelled and the request errors. It can't exceed your plan's maximum runtime.

#### Why does my request keep timing out?

Usually one slow action is using the whole budget, often a [`scroll`](actions/scroll.md) with a long `max_scroll_time` or a [`wait`](actions/wait.md) for an element that never appears. Check those before raising the limit.

#### How do I debug a browser request that isn't working?

Turn on `record_request` to watch it back as video, add [`screenshots`](actions/capture-screenshot.md) between actions to find the failing step, and run a [`capture_dom`](actions/capture-dom.md) to confirm your selectors exist on the page.

#### Why did my request return an error partway through?

By default, one failed action stops everything after it. Set `continue_on_fail: true` on actions that are allowed to fail, so the rest still run and you keep the results.
