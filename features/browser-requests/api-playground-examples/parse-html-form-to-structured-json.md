---
description: >-
  An example request that uses Gaffa to analyze a web form and extract all input
  fields, their labels, types, and properties into structured JSON.
---

# Parse HTML Form to Structured JSON

_The following example is a request we've pre-built to show you Gaffa's capabilities against our_ [_demo site_](https://demo.gaffa.dev)_._&#x20;

This example demonstrates how to extract structured information from HTML forms on web pages. Gaffa uses AI to identify form elements and their properties, making it perfect for form automation, testing, accessibility audits, or building form-filling assistants.

## API Request

The request below uses the [POST endpoint](https://gaffa.dev/docs/api-reference/post-v1-browser-requests) to open the demo form page, wait for the modal to appear, and then parse the visible form to extract all field information, including labels, input names, placeholders, and dropdown options.

```json
{
  "url": "https://demo.gaffa.dev/simulate/form?loadTime=3&showModal=true&modalDelay=5&formType=address",
  "proxy_location": null,
  "async": false,
  "max_cache_age": 0,
  "settings": {
    "record_request": false,
    "actions": [
      {
        "type": "parse_json",
        "data_schema": {
          "name": "AddressFormSchema",
          "description": "Extracts fields, labels, and placeholders from the demo address form",
          "fields": [
            {
              "type": "string",
              "name": "form_title",
              "description": "The heading or title of the form"
            },
            {
              "type": "object",
              "name": "full_name",
              "description": "Full name input field",
              "fields": [
                {
                  "type": "string",
                  "name": "label",
                  "description": "The visible label text"
                },
                {
                  "type": "string",
                  "name": "placeholder",
                  "description": "Placeholder text shown in the input"
                },
                {
                  "type": "string",
                  "name": "input_name",
                  "description": "The name attribute of the input element"
                }
              ]
            },
            {
              "type": "object",
              "name": "address_line_1",
              "description": "First address line input field",
              "fields": [
                {
                  "type": "string",
                  "name": "label",
                  "description": "The visible label text"
                },
                {
                  "type": "string",
                  "name": "placeholder",
                  "description": "Placeholder text shown in the input"
                },
                {
                  "type": "string",
                  "name": "input_name",
                  "description": "The name attribute of the input element"
                }
              ]
            },
            {
              "type": "object",
              "name": "address_line_2",
              "description": "Second address line input field",
              "fields": [
                {
                  "type": "string",
                  "name": "label",
                  "description": "The visible label text"
                },
                {
                  "type": "string",
                  "name": "placeholder",
                  "description": "Placeholder text shown in the input"
                },
                {
                  "type": "string",
                  "name": "input_name",
                  "description": "The name attribute of the input element"
                }
              ]
            },
            {
              "type": "object",
              "name": "city",
              "description": "City input field",
              "fields": [
                {
                  "type": "string",
                  "name": "label",
                  "description": "The visible label text"
                },
                {
                  "type": "string",
                  "name": "placeholder",
                  "description": "Placeholder text shown in the input"
                },
                {
                  "type": "string",
                  "name": "input_name",
                  "description": "The name attribute of the input element"
                }
              ]
            },
            {
              "type": "object",
              "name": "postcode",
              "description": "Postcode or ZIP code input field",
              "fields": [
                {
                  "type": "string",
                  "name": "label",
                  "description": "The visible label text"
                },
                {
                  "type": "string",
                  "name": "placeholder",
                  "description": "Placeholder text shown in the input"
                },
                {
                  "type": "string",
                  "name": "input_name",
                  "description": "The name attribute of the input element"
                }
              ]
            },
            {
              "type": "object",
              "name": "country",
              "description": "Country selection dropdown",
              "fields": [
                {
                  "type": "string",
                  "name": "label",
                  "description": "The visible label text"
                },
                {
                  "type": "string",
                  "name": "input_name",
                  "description": "The name attribute of the select element"
                },
                {
                  "type": "array",
                  "name": "options",
                  "description": "Available country options in the dropdown",
                  "fields": [
                    {
                      "type": "string",
                      "name": "value",
                      "description": "The option value or text"
                    }
                  ]
                }
              ]
            }
          ]
        },
        "instruction": "Extract all visible form fields from this address form, including their labels, input names, placeholders, and for dropdown fields, list all available options.",
        "model": "gpt-4o-mini",
        "output_type": "inline"
      }
    ]
  }
}
```

## Actions

{% content-ref url="../actions/parse-json.md" %}
[parse-json.md](../actions/parse-json.md)
{% endcontent-ref %}

## Response

The parsed form data is returned as a structured JSON object:

```json
{
    "data": {
        "id": "brq_VYg5H56A7m4vLJTdzj2jB3MgTAfT7K",
        "url": "https://demo.gaffa.dev/simulate/form?loadTime=3&showModal=true&modalDelay=5&formType=address",
        "state": "completed",
        "credit_usage": 0,
        "http_status_code": 200,
        "from_cache": false,
        "started_at": "2025-12-01T06:40:15.9241312Z",
        "completed_at": "2025-12-01T06:40:23.7495525Z",
        "running_time": "00:00:07.8254213",
        "page_load_time": "00:00:00.3124478",
        "actions": [
            {
                "id": "act_VYg5HDUFBrWq1GdmhQruRq4Gp7hjAk",
                "type": "parse_json",
                "timestamp": "2025-12-01T06:40:23.7495396Z",
                "output": {
                    "form_title": "Address Form",
                    "full_name": {
                        "label": "Full Name",
                        "placeholder": "Enter your full name",
                        "input_name": "full_name"
                    },
                    "address_line_1": {
                        "label": "Address Line 1",
                        "placeholder": "Enter your address",
                        "input_name": "address_line_1"
                    },
                    "address_line_2": {
                        "label": "Address Line 2",
                        "placeholder": "Optional",
                        "input_name": "address_line_2"
                    },
                    "city": {
                        "label": "City",
                        "placeholder": "Enter your city",
                        "input_name": "city"
                    },
                    "postcode": {
                        "label": "Postcode",
                        "placeholder": "Enter your postcode",
                        "input_name": "postcode"
                    },
                    "country": {
                        "label": "Country",
                        "input_name": "country",
                        "options": [
                            {
                                "value": "United States"
                            },
                            {
                                "value": "Canada"
                            },
                            {
                                "value": "United Kingdom"
                            },
                            {
                                "value": "Australia"
                            },
                            {
                                "value": "Germany"
                            }
                        ]
                    }
                },
                "reference": "https://storage.gaffa.dev/brq/dom/brq_VYg5H56A7m4vLJTdzj2jB3MgTAfT7K/act_VYg5HDUFBrWq1GdmhQruRq4Gp7hjAk_raw.txt"
            }
        ]
    }
}
```
