---
description: >-
  An example request that uses Gaffa to extract structured data (JSON) from a
  table on a webpage
---

# Parse an HTML Table to JSON

_The following example is a prebuilt request that demonstrates Gaffa's capabilities on our_ [_demo site_](https://demo.gaffa.dev/)_. **You can run this request right here in the**_ [_**Gaffa API Playground**_](https://gaffa.dev/dashboard/playground?templateId=parse_table)_**.**_

This example demonstrates how to extract tabular data from any webpage without writing a scraper. Gaffa renders the page using a real browser, waits for the table to load, and returns the rows as a clean JSON array, making it perfect for building data pipelines, monitoring dashboards, or feeding structured data into LLM workflows.

## API Request

The request below uses the [POST endpoint](../../../api-reference/post-v1-browser-requests.md) to load a demo table page, waits for the table element to appear, and parses each row into a structured JSON array, using the table's header row as property names.

```json
{
  "url": "https://demo.gaffa.dev/simulate/table?loadTime=1&rowCount=3",
  "proxy_location": null,
  "async": false,
  "max_cache_age": 0,
  "settings": {
    "record_request": false,
    "actions": [
      {
        "type": "wait",
        "selector": "table",
        "timeout": 5000
      },
      {
        "type": "parse_table",
        "selector": "table"
      }
    ]
  }
}
```

## Actions

{% content-ref url="../actions/wait.md" %}
[wait.md](../actions/wait.md)
{% endcontent-ref %}

{% content-ref url="../actions/parse-table.md" %}
[parse-table.md](../actions/parse-table.md)
{% endcontent-ref %}

## Response

The `parse_table` action returns an `output` URL pointing to the extracted JSON:

```json
{
  "data": {
    "id": "brq_W3jgR52eKUxk9qLKCAL5zUAmFjs4zY",
    "url": "https://demo.gaffa.dev/simulate/table?loadTime=1&rowCount=3",
    "state": "completed",
    "credit_usage": 0,
    "http_status_code": 200,
    "from_cache": false,
    "data_deleted": false,
    "started_at": "2026-11-24T18:27:51.1631567Z",
    "completed_at": "2026-11-24T18:27:55.4462851Z",
    "running_time": "00:00:04.2831284",
    "page_load_time": "00:00:00.4731942",
    "actions": [
      {
        "id": "act_W3jgRB6tGe5UYNXdQzS6ka3eZ4yTVg",
        "type": "wait",
        "timestamp": "2026-11-24T18:27:55.1989962Z"
      },
      {
        "id": "act_W3jgR5ePdbUp5oU9QCSzjuS9bVaCVB",
        "type": "parse_table",
        "timestamp": "2026-11-24T18:27:55.4462737Z",
        "output": "https://storage.gaffa.dev/brq/json/brq_W3jgR52eKUxk9qLKCAL5zUAmFjs4zY/act_W3jgR5ePdbUp5oU9QCSzjuS9bVaCVB.json"
      }
    ]
  }
}
```

Fetching that URL gives you the table rows as a ready-to-use array:

```json
[
  {
    "id": "1",
    "name": "Item 1",
    "quantity": "30",
    "price": "$56.05"
  },
  {
    "id": "2",
    "name": "Item 2",
    "quantity": "68",
    "price": "$76.89"
  },
  {
    "id": "3",
    "name": "Item 3",
    "quantity": "67",
    "price": "$20.44"
  }
]
```
