# Capture a full-height screenshot of a webpage

In just a few lines of JSON inlined in a single cURL command, you can automate:

* Dismissing Wikipedia’s EU cookie consent banner (if present)
* Waiting for the main heading on the Artificial Intelligence article
* Scrolling through every section (lazy-loaded images and all)
* Capturing a full-page PNG for archiving, visual regression, or documentation

All without installing Playwright or managing headless browsers, Gaffa handles it for you server-side via the[ Browser Requests API](https://gaffa-1.gitbook.io/gaffa/features/browser-requests).

### Prerequisites

* A valid Gaffa API key
* A simple HTTP client (cURL, Postman, axios, etc.).
* Familiarity with the[ API Playground](https://gaffa.dev/dashboard/playground) for testing browser requests.
* Target URL for this tutorial, for this we'll use Wikipedia: [https://en.wikipedia.org/wiki/Artificial\_intelligence](https://en.wikipedia.org/wiki/Artificial_intelligence)

{% stepper %}
{% step %}
### Execute the Request

Use cURL with the full JSON payload inlined to ensure Gaffa receives exactly what you intend:

```sh
curl https://api.gaffa.dev/v1/browser/requests \
  --request POST \
  --header 'Content-Type: application/json' \
  --header 'X-API-Key: YOUR_API_KEY' \
  --data '{
    "url": "https://en.wikipedia.org/wiki/Artificial_intelligence",
    "async": false,
    "max_cache_age": 0,
    "settings": {
      "actions": [
        {
          "type": "wait",
          "selector": "#cookie-policy-notice",
          "timeout": 10000,
          "continue_on_fail": true
        },
        {
          "type": "click",
          "selector": "#cookie-policy-notice",
          "continue_on_fail": true
        },
        {
          "type": "wait",
          "selector": "#firstHeading",
          "timeout": 10000
        },
        {
          "type": "scroll",
          "percentage": 100
        },
        {
          "type": "capture_screenshot",
          "size": "fullscreen"
        }
      ]
    }
  }'
```

Replace YOUR\_API\_KEY with your actual token from your [Dashboard.](https://gaffa.dev/dashboard/api-keys) This command has the following actions:

1. **Wait** (optional): Detect and accept Wikipedia’s cookie banner if it appears. If it fails, that simply means no banner was present, or it did not load in time. Since continue\_on\_fail defaults to true, Gaffa will continue without halting the workflow, ensuring the remaining steps still execute.
2. **Wait**: Ensure the main heading (#firstHeading) is loaded.
3. **Scroll**: Scroll through the entire page to trigger any lazy-loaded content.
4. **Capture** Screenshot: Produce a full-page PNG.
{% endstep %}

{% step %}
### Retrieve Your Screenshot

A successful response returns JSON like:

{% code lineNumbers="true" %}
```json
{
  "data": {
    "id": "brq_VJX3mbESLiyCFYvZQEUih9RdDYovog",
    "url": "https://en.wikipedia.org/wiki/Artificial_intelligence",
    "proxy_location": null,
    "state": "completed",
    "credit_usage": 2,
    "http_status_code": 200,
    "from_cache": false,
    "started_at": "2025-06-09T15:55:46.4235903Z",
    "completed_at": "2025-06-09T15:56:27.9381332Z",
    "running_time": "00:00:40.7348244",
    "page_load_time": "00:00:02.2087117",
    "actions": [
      {
        "id": "act_VJX3memaue6YUgFcn44uNscZbVUpYg",
        "type": "wait",
        "query": "wait?selector=%23cookie-policy-notice%2C%20.mw-cookie-consent-container&timeout=10000&continue_on_fail=true",
        "timestamp": "2025-06-09T15:55:48.6323091Z",
        "error": "action_timed_out"
      },
      {
        "id": "act_VJX3mkwfwNPdGiMUpqKr34Tm5xzyUU",
        "type": "click",
        "query": "click?selector=%23cookie-policy-notice%20button%2C%20.mw-cookie-consent-container%20button&continue_on_fail=true&timeout=5000",
        "timestamp": "2025-06-09T15:55:58.7949275Z",
        "error": "action_timed_out"
      },
      {
        "id": "act_VJX3mkSJ3sevWRXUCjFy6zwfD172fV",
        "type": "wait",
        "query": "wait?selector=%23firstHeading&timeout=10000&continue_on_fail=false",
        "timestamp": "2025-06-09T15:56:03.9581113Z"
      },
      {
        "id": "act_VJX3mbq9Jgj8EwADszW2AqdeJJXJiY",
        "type": "scroll",
        "query": "scroll?percentage=100&max_scroll_time=20000&scroll_speed=medium&continue_on_fail=false",
        "timestamp": "2025-06-09T15:56:03.9691994Z"
      },
      {
        "id": "act_VJX3mjBQYv8zTsXv1SkgUnBkzNFmJU",
        "type": "capture_screenshot",
        "query": "capture_screenshot?size=fullscreen&continue_on_fail=false",
        "timestamp": "2025-06-09T15:56:20.0727905Z",
        "output": "https://storage.gaffa.dev/static/docs/full-height-screenshot/screenshot-full.png"
      }
    ]
  },
  "error": null
}
```
{% endcode %}

The response contains the following information:

* **data.id**: Unique request identifier.
* **data.state**: "completed" means the workflow finished (even if some steps timed out).
* **data.credit\_usage**: Credits consumed for this run.
* **data.started\_at** / **data.completed\_at**: Workflow timing.
* **data.running\_time** and **data.page\_load\_time**: Performance metrics.
* **data.actions**: Each action’s details, including successes, timeouts, and final screenshot URL.

Within the list of actions, you'll be able to see the capture\_screenshot action, which contains an output parameter containing the full-size screenshot that was captured.
{% endstep %}
{% endstepper %}

If you don't want to use cURL, you can also run this query in the [Gaffa API Playground](https://gaffa.dev/dashboard/playground), which is an easy way to get started.

### Use Cases

Gaffa's screenshot action could be used for a huge number of use cases, but here are a few ideas:

* **Visual Regression**: Integrate into your CI pipeline to compare changes over time.
* **Archival**: Schedule daily captures for audit or compliance purposes.
* **Monitoring**: Automate periodic checks to detect visual bugs or layout shifts.

#### All this is powered by Gaffa’s hosted headless browsers with no local setup required. Experiment with more actions and easily build complex browser workflows. Refer to the full[ Browser Requests API documentation](https://gaffa-1.gitbook.io/gaffa/features/browser-requests) for additional capabilities.

<br>
