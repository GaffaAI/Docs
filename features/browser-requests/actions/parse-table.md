# Parse Table

**Type**: `parse_table`

The `parse_table` action finds a table on a page using a CSS selector and converts it into a structured JSON array with no HTML parsing or post-processing required on your end.

The action reads the table's header row and converts each header into a property name (lowercased, with non-alphanumeric characters replaced by underscores). It then maps each cell value to its corresponding header for every row, returning a clean, ready-to-use JSON array. At the moment, all values are returned as `string` types.

For cases where you need more control, such as handling merged cells, skipping rows, or applying custom transformations, consider using [`capture_dom`](https://gaffa.dev/docs/features/browser-requests/actions/capture-dom) with a parsing library like BeautifulSoup instead.

<details>

<summary><strong>Tips for using <code>parse_table</code></strong></summary>

* Use this rather than [`parse_json`](parse-json.md) whenever the data is in a real `<table>`. It's exact and costs nothing.
* Headers become your property names, lowercased with non-letters turned into underscores. `Ticket Price (£)` becomes `ticket_price_`.
* Every value comes back as a string, so convert numbers and dates on your side.
* Point the selector at the specific table. Pages often have several, including ones used for layout.
* Scroll and wait first if the table pages or loads as you scroll, so the rows are there when the action runs.

</details>

### Parameters

<table data-full-width="false"><thead><tr><th width="212">Name</th><th width="130">Type</th><th width="108" data-type="checkbox">Required</th><th>Description</th></tr></thead><tbody><tr><td><code>selector</code></td><td><code>string</code></td><td>true</td><td>The <a href="../selectors.md">selector</a> that identifies the table you want to parse.</td></tr><tr><td><code>timeout</code></td><td><code>integer</code></td><td>false</td><td>The maximum time in milliseconds to wait for the table to appear. <strong>Default: 5000 (5s)</strong></td></tr></tbody></table>

See [universal parameters](./#universal-parameters).

### Usage

#### Basic examples

The following request waits up to 1 second for a `.large_table` element to appear, then parses it into JSON:

```json
{
  "url": "https://example.com",
  "settings": {
    "actions": [
      {
        "type": "parse_table",
        "selector": ".large_table",
        "timeout": 1000
      }
    ]
  }
}
```

#### Real-world example

Here is an example using Wikipedia's [List of Countries by GDP (Nominal)](https://en.wikipedia.org/wiki/List_of_countries_by_GDP_\(nominal\)). Wikipedia applies a consistent CSS class to its data tables, making it straightforward to target with a selector:

```json
{
  "url": "https://en.wikipedia.org/wiki/List_of_countries_by_GDP_(nominal)",
  "settings": {
    "actions": [
      {
        "type": "parse_table",
        "selector": ".wikitable",
        "timeout": 5000
      }
    ]
  }
}
```

#### Sample output

Notice how column headers like "Country/Territory" and "IMF 2026" are automatically normalized into `country_territory` and `imf_2026.` Spaces and special characters are replaced with underscores, and everything is lowercased, so the output is immediately usable without any cleanup:

```json
[
  {
    "country_territory": "United States",
    "imf_2026": "30,337",
    "imf_year": "2026",
    "world_bank_2023": "27,361",
    "world_bank_year": "2023"
  },
  {
    "country_territory": "China",
    "imf_2026": "19,534",
    "imf_year": "2026",
    "world_bank_2023": "17,795",
    "world_bank_year": "2023"
  }
]
```

For a full walkthrough, including a comparison with the `capture_dom` + BeautifulSoup approach, see our blog post on [how to scrape a table with Python (the easy way)](https://gaffa.dev/blog/how-to-scrape-a-table-with-python-\(the-easy-way\)).

### FAQs

#### When do I use parse\_table?

Use it when the data you want is in an HTML table, and you want it as JSON rows. It's faster, cheaper and more predictable than AI parsing for this one job.

#### How do I extract a table as JSON?

Add a `parse_table` action with a selector for the table. Gaffa turns the headers into property names and each row into an object, and returns the result as JSON.

#### How does parse\_table name the properties?

It takes each header, makes it lowercase, and replaces anything that isn't a letter or number with an underscore. `Ticket Price (£)` becomes `ticket_price_`, so check your keys before mapping them.

#### What data types does parse\_table return?

Everything comes back as a string at the moment, including numbers and dates. Convert them to the types you need after you get the response.

#### When should I use parse\_json instead of parse\_table?

Use [`parse_json`](parse-json.md) when the data isn't in a real table — a grid built from divs, a list, a PDF, or plain prose — or when you need typed values rather than strings.

#### What if the data looks like a table but isn't one?

`parse_table` only works on `<table>` markup. For grids made of divs or lists, use [`parse_json`](parse-json.md) with a `schema`, or [`capture_element`](capture-element.md) and parse the HTML yourself.
