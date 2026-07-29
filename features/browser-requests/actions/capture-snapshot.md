# Capture Snapshot

**Type:** `capture_snapshot`

This output type will return an HTML file that captures a static version of the page state. The page will load offline and can be saved to your local machine.

This will:

* Load and embed all images on the page.
* Embed all CSS files

Currently, JavaScript will be disabled, and interactivity might not work as expected, but this feature should be useful for preserving the page state as it was and allowing you to view it offline.

### Parameters

See [universal parameters](./#universal-parameters)

### Usage

The following captures the current section of the page currently visible in the browser.

```json
"actions": [
    {
        "type": "capture_snapshot"
    }
]
```

### Example Output

Here's an example that shows an offline snapshot of a site

{% file src="../../../.gitbook/assets/GaffaSnapshotSample.mhtml" %}
