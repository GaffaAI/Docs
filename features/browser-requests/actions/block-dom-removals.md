# Block DOM Removals

**Type:** `block_dom_removals`

This action will prevent the page from removing items from the page. This is useful if you are trying to scrape data from a JavaScript-based web application that removes items from the page when they are out of view, which can make grabbing data difficult.

Using this action will block DOM removals for the rest of the browser request.

### Parameters

See [universal parameters](./#universal-parameters).

### Usage

Block DOM removals for the current browser request

```
"actions": [
    {
      "type": "block_dom_removals"
    }
]
```
