---
description: >-
  An introduction to the Gaffa Browser API. Learn how you can get started
  building fast, powerful web automations!
---

# Get Started

Welcome to the Gaffa documentation site! You'll find everything you need here to get started using the API, including [interactive API definitions](api-reference/post-v1-browser-requests.md), [a comprehensive list of actions](features/browser-requests/actions/) you can use to interact with our cloud browsers, and [breakdowns of our example requests](features/browser-requests/api-playground-examples/) you can run right away in our API Playground.

{% hint style="info" %}
Gaffa is currently in its very early stages, so we'd love to hear how we can improve our docs and API to make life easier for our users. If you have any questions or comments, please [email us](mailto:support@gaffa.dev) or use [the support tool on our site](https://go.crisp.chat/chat/embed/?website_id=87a5807c-14f5-4ed3-9fbe-3d161610357b).\
\
To stay up to date with the latest developments, features, and news on the mission to support the development of revolutionary AI Agents, sign up for sporadic [newsletter](https://gaffa.dev/#newsletter) updates.
{% endhint %}

{% stepper %}
{% step %}
## Create an account

You can sign up to create a Gaffa account [here](https://gaffa.dev/auth/sign-in). Every new account comes with 500 free credits, which you can use to start building and running automations on the open web right away. You can also use the [API Playground](https://gaffa.dev/dashboard/playground) to explore prebuilt automations built against the Gaffa [demo site](https://demo.gaffa.dev/) and experiment with Gaffa's features.
{% endstep %}

{% step %}
## Making your first browser request

The easiest way to make your first Gaffa [browser request](features/browser-requests/) is to use our [API Playground](https://gaffa.dev/dashboard/playground), where you can explore pre-made interactive browser request examples built against our test site. You can run and edit these examples to experiment, or use your 500 free credits to build and test automations on other sites across the open web.

You can also send your first request directly from the command line. Replace `YOUR_API_KEY` with a key from [**Dashboard → API Keys**](https://gaffa.dev/dashboard/api-keys).

```sh
curl --request POST 'https://api.gaffa.dev/v1/browser/requests' \
  --header 'Content-Type: application/json' \
  --header 'X-API-Key: YOUR_API_KEY' \
  --data '{
    "url": "https://demo.gaffa.dev/simulate/table?loadTime=3&rowCount=20",
    "async": false,
    "settings": {
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

See [Browser Requests](features/browser-requests/) for full cURL, Python, and JavaScript examples.

### Gaffa API Playground examples

Here are all the sample requests we've created for use in the API Playground.

<table data-view="cards"><thead><tr><th></th><th></th><th data-hidden data-card-target data-type="content-ref"></th></tr></thead><tbody><tr><td><strong>Print to PDF</strong></td><td>Export a web page to PDF and wait for elements to load with the Gaffa API.</td><td><a href="features/browser-requests/api-playground-examples/export-web-page-to-pdf.md">export-web-page-to-pdf.md</a></td></tr><tr><td><strong>Convert to Markdown</strong></td><td>Export a web page to markdown format - useful feeding into LLM apps.</td><td><a href="features/browser-requests/api-playground-examples/convert-web-page-to-markdown.md">convert-web-page-to-markdown.md</a></td></tr><tr><td><strong>Infinitely Scroll</strong></td><td>Scroll the bottom of a page that infinitely loads items and record the interaction.</td><td><a href="features/browser-requests/api-playground-examples/infinitely-scroll-an-e-commerce-site.md">infinitely-scroll-an-e-commerce-site.md</a></td></tr><tr><td><strong>Capture Screenshot</strong></td><td>Interact with a page and capture the a screenshot of the whole page.</td><td><a href="features/browser-requests/api-playground-examples/capture-a-full-height-screenshot.md">capture-a-full-height-screenshot.md</a></td></tr><tr><td><strong>Form Completion</strong></td><td>Fill out a form in a human-like way and record the interaction</td><td><a href="features/browser-requests/api-playground-examples/automated-form-filling.md">automated-form-filling.md</a></td></tr></tbody></table>
{% endstep %}

{% step %}
## Building your own browser requests

When you're ready to start building your own browser requests, you'll want to explore the available [actions](features/browser-requests/actions/), [proxy servers](features/browser-requests/parameters.md#proxy-servers), [caching](features/browser-requests/parameters.md#caching) options, and [other endpoints that are part of the Gaffa API](https://app.gitbook.com/s/yUba6osOT5MkKiV0wmgr/api-reference).
{% endstep %}
{% endstepper %}

## <sup>**Want to build faster with AI assistance?**</sup>

You can use Gaffa's [`llms.txt`](https://gaffa.dev/docs/llms-full.txt) file to give AI assistants like ChatGPT or Claude instant, accurate context about the Gaffa API, so they can generate working code for you straight away, without you having to explain the API yourself. [Learn how to use the Gaffa LLMs.txt file →](https://gaffa.dev/blog/let-your-ai-assistant-write-your-gaffa-code)
