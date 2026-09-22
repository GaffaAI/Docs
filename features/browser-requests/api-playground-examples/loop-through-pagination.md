---
description: >-
  An example request that uses Gaffa to loop through different type of pages and
  perform actions.
---

# Loop Through Pagination

_The following examples are prebuilt requests we've put together to show you Gaffa's capabilities against our demo site. **You can run any of them right now in the**_ [_**Gaffa API Playground**_](https://gaffa.dev/dashboard/playground?templateId=loop_pagination_numbered)_**.**_

These examples demonstrate the [`loop`](../actions/loop.md) action, which repeats a sequence of nested actions on every page of a paginated site, capturing the DOM (and optionally a screenshot) of each page until the pagination control disappears or a fixed number of iterations is reached, or a timeout is hit. This is useful for scraping listing pages, catalogues, or search results that span multiple pages, without having to send a separate request per page.

Pagination UIs vary from site to site, so below are three common patterns and how to loop through each of them:

1. [Numbered Pagination](loop-through-pagination.md#numbered-pagination)
2. [Next button Pagination](loop-through-pagination.md#next-button-pagination)
3. [Show more pagination](loop-through-pagination.md#show-more-pagination)

## Numbered Pagination

This example opens an e-commerce demo site that pages through its product listing using numbered page controls (1, 2, 3…), and loops through all 5 pages, capturing the DOM and a full-page screenshot of each one.

### API Request

The request below uses the [POST endpoint](../../../api-reference/post-v1-browser-requests.md) to open the [demo site](https://demo.gaffa.dev/simulate/ecommerce?loadTime=0\&showModal=false\&modalDelay=0\&loadingMode=paged\&pagingStyle=numbered\&pageCount=5\&pageSize=10\&itemCount=50\&itemLoadTime=0\&isVirtualScroll=false\&page=1), wait for the first product to render, then loop up to 5 times: on each iteration, it waits briefly for the page to settle, captures the DOM, takes a full-screen screenshot, and clicks the next numbered page button. _**You can run this request in the**_ [_**Gaffa API Playground**_](https://gaffa.dev/dashboard/playground?templateId=loop_pagination_numbered)_**.**_

```json
{
  "url": "https://demo.gaffa.dev/simulate/ecommerce?loadTime=0&showModal=false&modalDelay=0&loadingMode=paged&pagingStyle=numbered&pageCount=5&pageSize=10&itemCount=50&itemLoadTime=0&isVirtualScroll=false&page=1",
  "proxy_location": null,
  "async": false,
  "max_cache_age": 0,
  "settings": {
    "record_request": true,
    "max_media_bandwidth": null,
    "time_limit": 60000,
    "actions": [
      {
        "type": "wait",
        "selector": "[data-testid=\"product-1\"]",
        "timeout": 10000
      },
      {
        "type": "loop",
        "custom_id": "pagination-loop",
        "max_iterations": 5,
        "timeout": 45000,
        "stop_on_fail": true,
        "continue_on_fail": true,
        "actions": [
          {
            "type": "wait",
            "time": 500,
            "custom_id": "settle"
          },
          {
            "type": "capture_dom",
            "custom_id": "page-dom"
          },
          {
            "type": "capture_screenshot",
            "size": "fullscreen"
          },
          {
            "type": "click",
            "selector": "nav[aria-label=\"Pagination\"] button[aria-current=\"page\"] + button",
            "timeout": 5000,
            "custom_id": "next-page"
          }
        ]
      }
    ]
  }
}
```

### Response

The [`loop`](../actions/loop.md) action's nested actions run once per page and are returned as a single flat array, in execution order, with an `iterations` count on the loop action itself showing how many pages were captured before the click failed (i.e. the last "next page" button was gone) or the iteration cap was reached:

```json
{
  "data": {
    "id": "brq_...",
    "url": "https://demo.gaffa.dev/simulate/ecommerce?...&page=1",
    "state": "completed",
    "actions": [
      { "id": "act_...", "type": "wait", "timestamp": "..." },
      {
        "id": "act_...",
        "type": "loop",
        "custom_id": "pagination-loop",
        "iterations": 5,
        "timestamp": "..."
      },
      { "id": "act_...", "type": "wait", "custom_id": "settle", "timestamp": "..." },
      { "id": "act_...", "type": "capture_dom", "custom_id": "page-dom", "output": "...", "timestamp": "..." },
      { "id": "act_...", "type": "capture_screenshot", "output": "https://storage.gaffa.dev/...", "timestamp": "..." },
      { "id": "act_...", "type": "click", "custom_id": "next-page", "timestamp": "..." }
      // ...repeated for each of the 5 iterations
    ]
  }
}

```

## Next Button Pagination

This example opens a demo site that shows a cookie-consent modal on load and pages through a virtualised product list using a single "Next page" button, looping through all 3 pages.

### API Request

The request below uses the [POST endpoint](../../../api-reference/post-v1-browser-requests.md) to open the [demo site](https://demo.gaffa.dev/simulate/ecommerce?loadTime=1\&showModal=true\&modalDelay=1\&loadingMode=paged\&pagingStyle=next\&pageCount=3\&pageSize=3\&itemCount=30\&itemLoadTime=0\&isVirtualScroll=true\&page=1), dismiss the cookie modal, wait for the first product to render, then loop up to 3 times: on each iteration, it waits briefly, captures the DOM, and clicks "Next page". _**You can run this request in the**_ [_**Gaffa API Playground**_](https://gaffa.dev/dashboard/playground?templateId=loop_pagination_next_button)_**.**_

```json
{
 "url": "https://demo.gaffa.dev/simulate/ecommerce?loadTime=1&showModal=true&modalDelay=1&loadingMode=paged&pagingStyle=next&pageCount=3&pageSize=3&itemCount=30&itemLoadTime=0&isVirtualScroll=true&page=1",
 "async": false,
 "max_cache_age": 0,
 "settings": {
   "time_limit": 30000,
   "record_request": true,
   "actions": [
     { "type": "wait", "selector": "div[role=\"dialog\"]", "timeout": 8000, "continue_on_fail": true },
     { "type": "click", "selector": "[data-testid=\"accept-all-button\"]", "timeout": 5000, "continue_on_fail": true },
     { "type": "wait", "selector": "[data-testid=\"product-1\"]", "timeout": 10000 },
     {
       "type": "loop",
       "custom_id": "pagination-loop",
       "max_iterations": 3,
       "timeout": 20000,
       "stop_on_fail": true,
       "continue_on_fail": true,
       "actions": [
         { "type": "wait", "time": 500, "custom_id": "settle" },
         { "type": "capture_dom", "custom_id": "page-dom" },
         {
           "type": "click",
           "selector": "button:has-text('Next page')",
           "timeout": 5000,
           "custom_id": "next-page"
         }
       ]
     }
   ]
 }
}
```

### Response

As with the numbered pagination example, the loop's nested actions are flattened into the response's `actions` array in execution order, once per page:

```json
{
  "data": {
    "id": "brq_...",
    "url": "https://demo.gaffa.dev/simulate/ecommerce?...&page=1",
    "state": "completed",
    "actions": [
      { "id": "act_...", "type": "wait", "timestamp": "..." },
      { "id": "act_...", "type": "click", "timestamp": "..." },
      { "id": "act_...", "type": "wait", "timestamp": "..." },
      {
        "id": "act_...",
        "type": "loop",
        "custom_id": "pagination-loop",
        "iterations": 3,
        "timestamp": "..."
      },
      { "id": "act_...", "type": "wait", "custom_id": "settle", "timestamp": "..." },
      { "id": "act_...", "type": "capture_dom", "custom_id": "page-dom", "output": "...", "timestamp": "..." },
      { "id": "act_...", "type": "click", "custom_id": "next-page", "timestamp": "..." }
      // ...repeated for each of the 3 iterations
    ]
  }
}
```

## Show More Pagination

This example opens a demo site that shows a cookie-consent modal on load and loads more products via a "Show more" button, looping through all 4 pages of results.

### API Request

The request below uses the [POST endpoint](../../../api-reference/post-v1-browser-requests.md) to open the [demo site](https://demo.gaffa.dev/simulate/ecommerce?loadTime=1\&showModal=true\&modalDelay=1\&loadingMode=paged\&pagingStyle=show-more\&pageCount=4\&pageSize=10\&itemCount=40\&itemLoadTime=0\&isVirtualScroll=false\&page=1), dismiss the cookie modal, wait for the first product to render, then loop up to 4 times: on each iteration, it waits briefly, captures the DOM, takes a full-screen screenshot, and clicks "Show more". _**You can run this request in the**_ [_**Gaffa API Playground**_](https://gaffa.dev/dashboard/playground?templateId=loop_show_more_pagination)_**.**_

```json
{
 "url": "https://demo.gaffa.dev/simulate/ecommerce?loadTime=1&showModal=true&modalDelay=1&loadingMode=paged&pagingStyle=show-more&pageCount=4&pageSize=10&itemCount=40&itemLoadTime=0&isVirtualScroll=false&page=1",
 "async": false,
 "max_cache_age": 0,
 "settings": {
   "time_limit": 45000,
   "record_request": true,
   "actions": [
     { "type": "wait", "selector": "div[role=\"dialog\"]", "timeout": 8000, "continue_on_fail": true },
     { "type": "click", "selector": "[data-testid=\"accept-all-button\"]", "timeout": 5000, "continue_on_fail": true },
     { "type": "wait", "selector": "[data-testid=\"product-1\"]", "timeout": 10000 },
     {
       "type": "loop",
       "custom_id": "showmore-loop",
       "max_iterations": 4,
       "timeout": 35000,
       "stop_on_fail": true,
       "continue_on_fail": true,
       "actions": [
         { "type": "wait", "time": 500, "custom_id": "settle" },
         { "type": "capture_dom", "custom_id": "page-dom" },
         { "type": "capture_screenshot", "custom_id": "page-screenshot" },
         {
           "type": "click",
           "selector": "button:has-text('Show more')",
           "timeout": 5000,
           "custom_id": "show-more"
         }
       ]
     }
   ]
 }
}
```

### Response

```json
{
  "data": {
    "id": "brq_...",
    "url": "https://demo.gaffa.dev/simulate/ecommerce?...&page=1",
    "state": "completed",
    "actions": [
      { "id": "act_...", "type": "wait", "timestamp": "..." },
      { "id": "act_...", "type": "click", "timestamp": "..." },
      { "id": "act_...", "type": "wait", "timestamp": "..." },
      {
        "id": "act_...",
        "type": "loop",
        "custom_id": "showmore-loop",
        "iterations": 4,
        "timestamp": "..."
      },
      { "id": "act_...", "type": "wait", "custom_id": "settle", "timestamp": "..." },
      { "id": "act_...", "type": "capture_dom", "custom_id": "page-dom", "output": "...", "timestamp": "..." },
      { "id": "act_...", "type": "capture_screenshot", "custom_id": "page-screenshot", "output": "https://storage.gaffa.dev/...", "timestamp": "..." },
      { "id": "act_...", "type": "click", "custom_id": "show-more", "timestamp": "..." }
      // ...repeated for each of the 4 iterations
    ]
  }
}
```
