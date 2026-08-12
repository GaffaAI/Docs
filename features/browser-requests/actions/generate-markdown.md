# Generate Markdown

Type: `generate_markdown`

The Markdown output format exports page data (articles, tables, etc.) in a human- and LLM-readable format, removing unnecessary styling and other "junk" that is only relevant to the site's proper functioning.

Gaffa exports [GitHub-flavoured markdown](https://github.github.com/gfm/) with comments removed and unknown tags ignored.

<details>

<summary><strong>Tips for using <code>generate_markdown</code></strong></summary>

* Use this as your default when sending page content to an LLM. It keeps headings, lists, tables and links, and drops the rest.
* Add a `selector` to focus on a particular area of the page and cut out the noise. Pointing at `article` or `main` skips navigation, sidebars and footers.
* Use `output_type: "inline"` to get the Markdown straight back in the response. Use `file` for long pages.
* Use [`generate_simplified_dom`](generate-simplified-dom.md) instead if you need selectors, because Markdown drops classes and attributes.

</details>

### Parameters

<table><thead><tr><th width="184.21875">Name</th><th width="130.66796875">Type</th><th width="106.7734375" data-type="checkbox">Required</th><th width="206.73046875">Description</th></tr></thead><tbody><tr><td><code>selector</code></td><td>string</td><td>false</td><td>The <a href="../selectors.md">selector</a> that defines an element you want to generate markdown from. This is useful if you are only interested in the contents of a certain element.</td></tr><tr><td><code>output_type</code></td><td>string</td><td>false</td><td>Should the action output be saved to a file where a URL will be returned or should the parsed JSON object be included directly in the request.<br><br><strong>Default:</strong> <code>file</code><br><strong>Accepted</strong>: <code>["file", "inline"]</code></td></tr></tbody></table>

See [universal parameters](./#universal-parameters).

### Usage

The following converts the current page to Markdown:

```json
"actions": [
  {
    "type": "generate_markdown"
  }
]
```

The following converts only a specific element to Markdown and returns it inline:

```json
"actions": [
  {
    "type": "generate_markdown",
    "selector": "article",
    "output_type": "inline"
  }
]
```

### Example Output

{% file src="../../../.gitbook/assets/GaffaMarkdownExample.md" %}

### FAQs

#### When do I use generate\_markdown?

Use it when you want the readable content of a page, article, listing, or documentation page, especially for sending to an LLM. It's the cleanest and smallest output Gaffa produces.

#### What does generate\_markdown output?

It outputs GitHub-flavoured Markdown of the page, with comments removed and unknown tags ignored. Headings, lists, links and tables are kept. Styling and scripts are stripped out.

#### How do I convert only part of a page to Markdown?

Set the `selector` parameter to the element you want, like `"article"` or `"main"`. Gaffa converts only that element's contents, so you skip navigation, sidebars and footers.

#### Should I use file or inline output?

Use `inline` to get the Markdown directly in the response, which suits agents and shorter pages. Use `file`, the default, when the page is long.

#### **When should I use capture\_dom instead of generate\_markdown?**

Use [`capture_dom`](capture-dom.md) when you need the actual markup attributes, scripts, and full link URLs. Use `generate_markdown` when you only need the content, which is most of the time.

#### When should I use parse\_json instead of generate\_markdown?

Use [`parse_json`](parse-json.md) when you need specific fields in a fixed structure. Use `generate_markdown` when you want the whole page as text and will handle the rest yourself.

#### Does generate\_markdown keep tables?

Yes, as Markdown tables. If you plan to process the table in code, use [`parse_table`](parse-table.md) instead; it returns proper JSON rows rather than text.

#### Should I use file or inline output for generate\_markdown?

Use `inline` to get the Markdown directly in the action's `output` field, which suits agents and shorter pages. Use `file` (the default) when the page is long — that returns a storage URL instead.
