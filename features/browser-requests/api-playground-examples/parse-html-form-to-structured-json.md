---
description: >-
  An example request that uses Gaffa to analyze a web form and extract all input
  fields, their labels, types, and properties into structured JSON.
---

# Parse HTML Form to Structured JSON

_The following example is a request we've pre-built to show you Gaffa's capabilities against our_ [_demo site_](https://demo.gaffa.dev)_. **You can run this request right now in the**_ [_**Gaffa API Playground**_](https://gaffa.dev/dashboard/playground?templateId=parse_html_form_to_structured_json)_**.**_

This example demonstrates how to extract structured information from HTML forms on web pages. Gaffa uses AI to identify form elements and their properties, making it perfect for form automation, testing, accessibility audits, or building form-filling assistants.

## API Request

The request below uses the [POST endpoint](https://gaffa.dev/docs/api-reference/post-v1-browser-requests) to open the demo form page, wait for the modal to appear, and then parse the visible form to extract all field information, including labels, input names, placeholders, and dropdown options.

```json
{
  "url": "https://demo.gaffa.dev/simulate/form?loadTime=3&showModal=true&modalDelay=5&formType=address",
  "async": false,
  "max_cache_age": 0,
  "settings": {
    "record_request": false,
    "actions": [
      {
        "type": "wait",
        "selector": "form",
        "timeout": 10000
      },
      {
        "type": "parse_json",
        "data_schema": {
          "name": "FormFields",
          "description": "Extract all form input fields",
          "fields": [
            {
              "type": "string",
              "name": "form_title",
              "description": "Form title"
            },
            {
              "type": "array",
              "name": "fields",
              "description": "List of all input fields",
              "fields": [
                {
                  "type": "string",
                  "name": "label",
                  "description": "Field label"
                },
                {
                  "type": "string",
                  "name": "field_name",
                  "description": "Field name attribute"
                },
                {
                  "type": "string",
                  "name": "field_type",
                  "description": "Input type"
                },
                {
                  "type": "boolean",
                  "name": "required",
                  "description": "Is required?"
                },
                {
                  "type": "string",
                  "name": "placeholder",
                  "description": "Placeholder text"
                }
              ]
            }
          ]
        },
        "instruction": "Extract all form fields with their properties",
        "model": "gpt-4o-mini",
        "output_type": "inline"
      }
    ]
  }
}
```

## Actions

{% content-ref url="../actions/wait.md" %}
[wait.md](../actions/wait.md)
{% endcontent-ref %}

{% content-ref url="../actions/parse-json.md" %}
[parse-json.md](../actions/parse-json.md)
{% endcontent-ref %}

## Response

The parsed form data is returned as a structured JSON object:

```json
{
  "data": {
    "id": "brq_VrwuWctcUAj75jbs5XUFkMbGGeBeLP",
    "url": "https://demo.gaffa.dev/simulate/form?loadTime=3&showModal=true&modalDelay=5&formType=address",
    "state": "completed",
    "credit_usage": 0,
    "http_status_code": 200,
    "from_cache": false,
    "started_at": "2026-07-14T16:20:29.8409557Z",
    "completed_at": "2026-07-14T16:20:43.1357433Z",
    "running_time": "00:00:13.2947876",
    "page_load_time": "00:00:00.5453910",
    "actions": [
      {
        "id": "act_VrwuWbYiipbS2e91UHf3vokLudWjez",
        "type": "wait",
        "timestamp": "2026-07-14T16:20:37.1333682Z"
      },
      {
        "id": "act_VrwuWkyuTBi8w3pBaQD98naDi9MvaK",
        "type": "parse_json",
        "timestamp": "2026-07-14T16:20:43.1357329Z",
        "output": {
          "form_title": "Form Submission Test",
          "fields": [
            {
              "label": "First Name *",
              "field_name": "first_name",
              "field_type": "text",
              "required": true,
              "placeholder": ""
            },
            {
              "label": "Last Name *",
              "field_name": "last_name",
              "field_type": "text",
              "required": true,
              "placeholder": ""
            },
            {
              "label": "Email *",
              "field_name": "email",
              "field_type": "email",
              "required": true,
              "placeholder": ""
            },
            {
              "label": "Address Line 1 *",
              "field_name": "address_line_1",
              "field_type": "text",
              "required": true,
              "placeholder": ""
            },
            {
              "label": "Address Line 2",
              "field_name": "address_line_2",
              "field_type": "text",
              "required": false,
              "placeholder": ""
            },
            {
              "label": "City *",
              "field_name": "city",
              "field_type": "text",
              "required": true,
              "placeholder": ""
            },
            {
              "label": "State/Province *",
              "field_name": "state_province",
              "field_type": "text",
              "required": true,
              "placeholder": ""
            },
            {
              "label": "ZIP/Postal Code *",
              "field_name": "zip_postal_code",
              "field_type": "text",
              "required": true,
              "placeholder": ""
            },
            {
              "label": "Country *",
              "field_name": "country",
              "field_type": "text",
              "required": true,
              "placeholder": ""
            }
          ]
        },
        "reference": "https://storage.gaffa.dev/brq/dom/brq_VrwuWctcUAj75jbs5XUFkMbGGeBeLP/act_VrwuWkyuTBi8w3pBaQD98naDi9MvaK_raw.txt"
      }
    ]
  }
}
```
