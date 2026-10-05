# Generate Simplified DOM

**Type:** `generate_simplified_dom`

When you're looking at the DOM of a web page, there's a lot of unnecessary data that can be discarded if you are only interested in the page's elements or looking to export the data into an LLM.\
\
The `generate_simplified_dom` output format processes the HTML in the following way:

* Removes all links in the `head`
* Removes all `script` nodes and links to scripts
* Removes all `style` nodes
* Remove `style` attributes from all elements
* Remove all links to stylesheets
* Remove all `noscript` elements outside of the body
* Finds all `hrefs` with query strings and removes the query strings
* Important `meta` tags are kept, all others are removed
* Remove all `alternate` links
* Remove all SVG paths
* Remove empty text nodes and excessive spacing

<details>

<summary><strong>Tips for using <code>generate_simplified_dom</code></strong></summary>

* Use this when an agent needs to look at a page and work out what to click or type next. It keeps the classes, IDs and structure that selectors are built from.
* Use it instead of [`capture_dom`](capture-dom.md) for anything going to an LLM. Same structure, far fewer tokens.
* Don't use it to debug styling — styles and stylesheets are removed.
* Note that query strings are stripped from links, so use [`capture_dom`](capture-dom.md) if you're collecting URLs with parameters.

</details>

### Parameters

See [universal parameters](./#universal-parameters).

### Usage

The following JSON captures the page's DOM and simplifies it.

```json
"actions": [
    {
        "type": "generate_simplified_dom"
    }
]
```

{% hint style="info" %}
We are actively working to improve this and to make this process more configurable - let us know if there's something you think we can improve.
{% endhint %}

### Example Output

{% file src="../../../.gitbook/assets/GaffaSimplifiedDOMSample.txt" %}

### **FAQs**

#### When do I use generate\_simplified\_dom?

Use it when you need to understand a page's structure — usually to find selectors on a site you haven't scraped before, or to give an AI agent something to plan its next action from.

#### What does generate\_simplified\_dom remove?

It removes scripts, styles and style attributes, stylesheet and alternate links, noscript elements outside the body, SVG paths, most meta tags, query strings on links, and empty text.

#### When should I use capture\_dom instead of generate\_simplified\_dom?

Use [`capture_dom`](capture-dom.md) when you need something simplification removes — inline scripts, style attributes or full link URLs. Use the simplified version for everything else.

#### When should I use generate\_markdown instead of generate\_simplified\_dom?

Use [`generate_markdown`](generate-markdown.md) when you only need the content. Use `generate_simplified_dom` when you need the structure too, like classes and IDs to build selectors from.

#### How can an AI agent find selectors on a page it doesn't know?

Run `generate_simplified_dom` first, look through the structure for stable IDs, classes or ARIA labels, then build [`click`](click.md), [`type`](type.md) or [`capture_element`](capture-element.md) actions from what you find.

#### Does generate\_simplified\_dom keep links?

Yes, but without their query strings. If you need full URLs with parameters, use [`capture_dom`](capture-dom.md) or [`capture_element`](capture-element.md) instead.
