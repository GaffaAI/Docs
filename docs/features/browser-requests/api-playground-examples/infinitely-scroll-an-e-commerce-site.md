---
description: >-
  An example request that uses Gaffa to infinitely scroll down a simulated
  ecommerce site whilst recording the interaction.
---

# Infinitely Scroll an E-commerce Site

_The following example is a request we've prebuilt to show you Gaffa's capabilities on our_ [_demo site._](https://demo.gaffa.dev) _**You can run this request right now in the**_ [_**Gaffa API Playground**_](https://gaffa.dev/dashboard/playground?templateId=infinite_scroll)_**.**_

Gaffa automates infinite scrolling on dynamic pages, such as e-commerce storefronts. Set a duration, and Gaffa will capture all content as it scrolls. Each session can be recorded as a video for playback, letting you debug or review the interaction.

## API Request

The request below uses the [POST endpoint](../../../api-reference/post-v1-browser-requests.md) to open the demo site in the e-commerce site simulator, featuring an infinitely scrolling storefront. It will wait for and dismiss a dialog box, wait for a product to load, and then scroll down the page for a maximum of 20 seconds - if new items load, it will keep scrolling.

```json
{
  "url": "https://demo.gaffa.dev/simulate/ecommerce?loadTime=3&showModal=true&modalDelay=0&itemCount=infinite",
  "proxy_location": null,
  "async": false,
  "max_cache_age": 0,
  "settings": {
    "record_request": true,
    "actions": [
      {
        "type": "wait",
        "selector": "div[role=\"dialog\"]",
        "timeout": 10000
      },
      {
        "type": "click",
        "selector": "[data-testid=\"accept-all-button\"]"
      },
      {
        "type": "wait",
        "selector": "[data-testid^=\"product-1\"]",
        "timeout": 5000
      },
      {
        "type": "scroll",
        "percentage": 100,
        "max_scroll_time": 20000
      }
    ]
  }
}
```

## Actions

{% content-ref url="../actions/wait.md" %}
[wait.md](../actions/wait.md)
{% endcontent-ref %}

{% content-ref url="../actions/click.md" %}
[click.md](../actions/click.md)
{% endcontent-ref %}

{% content-ref url="../actions/scroll.md" %}
[scroll.md](../actions/scroll.md)
{% endcontent-ref %}

## Response

Here's a video showing Gaffa scrolling the page for 20 seconds as more items load.

{% embed url="https://youtu.be/s4WsBYxGWOo" %}
Gaffa scrolling to the bottom of a simulated ecommerce page!
{% endembed %}

## Read More

Read more about screen recording [here](../settings.md#screen-recording).

{% content-ref url="../../../get-started.md" %}
[get-started.md](../../../get-started.md)
{% endcontent-ref %}
