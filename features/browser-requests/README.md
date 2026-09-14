---
description: Making web automation requests has never been so simple.
---

# Browser Requests

Browser Requests allow you to send the Gaffa API a URL and a list of actions you want to be carried out, including any outputs you want from the page. We'll carry out the request in our cloud browsers and return the response, so you don't have to worry about proxies, IP rotation, web automation frameworks, or scaling.

There's absolutely zero configuration needed, and you can interact with Gaffa from any program that can send web requests. We think it's by far the simplest way to automate basic web tasks, and the good news is that we're just getting started and have much more planned.

### How It Works

A browser request consists of three main components:

1. **Parameters** — Control the basics like URL, proxy location, and caching
2. **Settings** — Configure recording, media limits, and timing
3. **Actions** — Define the tasks you want performed on the page

### Example Request

Running a new browser request is as simple as sending the following [POST body to our endpoint](../../api-reference/post-v1-browser-requests.md). Below, you can see the URL ([our demo site](https://demo.gaffa.dev/)) and a list of actions that instruct Gaffa to wait for the table to load, then print the page to PDF.

You can read more about this particular example and how you can run it right now in our API Playground [here](api-playground-examples/export-web-page-to-pdf.md).

```json
{
  "url": "https://demo.gaffa.dev/simulate/table?loadTime=3&rowCount=20",
  "proxy_location": null,
  "async": false,
  "max_cache_age": 0,
  "settings": {
    "record_request": false,
    "max_media_bandwidth": null,
    "actions": [
      {
        "type": "wait",
        "selector": "table"
      },
      {
        "type": "print",
        "size": "A4",
        "margin": 20,
        "orientation": "portrait"
      }
    ]
  }
}
```

### Run this request

Set `GAFFA_API_KEY` to an API key from [**Dashboard → API Keys**](https://gaffa.dev/dashboard/api-keys). Then choose your preferred language.

{% tabs %}
{% tab title="cURL" %}
```sh
curl --request POST 'https://api.gaffa.dev/v1/browser/requests' \
  --header 'Content-Type: application/json' \
  --header "X-API-Key: $GAFFA_API_KEY" \
  --data '{
    "url": "https://demo.gaffa.dev/simulate/table?loadTime=3&rowCount=20",
    "proxy_location": null,
    "async": false,
    "max_cache_age": 0,
    "settings": {
      "record_request": false,
      "max_media_bandwidth": null,
      "actions": [
        {
          "type": "wait",
          "selector": "table"
        },
        {
          "type": "print",
          "size": "A4",
          "margin": 20,
          "orientation": "portrait"
        }
      ]
    }
  }'
```
{% endtab %}

{% tab title="Python" %}
```python
import json
import os
from urllib.request import Request, urlopen

payload = {
    "url": "https://demo.gaffa.dev/simulate/table?loadTime=3&rowCount=20",
    "proxy_location": None,
    "async": False,
    "max_cache_age": 0,
    "settings": {
        "record_request": False,
        "max_media_bandwidth": None,
        "actions": [
            {"type": "wait", "selector": "table"},
            {
                "type": "print",
                "size": "A4",
                "margin": 20,
                "orientation": "portrait",
            },
        ],
    },
}

request = Request(
    "https://api.gaffa.dev/v1/browser/requests",
    data=json.dumps(payload).encode(),
    headers={
        "Content-Type": "application/json",
        "X-API-Key": os.environ["GAFFA_API_KEY"],
    },
    method="POST",
)

with urlopen(request) as response:
    print(response.read().decode())
```
{% endtab %}

{% tab title="JavaScript" %}
```javascript
const response = await fetch("https://api.gaffa.dev/v1/browser/requests", {
  method: "POST",
  headers: {
    "Content-Type": "application/json",
    "X-API-Key": process.env.GAFFA_API_KEY,
  },
  body: JSON.stringify({
    url: "https://demo.gaffa.dev/simulate/table?loadTime=3&rowCount=20",
    proxy_location: null,
    async: false,
    max_cache_age: 0,
    settings: {
      record_request: false,
      max_media_bandwidth: null,
      actions: [
        { type: "wait", selector: "table" },
        {
          type: "print",
          size: "A4",
          margin: 20,
          orientation: "portrait",
        },
      ],
    },
  }),
});

console.log(await response.json());
```
{% endtab %}
{% endtabs %}

### Stealth

We believe your AI Agents should be able to use the internet exactly how humans would. Gaffa can help you access sites with some of the most challenging anti-bot restrictions by combining proxies, human-like behaviour, captcha solving, and a custom browser implementation. We handle and maintain all of that so you can focus on building your solution!

### Learn More

[**Parameters**](parameters.md) — Learn about URL, proxy settings, async mode, and caching

[**Settings**](settings.md) — Explore recording, media bandwidth controls, and time limits

[**Actions**](actions/) — Discover all available actions like screenshots, markdown generation, and more

[**Examples**](api-playground-examples/) — View pre-built requests and start using them in the API Playground

[**API Reference**](../../api-reference/)— Complete endpoint documentation and technical details

### Examples

We've created a number of sample browser requests you can read about [here](api-playground-examples/), or you can jump straight into the [API Playground](https://gaffa.dev/dashboard/playground) to run them right now.

### API Endpoints

Check out our API reference for more details on the available endpoints, particularly [those you can use to query for past requests by ID or status](../../api-reference/get-v1-browser-requests.md).
