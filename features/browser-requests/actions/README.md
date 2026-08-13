# Actions

When [making a Browser Request](../../../api-reference/post-v1-browser-requests.md), you can specify a list of actions you want us to perform on the requested web page. These actions conform to the following format:

{% code overflow="wrap" fullWidth="false" %}
```json
{
    "type": "", //the type of the action
    //other params follow as key-value pairs
    "key": value //string, number, etc. 
}
```
{% endcode %}

### Universal Parameters

All actions have the following parameters:

<table data-full-width="false"><thead><tr><th width="226">Name</th><th width="130">Type</th><th width="108" data-type="checkbox">Required</th><th>Description</th></tr></thead><tbody><tr><td><code>type</code></td><td><code>string</code></td><td>true</td><td>The type name of the action.</td></tr><tr><td><code>continue_on_fail</code></td><td><code>boolean</code></td><td>false</td><td>Should execution of further actions continue or throw an error if this action fails.<br><strong>Default:</strong> <code>false</code></td></tr><tr><td><code>custom_id</code></td><td><code>string</code></td><td>false</td><td>A custom_id to help you find the action in the response.<br><strong>Default:</strong> <code>null</code></td></tr></tbody></table>

#### Action Execution

Actions are carried out in the order they are submitted. Every action type has a `continue_on_fail` parameter, which defaults to `false`.This means that if any action fails, the execution of the browser request ends, and an error will be returned. Setting `continue_on_fail` to `true` ensures that all actions are carried out, regardless of the previous action's results, and an error will not be returned.

#### Custom Id

As shown above, you can submit a `custom_id` with each action you submit to the API. We'll include this ID in the outputs from the browser request so you can find a certain action's output and/or status easily in the response.

## Response Format

When a browser request has completed, information on an action's execution

{% code fullWidth="false" %}
```json
{
    "id": "", //a unique id given to the action by Gaffa
    "type": "capture_screenshot", //the type of the action
    "query": "", //a representation of the action in querystring format
    "timestamp": "", //the UTC timestamp the action was executed
    "output": "" //if the action has an output, you will find a URL for this here,
    "error": "" //if the request fails, the error message will be returned here
}
```
{% endcode %}

## Choosing an action

<table data-header-hidden><thead><tr><th width="271.1328125"></th><th width="242.2528076171875"></th><th></th></tr></thead><tbody><tr><td>I want to…</td><td>Use</td><td>Not</td></tr><tr><td>Send page content to an LLM</td><td><a href="generate-markdown.md"><code>generate_markdown</code></a></td><td><a href="capture-dom.md"><code>capture_dom</code></a> — too big and too noisy</td></tr><tr><td>Find selectors on a page I don't know</td><td><a href="generate-simplified-dom.md"><code>generate_simplified_dom</code></a></td><td><a href="generate-markdown.md"><code>generate_markdown</code></a> — it drops the structure you need</td></tr><tr><td>Work out why a selector isn't matching</td><td><a href="capture-dom.md"><code>capture_dom</code></a></td><td><a href="generate-simplified-dom.md"><code>generate_simplified_dom</code></a> — it may have removed what you're looking for</td></tr><tr><td>Turn one HTML table into rows</td><td><a href="parse-table.md"><code>parse_table</code></a></td><td><a href="parse-json.md"><code>parse_json</code></a> — you'd pay credits for something free, and it wouldn’t be deterministic</td></tr><tr><td>Pull structured data out of prose, a PDF, or a messy layout</td><td><a href="parse-json.md"><code>parse_json</code></a></td><td><a href="parse-table.md"><code>parse_table</code></a> — it only reads &#x3C;table> markup</td></tr><tr><td>Get the contents of one element I know about</td><td><a href="capture-element.md"><code>capture_element</code></a></td><td><a href="capture-dom.md"><code>capture_dom</code></a> — it returns the whole page</td></tr><tr><td>Show what a page looked like</td><td><a href="capture-screenshot.md"><code>capture_screenshot</code></a></td><td><a href="generate-markdown.md"><code>generate_markdown</code></a> — no visual record</td></tr><tr><td>Keep a copy of a page I can look at again later</td><td><a href="capture-snapshot.md"><code>capture_snapshot</code></a></td><td><a href="capture-screenshot.md"><code>capture_screenshot</code></a> — you can't search an image</td></tr><tr><td>Turn a page into a PDF</td><td><a href="print.md"><code>print</code></a></td><td><a href="capture-snapshot.md"><code>capture_snapshot</code></a> — that gives you HTML</td></tr><tr><td>Get a file the browser has open</td><td><a href="download-file.md"><code>download_file</code></a></td><td><a href="print.md"><code>print</code></a> — it re-renders the page instead of taking the file</td></tr><tr><td>Capture several pages of results in one request</td><td><a href="loop.md"><code>loop</code></a></td><td>Sending one request per page — you'd pay the page load each time |</td></tr></tbody></table>

## Supported Actions

The Gaffa API supports the following actions, detailed below. Click the "read more" buttons to read more information about each type.

### Actions without outputs

