# Parameters

Parameters are the top-level settings that control the fundamental behaviour of your automation. These parameters define where your request goes, how it's routed, whether it runs synchronously or asynchronously, and how caching is handled.

Below you'll find detailed documentation for each available parameter.

## Proxy servers

**Parameter:** `proxy_location` (string or null)

Gaffa makes it super simple to proxy your traffic through a global network of residential proxies. Setting `proxy_location` in your request will allow you to utilize one of our partner third-party proxy services to gain local access to a site.

Not setting a `proxy_location` will mean the request does not use a proxy server and will use a generic datacenter IP.

### Available Locations

| Proxy Server Location | Country Code |
| --------------------- | ------------ |
| United States         | `us`         |
| Ireland               | `ie`         |
| Singapore             | `sg`         |
| France                | `fr`         |

{% hint style="info" %}
At the moment, all our servers are in one location, but we aim to deploy local machines at our proxy locations to improve realistic end-user load times. If this interests you, please contact support.
{% endhint %}

### IP Types

Currently, all our IP addresses are residential IP addresses, which are procured through reputable third parties.

### IP Rotation

IP rotation is an essential part of any web data scraping or automation task. In Gaffa, each browser request is treated as unique. We regularly rotate the IP addresses used, so you should assume each request is made from a different IP address than the last.

{% hint style="info" %}
We are working to support a wider range of IP address scenarios, including static IPs in the future, and to enable more trusted proxies for requests that require enhanced security (logins, etc.).
{% endhint %}

### Restrictions

Whilst we'll do our best to provide access to as wide a range of sites as possible, we may have to restrict access to certain sites to prevent abuse of our service or of other services. Our proxy partners may also enforce restrictions on certain sites and categories of sites that we don't have any control over.

***

## Synchronous & Asynchronous Requests

**Parameter:** `async` (boolean)

By default, browser requests run synchronously: the request waits and hands back the full result directly, with nothing further to check. Setting `async: true` instead returns an `id` immediately, and your automation checks back later to see when the result is ready.

Sync suits quick, predictable jobs where you'd rather not check back at all. Async is the better fit for anything that might run a while. A synchronous request holds a connection open for as long as it runs, and most tools time out well before a long job finishes, so a slow sync request risks getting cut off partway through even if Gaffa itself would have completed it. If a job might run long, use async.

When you set `async: true`, running the request gives you back just enough to track it: an `id` and a pending status, not a result yet.

**Response:**

```json
{
  "data": {
    "id": "brq_...",
    "url": "https://demo.gaffa.dev/simulate/table?loadTime=1&rowCount=20",
    "state": "pending",
    "credit_usage": 0,
    "from_cache": false
  }
}
```

#### Getting your result

Poll `GET /v1/browser/requests/{id}` — using the `id` from your original request — with your API key passed as the `X-API-Key` header:

**Request:**

```bash
curl --location "https://api.gaffa.dev/v1/browser/requests/{id}" \
  --header "X-API-Key: YOUR_API_KEY"
```

Keep checking back until it's done. There are two states:

1. `state: "pending"` — still running. Check back again shortly.
2. `state: "completed"` — finished. Your result is included, along with everything your actions produced.

`state` tracks the request's lifecycle within Gaffa, not the outcome of your automation. `"pending"` means it's still running. `"completed"` means Gaffa has finished processing it, your actions ran (or were cancelled), and the result is ready, either way.\
Whether it actually succeeded is a separate question, answered by the `error` field, not by `state`. A request that failed or was cancelled partway through is still `"completed"`.

#### A couple of things worth knowing

