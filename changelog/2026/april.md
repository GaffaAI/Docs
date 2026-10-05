# April

### Samples, Blog & Tutorials

#### How to Scrape a Table with Python (The Easy Way)

We published a full walkthrough of both table scraping approaches using [`capture_dom`](https://gaffa.dev/docs/features/browser-requests/actions/capture-dom) with BeautifulSoup and using Gaffa's [`parse_table`](https://gaffa.dev/docs/features/browser-requests/actions/parse-table) action, directly covering when to use each and how to get clean, structured output either way. Read the [blog](https://gaffa.dev/blog/how-to-scrape-a-table-with-python-\(the-easy-way\)).

#### Web Scraping with JavaScript Using Gaffa

We published a full walkthrough on scraping the web in JavaScript using Gaffa's REST API. It covers two core actions: [`generate_markdown`](https://gaffa.dev/docs/features/browser-requests/actions/generate-markdown) for extracting clean, readable content and [`capture_dom`](https://gaffa.dev/docs/features/browser-requests/actions/capture-dom) for pulling raw HTML when you need more control. Also touches on dynamic content handling, geo-routing, async mode, and actions like [`parse_table`](https://gaffa.dev/docs/features/browser-requests/actions/parse-table) and [`parse_json`](https://gaffa.dev/docs/features/browser-requests/actions/parse-json). Read the [blog](https://gaffa.dev/blog/web-scraping-with-javascript-using-gaffa).

#### Gaffa at Major League Hacking's Global Hack Week

We presented a live session at MLH's Global Hack Week Cloud, walking through web scraping fundamentals, an intro to Gaffa, and two hands-on demos, one scraping a Wikipedia article and feeding it into an OpenAI Q\&A session using [`generate_markdown`](https://gaffa.dev/docs/features/browser-requests/actions/generate-markdown), and another using [`parse_json`](https://gaffa.dev/docs/features/browser-requests/actions/parse-json) to extract structured data from both web pages and hosted PDFs. The session also covered scraping legality, dynamic content, and how `parse_json` field descriptions can enforce specific output formats. We've also added both demo notebooks to our Python examples repository:

* [`webpage_to_markdown_and_qa.ipynb`](https://github.com/GaffaAI/GaffaPythonExamples/blob/main/talks/MLH_Global_Hack_Week/webpage_to_markdown_and_qa.ipynb) — Scraping a Wikipedia page and using it as context for an OpenAI Q\&A session.
* [`structured_data_extraction_with_parse_json.ipynb`](https://github.com/GaffaAI/GaffaPythonExamples/blob/main/talks/MLH_Global_Hack_Week/structured_data_extraction_with_parse_json.ipynb) — Extracting structured data from web pages and hosted PDFs using `parse_json`.

Read the [recap](https://gaffa.dev/blog/gaffa-at-major-league-hackings-global-hack-week).