<table data-view="cards" data-full-width="true"><thead><tr><th>Type</th><th>Description</th><th>Read More</th></tr></thead><tbody><tr><td><code>block_dom_removals</code></td><td>Prevents the page from removing elements from the DOM.</td><td><a href="block-dom-removals.md">Block DOM Removals</a></td></tr><tr><td><code>click</code></td><td>Click on a given element</td><td><a href="click.md">Click</a></td></tr><tr><td><code>scroll</code></td><td>Scroll to a particular point on the page or, in the case of pages with infinite scrolling, scroll until a given time has elapsed.</td><td><a href="scroll.md">Scroll</a></td></tr><tr><td><code>type</code></td><td>Type the provided text into a given element</td><td><a href="type.md">Type</a></td></tr><tr><td><code>wait</code></td><td>Wait for a given time to elapse or an element to appear on page before proceeding to the next action.</td><td><a href="wait.md">Wait</a></td></tr></tbody></table>

### Actions with outputs

<table data-view="cards" data-full-width="true"><thead><tr><th>Type</th><th>Description</th><th>Read More</th></tr></thead><tbody><tr><td><code>capture_cookies</code></td><td>Save a JSON object of cookies for the current page</td><td><a href="capture-cookies.md">Capture Cookies</a></td></tr><tr><td><code>capture_dom</code></td><td>Export the raw DOM page data</td><td><a href="capture-dom.md">DOM</a></td></tr><tr><td><code>capture_screenshot</code></td><td>Capture a screenshot of the web page</td><td><a href="capture-screenshot.md">Screenshot</a></td></tr><tr><td><code>capture_element</code></td><td>Export the contents of a single element on the page</td><td><a href="capture-element.md">Capture Element</a></td></tr><tr><td><code>capture_snapshot</code></td><td>Create a completely static version of the web page which can be accessed offline</td><td><a href="capture-snapshot.md">Snapshot</a></td></tr><tr><td><code>download_file</code></td><td>Download an online file using Gaffa</td><td><a href="download-file.md">Download File</a></td></tr><tr><td><code>generate_markdown</code></td><td>Convert the page into markdown</td><td><a href="generate-markdown.md">Markdown</a></td></tr><tr><td><code>generate_simplified_dom</code></td><td>Generate a simplified version of the DOM</td><td><a href="generate-simplified-dom.md">Simplified DOM</a></td></tr><tr><td><code>parse_json</code></td><td>Parse online data to a defined JSON schema</td><td><a href="parse-json.md">JSON Parsing</a></td></tr><tr><td><code>parse_table</code></td><td>Convert an HTML table into JSON rows</td><td><a href="parse-table.md">Parse Table</a></td></tr><tr><td><code>print</code></td><td>Print the web page to a PDF</td><td><a href="print.md">Print</a></td></tr></tbody></table>

### Flow Actions

<table data-view="cards"><thead><tr><th>Type</th><th>Description</th><th>Read more</th></tr></thead><tbody><tr><td><code>loop</code></td><td>Runs nested actions repeatedly until it reaches a limit, times out, or an action fails.</td><td><a href="loop.md">Loop</a></td></tr></tbody></table>

<details>

<summary><strong>Tips</strong></summary>

* Actions run in the order you list them, so put your capture actions last.
* Set `continue_on_fail: true` on anything optional, like closing a cookie banner. Otherwise, a single failure halts the entire request.
* Set a `custom_id` on each action so you can find its output in the response.
* You don't need a [`wait`](wait.md) before [`click`](click.md), [`capture_element`](capture-element.md) or [`parse_table`](parse-table.md). They already wait for their own [`selector`](../selectors.md).
* Add a `selector` to [`generate_markdown`](generate-markdown.md) or [`parse_json`](parse-json.md) to reduce what you capture and what you pay for.
* If you're not sure an action worked, add a [`capture_screenshot`](capture-screenshot.md) after it.
* Use [`loop`](loop.md) when you need more than one page from a single request, rather than sending a request per page.
* Inside a [`loop`](loop.md), put capture actions before the action that navigates. Anything after a failing action is skipped for that pass.

</details>

***

<details>

<summary><strong>FAQs</strong></summary>

#### In what order do Gaffa actions run?

Actions run one after another, in the order you list them in the actions array.

#### How do I capture multiple pages in a single request?

Use the [`loop`](loop.md) action. It repeats a nested list of actions, so one request can page through search results or a numbered list. Loops can't be nested inside each other.

#### What happens when a Gaffa action fails?

The browser request stops and returns an error, and the remaining actions are skipped. Set `continue_on_fail: true` on that action if you want the rest to run anyway. Inside a [`loop`](loop.md), a failure skips the rest of that iteration rather than ending the request, and the loop's `stop_on_fail` decides whether it exits or starts the next pass.

#### How do I find the output of a specific action in the response?

Give the action a `custom_id` when you send it. Gaffa returns that ID in the response, so you can look up the action's output without counting through the array.

#### Can I use the same action twice in one request?

Yes. Add it at each point where you need it — for example, a before-and-after screenshot of a click. Give each one a different `custom_id` so you can tell them apart.

#### Which Gaffa actions cost extra credits?

[`parse_json`](parse-json.md) is the only one. It's billed on the tokens sent to and returned by the AI model. Keep costs down with `selector`, `input_token_cap` and `max_pages`.

</details>
