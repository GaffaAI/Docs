# Using the Gaffa LLMs.txt File with Your AI Assistant

AI assistants like ChatGPT or Claude can generate working code far more effectively when they have accurate, up-to-date context about an API. That's exactly what Gaffa's `llms.txt` file provides. It provides a concise reference covering Gaffa's endpoints, actions, and code samples that you can drop directly into any AI assistant to get useful, accurate code from the very first prompt.

In this tutorial, we'll walk you through how to use the `llms.txt` file to build a complete Python script that interacts with the Gaffa API.

#### Step 1: Get the LLMs.txt File

Download or open the file at [https://gaffa.dev/docs/llms.txt](https://gaffa.dev/docs/llms.txt). It contains a concise overview of the Gaffa API, including available endpoints, actions, and example payloads.

#### Step 2: Load It Into Your AI Assistant

Start a new chat with ChatGPT, Claude, or your preferred AI assistant, then paste the full contents of the file into the conversation. This gives the assistant accurate, up-to-date context about the Gaffa API before you ask it anything.

#### Step 3: Ask the Assistant to Write Your Script

Once the assistant has the context loaded, you can ask it to build scripts for you. For example:

> _"Write me a Python script that uses Gaffa's browser API to convert a page into Markdown and save the output file locally."_

Because the assistant already has the full API context, it can produce accurate code without you needing to explain endpoint structures or payload formats.

#### Step 4: Example Script

Here's an example of the kind of script your AI assistant might generate, based directly on the Gaffa API. It submits a browser request to convert a page to Markdown, polls until the request completes, and downloads the output file.

```python
import os, time, requests, pathlib, urllib.parse

API_KEY = os.environ.get("GAFFA_API_KEY", "YOUR_API_KEY")
BASE = "https://api.gaffa.dev"

def submit_request(url, actions, async_mode=True):
    payload = {
        "url": url,
        "async": async_mode,
        "settings": {"actions": actions}
    }
    r = requests.post(
        f"{BASE}/v1/browser/requests",
        headers={"X-API-Key": API_KEY, "Content-Type": "application/json"},
        json=payload
    )
    r.raise_for_status()
    return r.json()["data"]

def wait_for_completion(request_id, poll_every=2, max_wait=180):
    start = time.time()
    while True:
        r = requests.get(
            f"{BASE}/v1/browser/requests/{request_id}",
            headers={"X-API-Key": API_KEY}
        )
        data = r.json()["data"]
        if data["state"] in ("completed", "failed"):
            return data
        if time.time() - start > max_wait:
            raise TimeoutError("Request timed out")
        time.sleep(poll_every)

def download_outputs(brq, dest="outputs"):
    dest = pathlib.Path(dest)
    dest.mkdir(parents=True, exist_ok=True)
    files = []
    for act in brq.get("actions") or []:
        out = act.get("output")
        if isinstance(out, str) and out.startswith("http"):
            name = pathlib.Path(urllib.parse.urlparse(out).path).name
            p = dest / name
            with requests.get(out, stream=True) as r:
                with open(p, "wb") as f:
                    for chunk in r.iter_content(8192):
                        if chunk: f.write(chunk)
            files.append(str(p))
    return files

if __name__ == "__main__":
    target_url = "https://demo.gaffa.dev/simulate/article?paragraphs=5"
    actions = [
        {"type": "wait", "selector": "article"},
        {"type": "generate_markdown"}
    ]
    job = submit_request(target_url, actions)
    brq = wait_for_completion(job["id"])
    print("Final state:", brq["state"])
    if brq["state"] == "completed":
        saved = download_outputs(brq)
        print("Downloaded:", saved)
```

Run it with:

```bash
python gaffa_script.py
```

You'll see the job state printed in your terminal and a downloaded Markdown file saved to an `outputs` folder.

#### Step 5: Extend and Customise

From here, you can modify the `actions` list to use other supported operations, such as `print`, `capture_screenshot`, or `capture_dom`. You can make these changes manually, or simply ask your AI assistant to adapt the script for you. Since it still has the `llms.txt` context loaded, it can adjust the code to your specific requirements without needing any further explanation.
