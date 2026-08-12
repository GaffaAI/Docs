# Block DOM Removals

**Type:** `block_dom_removals`

This action will prevent the page from removing items from the page. This is useful if you are trying to scrape data from a JavaScript-based web application that removes items from the page when they are out of view, which can make grabbing data difficult.

Using this action will block DOM removals for the rest of the browser request.

<details>

<summary><strong>Tips for using <code>block_dom_removal</code> action</strong></summary>

* Put this action first, before any scrolling. It only protects what happens after it runs.
* Use it if you scroll a long list and only the last few rows end up in your capture.
* The usual pattern is `block_dom_removals`, then [`scroll`](scroll.md), then a capture action.
* Expect a larger DOM since nothing gets cleared.

</details>

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

### FAQs

#### When do I use the block\_dom\_removals action?

Use it when a site removes elements from the page as you scroll past them. This is common in long feeds and data grids. Blocking removals keeps every loaded row on the page.

#### How do I know a site is removing elements?

Scroll the page yourself with DevTools open and watch the Elements panel. If earlier rows disappear as new ones load, the site is recycling elements, and you need this action.

#### Does the block\_dom\_removals action affect the whole request?

Yes. Once it runs, removals are blocked for all subsequent actions. Put it early because it can't restore elements the page has already removed.

#### Does the block\_dom\_removals action take any parameters?

No. It only takes the [universal parameters](./#universal-parameters): `type`, `continue_on_fail`, and `custom_id`. Add `"type": "block_dom_removals"` to your actions array and nothing else.
