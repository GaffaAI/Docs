# Capture DOM

**Type:** `capture_dom`

This action will capture and return the site's raw DOM, which you can then extract data from on your end.

For common AI scenarios, you may find that this returns too much data, so we have provided a [`generate_simplified_dom`](generate-simplified-dom.md) , an action that distils the DOM to only the important elements.

<details>

<summary><strong>Tips for using <code>cature_dom</code> action</strong></summary>

* Use it when you want to parse the page yourself with a traditional HTML library such as BeautifulSoup, lxml, or Cheerio.
* It can also be used for debugging. When a selector isn't matching, the raw DOM shows you what's actually on the page.
* Use it when you need something the other actions strip out, like inline scripts, `data-` attributes or full link URLs.
* Don't send the output to an LLM. Use [`generate_simplified_dom`](generate-simplified-dom.md) or [`generate_markdown`](generate-markdown.md) instead, which carry the same information for far fewer tokens.
* It captures the page as it is at that point in the run, so you can use it multiple times to compare the state before and after an interaction.
* If content is missing from the capture, the page probably hadn't finished loading. Add a [`wait`](wait.md) before it.

</details>

### Parameters

See [universal parameters](./#universal-parameters).

### Usage

Capture the raw DOM of the current page

```
"actions": [
    {
      "type": "capture_dom"
    }
]
```

### Example Output

{% file src="../../../.gitbook/assets/GaffaDOMSample.txt" %}

### FAQs

#### When do I use the capture\_dom action?

Use it when you want to parse the page yourself with a library like BeautifulSoup, or when you need scripts, attributes and link query strings that the simplified and Markdown outputs remove. See our[ guide to scraping tables with Python](https://gaffa.dev/blog/how-to-scrape-a-table-with-python-\(the-easy-way\)).

#### What does capture\_dom action return?

It returns the page's raw DOM as the browser rendered it, including scripts, styles and every attribute. This is the page after JavaScript has run, not the original server HTML.

#### Why is content missing from my capture\_dom output?

The action probably ran before the page finished loading. Add a [`wait`](wait.md) action with a selector for the content you're expecting, so Gaffa captures the page once it's there.

#### When should I use generate\_simplified\_dom instead of capture\_dom?

Use [`generate_simplified_dom`](generate-simplified-dom.md) whenever the output goes to an LLM, or you're looking for `selectors`. It keeps the page structure but removes the scripts and styling that make raw DOM so large. See our[ guide to simplifying a webpage DOM](https://gaffa.dev/blog/how-to-extract-and-simplify-a-webpage-dom-with-gaffa).

#### Why is my capture\_dom output so big?

Raw DOM includes all scripts and styles on the page. Use [`generate_simplified_dom`](generate-simplified-dom.md) to strip those out, or [`capture_element`](capture-element.md) with a selector to capture only the part you need.

#### How do I capture only part of the page with capture\_dom action?

Use [`capture_element`](capture-element.md) with a selector instead. It gives you the same raw HTML, but only for the element you point it at.
