---
description: >-
  An example request that uses Gaffa to automate the completion of a form and
  waits for a success modal to appear.
---

# Automated Form Filling

_The following example is a request we've prebuilt to show you Gaffa's capabilities on our_ [_demo site._](https://demo.gaffa.dev) _**You can run this request right now in the**_ [_**Gaffa API Playground**_](https://gaffa.dev/dashboard/playground?templateId=form_fill)_**.**_

## API Request

```json
{
  "url": "https://demo.gaffa.dev/simulate/form?loadTime=3&showModal=false&modalDelay=0&formType=address&firstName=John&lastName=Doe&address1=123%20Main%20Street&city=London&country=UK",
  "proxy_location": null,
  "async": false,
  "max_cache_age": 0,
  "settings": {
    "record_request": true,
    "actions": [
      {
        "type": "type",
        "selector": "#email",
        "text": "johndoe@example.com"
      },
      {
        "type": "type",
        "selector": "#state",
        "text": "CA"
      },
      {
        "type": "type",
        "selector": "#zipCode",
        "text": "12345"
      },
      {
        "type": "click",
        "selector": "button[type='submit']"
      },
      {
        "type": "wait",
        "selector": "[role=\"dialog\"] h2:has-text(\"Success!\")",
        "timeout": 10000
      }
    ]
  }
}
```

## Actions

{% content-ref url="../actions/type.md" %}
[type.md](../actions/type.md)
{% endcontent-ref %}

{% content-ref url="../actions/click.md" %}
[click.md](../actions/click.md)
{% endcontent-ref %}

{% content-ref url="../actions/wait.md" %}
[wait.md](../actions/wait.md)
{% endcontent-ref %}

## Response

Here's a video showing Gaffa filling out the page and waiting for the success modal.

{% embed url="https://youtu.be/TGPnuc-71Bs" %}
Gaffa can help automatically fill out your forms!
{% endembed %}

## Read More

Read more about screen recording here (TODO).
