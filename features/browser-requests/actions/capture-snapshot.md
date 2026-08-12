# Capture Snapshot

**Type:** `capture_snapshot`

This output type will return an HTML file that captures a static version of the page state. The page will load offline and can be saved to your local machine.

This will:

* Load and embed all images on the page.
* Embed all CSS files

Currently, JavaScript is disabled, and interactivity might not work as expected, but this feature should help preserve the page state as it was and allow you to view it offline.

<details>

<summary><strong>Tips for using <code>capture_snapshot</code> action</strong></summary>

* Use this when you want to revisit the page later. Unlike a screenshot, you can still read and search the HTML.
* JavaScript is switched off in the saved file, so anything interactive is frozen as it was.
* Open any tabs or expand any sections before you capture, because they won't open in the saved file.
* Snapshots are large, since every image and stylesheet is embedded in the file.

</details>

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

### FAQs

#### When do I use the capture\_snapshot action?

Use it when you need a copy of a page to open and read later for record-keeping, evidence, or to check back on a page after it's changed or gone offline.

#### What does the capture\_snapshot action produce?

It produces a single HTML file with all the images and CSS built in, so the page opens offline on your machine without needing the original site or a network connection.

#### Does JavaScript work in a Gaffa snapshot?

No. JavaScript is disabled in the saved file, so tabs, carousels, and dropdowns won't respond. The snapshot keeps the page exactly as it was when you captured it.

#### How do I take a snapshot of a page with content behind a tab?

Add a [`click`](click.md) for the tab, then a [`wait`](wait.md) for its content to appear, then `capture_snapshot`. Scripts won't run in the saved file, so open everything you need beforehand.

#### When should I use capture\_screenshot instead of capture\_snapshot?

Use a screenshot when you just need to see the page. Use a snapshot when you'll want to read or pull data out of it later, since a snapshot is still HTML.

#### When should I use print instead of capture\_snapshot?

Use [`print`](print.md) when you need a PDF to share or file. Use `capture_snapshot` when you need a working copy of how the page looked in the browser.