* Your plan caps request runtime, and the cap differs for sync and async requests. Read more about [Time Limit](settings.md#time-limit).
* A request that gets cut off for running too long isn't billed for the attempt.
* Results are kept for as long as your plan's data retention period, after which the `id` can no longer be used to fetch them. See [data retention](../data-retention.md#retention-by-plan) for details.

<details>

<summary><strong>How sync and async behave in practice, with examples</strong></summary>

Every request, whether sync or async, is bound by `time_limit`, which is capped by your plan. See [Time Limit](settings.md#time-limit) for the parameter itself, the base plan limits, and what happens when a request runs out of time — the same cancellation behaviour applies whether you're polling for the result (async) or waiting on it directly (sync).

A synchronous request holds a single HTTP connection open for its entire duration, and most client libraries, load balancers, and reverse proxies default to read timeouts under 5 minutes — so a long sync request risks being dropped before you ever see the result, even if the run itself would have succeeded. Use async for anything that might need the higher end of your plan's cap.

#### Scenario 1: sync completes inside its plan's cap

A normal, successful synchronous (`async: false`) request. The full result comes back directly on the initial call.

**Request:**

```json
{
  "url": "https://demo.gaffa.dev/simulate/table?loadTime=1&rowCount=20",
  "async": false,
  "max_cache_age": 0,
  "settings": {
    "time_limit": 30000,
    "actions": [
      { "type": "wait", "selector": "table", "timeout": 5000 },
      { "type": "print", "size": "A4", "margin": 20, "orientation": "portrait" }
    ]
  }
}
```

**Response:**

```json
{
  "data": {
    "id": "brq_VwPWCbsJV6RFxJM5YKtE63LACZBQMj",
    "url": "https://demo.gaffa.dev/simulate/table?loadTime=1&rowCount=20",
    "state": "completed",
    "credit_usage": 0,
    "http_status_code": 200,
    "from_cache": false,
    "started_at": "2026-09-07T11:11:33.5709542Z",
    "completed_at": "2026-09-07T11:11:42.0714203Z",
    "running_time": "00:00:08.5004661",
    "page_load_time": "00:00:00.6294882",
    "actions": [
      { "id": "act_VwPWCiDUigtxtTbnzJXECXUUNk1B5Q", "type": "wait", "timestamp": "2026-09-07T11:11:41.4129634Z" },
      {
        "id": "act_VwPWChYYzXX7hatJacEHRcE7rJXyzi",
        "type": "print",
        "timestamp": "2026-09-07T11:11:42.0696721Z",
        "output": "https://storage.gaffa.dev/brq/pdf/brq_VwPWCbsJV6RFxJM5YKtE63LACZBQMj/act_VwPWChYYzXX7hatJacEHRcE7rJXyzi.pdf"
      }
    ]
  }
}
```

#### Scenario 2: async doesn't hand you the result the same way

For an asynchronous request (`async: true`), the result doesn't come back in one go like this. The initial call returns only an `id` and a pending status—you then have to poll a separate endpoint to find out when it's done and get the real result.

**Polling in full**

**Request:**

```shellscript
GET /v1/browser/requests/{id}
X-API-Key: YOUR_API_KEY
```

Poll at an interval until the request reaches a terminal state. While still running, `state` shows `"pending"`. Once finished, `state` becomes `"completed"`.

Response:

```json
{
  "data": {
    "id": "brq_...",
    "state": "pending",
    "credit_usage": 0
  }
}
```

Once terminal, `state` is `"completed"` either way — success is signalled by the absence of `error`, failure by its presence:

Response:

```json
{
  "data": {
    "id": "brq_...",
    "state": "completed",
    "credit_usage": 0,
    "error": "request_timeout",
    "error_reason": "The request hit the maximum allowed time and was cancelled.",
    "actions": [
      { "id": "act_...", "type": "wait", "error": "action_cancelled" }
    ]
  }
}
```



</details>

***

## Caching

**Parameter:** `max_cache_age` integer (seconds)

When we were building Gaffa, we noticed that many existing scraping tools don't let users easily share their scraped web data, even though many users request the same pages on the same sites. Not only is this a waste of a user's allowance, but it also puts a burden on the site owners who are serving the same data to different users for the same purpose. Because of this, we have created a service-wide cache in Gaffa.

### How it works

When making a browser request, you can provide a `max_cache_age` parameter that is **a number in seconds equal to or greater than 0**. This value denotes the maximum age of data you would accept from the API.\
\
If another user of our service has requested the same URL with exactly the same parameters and actions as you in this timeframe, the response will be returned to you immediately and will not be processed by one of our browsers. If there are multiple identical requests in the given timeframe, then the most recent will be returned.\
\
This will save you time waiting for a response and credits, because requests returned from the cache don't use any bandwidth.

***

## **Settings**

The `settings` object allows you to configure how your browser requests behave. It currently supports six parameters that control request recording, media downloads, execution time limits, ad blocking, redirect logging, and browser actions.

You can read more about all available settings parameters [here](settings.md).
