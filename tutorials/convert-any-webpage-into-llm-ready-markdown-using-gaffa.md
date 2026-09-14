# Convert any webpage into LLM-ready Markdown using Gaffa

The ability to convert websites into LLM-friendly markdown is powerful when building applications for summarization, Q\&A, or knowledge extraction. In this guide, you'll learn how to use the [Gaffa API](https://gaffa.dev/) to extract the main content of any web page using browser rendering and convert it into structured markdown.

By the end of this guide, you’ll be able to:

* Render web pages using Gaffa’s API.
* Extract clean page content.
* Generate structured markdown suitable for LLM-based Q\&A or summarization.

### **Prerequistes**

1. Install Python 3.10 or newer.
2. Create a virtual environment

```sh
python -m venv venv && source venv/bin/activate
```

3. Install the required libraries

```sh
pip install requests openai
```

4. Get your [Gaffa API](https://gaffa.dev/dashboard/api-keys) key and [OpenAI API](https://platform.openai.com/signup) key, and store them as environment variables:

```sh
export GAFFA_API_KEY=your_gaffa_api_key
export OPENAI_API_KEY=your_openai_api_key
```

### Convert a webpage to Markdown

In the code below, we define a function that takes a URL as input, makes a POST request to the Gaffa API, invoking the [generate\_markdown](../features/browser-requests/actions/generate-markdown.md) action, which uses the browser rendering engine to extract the page's main content and convert it to markdown.

{% code overflow="wrap" lineNumbers="true" %}
```python
import os
import requests
from openai import OpenAI

GAFFA_API_KEY = os.getenv("GAFFA_API_KEY")
OPENAI_API_KEY = os.getenv("OPENAI_API_KEY")

# Fetch the markdown content from Gaffa
def fetch_markdown_with_gaffa(url):
    payload = {
        "url": url,
        "proxy_location": None,
        "async": False,
        "max_cache_age": 0,
        "settings": {
            "record_request": False,
            "actions": [
                {
                    "type": "wait",
                    "selector": "article"
                },
                {
                    "type": "generate_markdown"
                }
            ]
        }
    }
   
    # Set the headers for the request
    headers = {
        "x-api-key": GAFFA_API_KEY,
        "Content-Type": "application/json"
    }
    # Make the POST request to the Gaffa API
    print("Calling Gaffa API to generate markdown...")
    response = requests.post("https://api.gaffa.dev/v1/browser/requests", json=payload, headers=headers)
    response.raise_for_status()
   
    # Extract the markdown URL from the response
    markdown_url = response.json()["data"]["actions"][1]["output"]
   
    # Fetch the markdown content from the generated URL
    print(f"📥 Fetching markdown from: {markdown_url}")
    markdown_response = requests.get(markdown_url)
    markdown_response.raise_for_status()
   
    return markdown_response.text
```
{% endcode %}

### Ask questions using OpenAI

Now that we have the markdown content, we can ask questions about it using the OpenAI API. The function below takes markdown content and a question as input, then uses the OpenAI API to generate a summary based on the provided content. In this case, we are using the [gpt-3.5-turbo](https://platform.openai.com/docs/models) model, but you can choose any other model.

{% code overflow="wrap" lineNumbers="true" %}
```python
def ask_question(markdown, question):
    client = OpenAI(api_key=OPENAI_API_KEY)

    prompt = (
        "You are an assistant helping analyze different webpages.\n\n"
        f"Markdown content:\n{markdown[:3000]}\n\n"
        f"Question: {question}\nAnswer as clearly as possible."
    )

    response = client.chat.completions.create(
        model="gpt-4o-mini",
        messages=[
            {"role": "user", "content": prompt}
        ]
    )

    return response.choices[0].message.content
```
{% endcode %}

The markdown becomes the model’s context, enabling accurate answers about the original web content.

### User Interaction and Execution

Having defined the functions, we can now create a simple command-line interface that lets users enter a URL and ask questions about its content.

{% code overflow="wrap" lineNumbers="true" %}
```python
def main():
    url = input("Enter the URL of the article: ")
    try:
        markdown = fetch_markdown_with_gaffa(url)
        print("\n✅ Markdown successfully retrieved from Gaffa.\n")

        while True:
            question = input("Ask a question about the content (or type 'exit'): ")
            if question.lower() == "exit":
                break
            answer = ask_question(markdown, question)
            print(f"\n💬 Answer: {answer}\n")

    except Exception as e:
        print(f"⚠️ Error: {e}")

if __name__ == "__main__":
    main()
```
{% endcode %}

### Full Script

The full script is available to download from the [Gaffa Python Examples GitHub repo](https://github.com/GaffaAI/GaffaPythonExamples/blob/main/scripts/WebpageToMarkdown/markdown_generator.py).

### Running the Script

To run the script, simply execute it in your terminal:

```sh
python your_script_name.py
```

With your script running, you can enter any web page URL, and it will fetch the markdown content and let you ask questions about it.
