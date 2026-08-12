# Download File

**Type**: `download_file`

Request a copy of the most recently viewed file in the browser.

<details>

<summary><strong>Tips for using <code>download_file</code> action</strong></summary>

* Gaffa keeps track of files the browser downloads, so you can run this action at any point afterwards to collect one.
* It works whether the file downloads automatically when the page loads or is triggered by an earlier action, such as a [`click`](click.md).
* It returns the most recent download, so add a `download_file` action for each file if you're expecting several.
* Set a `timeout` long enough for the file to finish downloading. Larger files need longer, and the action waits until that limit is reached.
* Check your file type is supported: **.pdf, .jpg, .png, .gif, .bmp, .webp, .svg, .tiff, .tif** and **.img.**
* Each `download_file` action collects a file, which is then consumed. Add one action per file you're expecting.
* Don't add more `download_file` actions than there are files. Any extras will wait out their timeout and fail.

</details>

### Parameters

<table data-full-width="false"><thead><tr><th width="214">Name</th><th width="130">Type</th><th width="108" data-type="checkbox">Required</th><th>Description</th></tr></thead><tbody><tr><td><code>timeout</code></td><td><code>integer</code></td><td>false</td><td>The maximum amount of time the browser should wait for a file to download. <strong>Default: 5,000 (5s)</strong></td></tr></tbody></table>

See [universal parameters](./#universal-parameters).

### Files Supported

Currently, this only works with the following file formats: **.pdf, .jpg, .png, .gif, .bmp, .webp, .svg, .tiff, .tif, .img**

### Usage

#### Download a copy of a PDF and open it in the Browser

The following waits 20s for a file to download and then returns it.

```
"actions": [
    {
        "type": "download_file",
        "timeout": 20000
    }
]
```

And the service responds with the file being in the action output:

```
"actions": [
      {
        "id": "act_VHhrUbXjZSaYCPTqbBYD4acCzzeFGH",
        "type": "download_file",
        "query": "download_file?continue_on_fail=false&timeout=20000",
        "timestamp": "2025-05-30T15:02:06.6615306Z",
        "output": "https://storage.gaffa.dev/brq/downloads/5845df07-3749-424e-9c64-9602be19a857.pdf"
      }
    ]
```

### FAQs

#### When do I use the download\_file action?

Use it when the thing you want is already a file — a PDF, an image, a report. It returns the original file rather than a version re-rendered from the page.

#### How do I download a PDF with Gaffa?

Point the browser request to the PDF's URL and add a `download_file` action with a `timeout` long enough for the file to finish downloading. The response gives you a storage URL.

#### Which file types does the download\_file action support?

It supports **.pdf, .jpg, .png, .gif, .bmp, .webp, .svg, .tiff, .tif** and **.img**. Other file types can't be downloaded with this action.

#### What timeout should I set for the download\_file action?

Set it to cover the time it takes to download the file. The 5,000ms default suits small images, while larger documents need more — 20,000ms is a good starting point.

#### How do I download a file that starts downloading on its own?

Just add a `download_file` action. Gaffa tracks downloads as they occur, so the file is collected whether it starts on page load or is triggered by an earlier action.

#### How do I download a file that's behind a link?

Add a [`click`](click.md) for the link, then a `download_file` action with a long enough `timeout`. The click starts the download and `download_file` collects it.

#### How do I download multiple files in a request?

Add a `download_file` action for each file. Each action collects one download and removes it from the queue, so the next action picks up the following file. Give each a `custom_id` to tell them apart.

#### Why did my second download\_file action time out?

It was waiting for a download that hadn't happened. Each action consumes one file, so a second action only works once a second download has been triggered.

#### Does download\_file work on a PDF that opens in the browser?

Yes. It doesn't matter whether the PDF opens in the browser's viewer or downloads when you click a link. Either way, the action returns a storage URL for the file.

#### Where do I find the downloaded file?

The action's output field in the response contains a storage URL for the file, alongside the action's `id`, `type`, `query` and `timestamp`. The file keeps its original name.
