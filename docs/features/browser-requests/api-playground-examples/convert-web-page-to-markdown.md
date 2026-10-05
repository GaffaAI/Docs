---
description: >-
  An example request that uses Gaffa to convert a web page page to markdown.
  This could be used to export web page reports or to print the content of a
  page in a readable format.
---

# Convert Web Page to Markdown

_The following example is a request we've prebuilt to demonstrate Gaffa's capabilities on our_ [_demo site._](https://demo.gaffa.dev) _**You can run this request right now in the**_ [_**Gaffa API Playground**_](https://gaffa.dev/dashboard/playground?templateId=article_to_markdown)_**.**_

Gaffa converts web pages to clean markdown, stripping away styling, scripts, and images. This optimises content for LLM applications by reducing credit usage while preserving essential information.

## API Request

The request below uses the POST endpoint to open the demo site on the article simulator, wait for the article to load, and then generate a markdown from the page's content, which you can download for use in your program.

```json
{
  "url": "https://demo.gaffa.dev/simulate/article?loadTime=3&paragraphs=10&images=3",
  "proxy_location": null,
  "async": false,
  "max_cache_age": 0,
  "settings": {
    "record_request": false,
    "actions": [
      {
        "type": "wait",
        "selector": "article"
      },
      {
        "type": "generate_markdown"
      }
    ]
  }
}
```

## Actions

{% content-ref url="../actions/wait.md" %}
[wait.md](../actions/wait.md)
{% endcontent-ref %}

{% content-ref url="../actions/generate-markdown.md" %}
[generate-markdown.md](../actions/generate-markdown.md)
{% endcontent-ref %}

## Response

Here's an example of the PDF returned by the request after the article has loaded.

{% file src="../../../.gitbook/assets/GaffaMarkdownExample.md" %}
