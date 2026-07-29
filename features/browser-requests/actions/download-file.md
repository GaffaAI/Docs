# Download File

**Type**: `download_file`

Request a copy of the most recently viewed file in the browser.

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
