# Print

**Type**: `print`

Request that the browser print the page to a PDF.

### Parameters

<table data-full-width="false"><thead><tr><th width="226">Name</th><th width="130">Type</th><th width="108" data-type="checkbox">Required</th><th>Description</th></tr></thead><tbody><tr><td><code>size</code></td><td><code>string</code></td><td>false</td><td>The size of paper the page should be printed to.<br><strong>Default:</strong> <code>A4</code><br><strong>Accepted</strong>: <code>["A4"]</code></td></tr><tr><td><code>margin</code></td><td><code>integer</code></td><td>false</td><td>The margin of the page in pixels when the page is printed to PDF.<br><strong>Default: 20</strong></td></tr><tr><td><code>orientation</code></td><td><code>string</code></td><td>false</td><td>The orientation of the printed PDF page.<br><strong>Default: portrait</strong><br><strong>Accepted:</strong> <code>["portrait", "landscape"]</code></td></tr></tbody></table>

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
