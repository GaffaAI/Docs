# Data Retention

Anytime Gaffa generates and hosts a URL, it keeps it only for a limited time. After that window, it's gone: the URL or ID stops resolving, and you can't recover it.

### When Data Retention Matters

1. **Actions that return a Gaffa-hosted URL.** [`capture_dom`](browser-requests/actions/capture-dom.md), [`capture_screenshot`](browser-requests/actions/capture-screenshot.md),  [`download_file`](browser-requests/actions/download-file.md), [`generate_markdown`](browser-requests/actions/generate-markdown.md), [`print`](browser-requests/actions/print.md), and more return their output as a `storage.gaffa.dev` URL. That file is only available for as long as your plan's retention window. Check each action's own docs page to see whether it returns an output URL.
2. **Async requests (`async: true`).** The request returns an `id`, which you use to fetch the full result later via `GET /v1/browser/requests/{id}`. That lookup only works within your plan's retention window.
3. **Recorded videos (`record_request: true`).** The video is hosted the same way, and only available for the same window.

### Retention by plan

| Plan    | Retention period |
| ------- | ---------------- |
| Starter | 7 days           |
| Startup | 30 days          |
| Growth  | 3 months         |

### After the window passes

Once the retention period has passed, you can no longer access the URL or ID.&#x20;

{% hint style="info" %}
#### A couple of things worth knowing

* Retention starts from when the request completes, not from when you first fetch the result.
* If you need to keep a result beyond your plan's window, save the response and any output files somewhere of your own before it lapses. You can't extend or recover access after the fact.
{% endhint %}
