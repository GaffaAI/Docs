# Print

**Type**: `print`

Request that the browser print the page to a PDF.

<details>

<summary><strong>Tips for using <code>print</code> action</strong></summary>

* The PDF uses the site's print styles, so it may not look like the page on screen.
* Use `"orientation": "landscape"` for wide content, such as tables and dashboards.
* Set `"margin": 0` if you want the content to reach the edge of the page. The default is 20px.
* Don't use `print` on a page that's already a PDF. Use [`download_file`](download-file.md) to get the original instead.

</details>

### Parameters

<table data-full-width="false"><thead><tr><th width="226">Name</th><th width="130">Type</th><th width="108" data-type="checkbox">Required</th><th>Description</th></tr></thead><tbody><tr><td><code>size</code></td><td><code>string</code></td><td>false</td><td>The size of paper the page should be printed to.<br><strong>Default:</strong> <code>A4</code><br><strong>Accepted</strong>: <code>["A4"]</code></td></tr><tr><td><code>margin</code></td><td><code>integer</code></td><td>false</td><td>The margin of the page in pixels when the page is printed to PDF.<br><strong>Default: 20</strong></td></tr><tr><td><code>orientation</code></td><td><code>string</code></td><td>false</td><td>The orientation of the printed PDF page.<br><strong>Default: portrait</strong><br><strong>Accepted:</strong> <code>["portrait", "landscape"]</code></td></tr><tr><td></td><td></td><td>false</td><td></td></tr></tbody></table>

See [universal parameters](./#universal-parameters).

### Usage

#### Print a page in landscape to PDF

The following JSON prints the page to a PDF in landscape orientation with a 20px margin.

```json
"actions": [
    {
        "type": "print",
        "size": "A4",
        "orientation": "landscape",
        "margin": 20
    }
]
```

### Example Output

{% file src="../../../.gitbook/assets/GaffaPrintPdfExample.pdf" %}

### FAQs

#### When do I use the print action?

Use it when you need a PDF of a web page — for a report, a record, or something to share. It gives you a paginated A4 document rather than an image or HTML.

#### How do I convert a web page to PDF with Gaffa?

Add a `print` action to your request. It renders the page to an A4 PDF using the site's print styles. Use `orientation` and `margin` to control the layout.

#### How do I print a page in landscape?

Set `"orientation": "landscape"`. Use it for wide tables, dashboards and anything that loses columns when squeezed into portrait A4.

#### Why does my PDF look different to the web page?

Most sites have a separate print stylesheet that hides navigation or changes colours. If you need the page as it looks on screen, use [`capture_screenshot`](capture-screenshot.md) with `size: "fullscreen"` instead.

#### What paper sizes can I use?

A4 only at the moment, and it's the default. Use `orientation` and `margin` to control everything else about the layout.

#### How do I remove the margins?

Set `"margin": 0`. The default is 20 pixels, which suits documents but crops designs meant to reach the edge of the page.

#### When should I use download\_file action instead of print action?

Use [`download_file`](download-file.md) when the browser already has a PDF open, and you want the original file. Use `print` to create a PDF from a page that isn't one yet.
