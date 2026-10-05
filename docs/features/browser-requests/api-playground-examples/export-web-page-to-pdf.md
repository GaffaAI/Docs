---
description: >-
  An example request that uses Gaffa to convert an HTML page to a PDF. There are
  lots of HMTL to PDF API's but Gaffa handles it easily, as well as doing much
  more.
---

# Export Web Page to PDF

_The following example is a request we've prebuilt to show you Gaffa's capabilities on our_ [_demo site._](https://demo.gaffa.dev) _**You can run this request right now in the**_ [_**Gaffa API Playground**_](https://gaffa.dev/dashboard/playground?templateId=html_to_pdf)_**.**_

Gaffa's print-to-PDF feature allows you to easily export web pages as PDF files. Unlike the standard "Print to PDF" in your local browser, Gaffa's feature waits for specific items to load, uses proxies, and scales with your product's growth. Enhance your customer experience and streamline your PDF export process

## API Request

The request below uses the [POST endpoint](../../../api-reference/post-v1-browser-requests.md) to open the demo site on the table page, wait for the table to load, and then print the webpage to a PDF in A4 size with a 20-point margin and in portrait orientation.

```json
{
  "url": "https://demo.gaffa.dev/simulate/table?loadTime=3&rowCount=20",
  "proxy_location": null,
  "async": false,
  "max_cache_age": 0,
  "settings": {
    "record_request": false,
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

## Actions

Read the full documentation for these actions here.

{% content-ref url="../actions/wait.md" %}
[wait.md](../actions/wait.md)
{% endcontent-ref %}

{% content-ref url="../actions/print.md" %}
[print.md](../actions/print.md)
{% endcontent-ref %}

## Response

Here's an example of the PDF returned by the request after the table has loaded.

{% file src="../../../.gitbook/assets/GaffaPrintPdfExample.pdf" %}
