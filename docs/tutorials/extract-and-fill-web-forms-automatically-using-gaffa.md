# Extract and Fill Web Forms Automatically Using Gaffa

Web forms are some of the most common and repetitive elements that users often interact with as developers. Whether you are collecting data, testing user flows, or even building other automation systems.

In this guide, you'll learn how to use `pase_json` action to extract the structure of a web form and then automatically fill and submit it using Gaffa's browser automation features.

By the end of this guide, you will be able to:

* Extract structured form data (labels, input names, required fields, and placeholders) using `parse_json`
* Define and use schemas to reliably understand page structure
* Build a simple interactive CLI that collects user input
* Automatically fill and submit a web form using Gaffa browser actions

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

4. Get your [Gaffa API](https://gaffa.dev/dashboard/api-keys) key and store it as an environment variable:

```sh
GAFFA_API_KEY=your_gaffa_api_key
```

5. Install the required library

```sh
pip install requests
```

### What You'll Build

In this tutorial, you'll create a Python script that:

* **Extracts form fields** - Uses Parse JSON to analyze any web form and identify all input fields.
* **Collects user input** - Prompts the user in the terminal to provide values for each field.
* **Submits the form** - Automatically fills and submits the form using Gaffa's browser automation.

By the end, you'll have a working form automation tool that can be adapted for countless use cases.

### Set Up Your Environment

Create a new directory and Python file.

```sh
mkdir gaffa-form-filler
cd gaffa-form-filler
```

Create a file called `form_filler.py` (_or any name that works for you_) and add your configuration.

```python
import requests
import json

# Configuration
GAFFA_API_KEY = "your_api_key_here"  # Replace with your actual API key
GAFFA_API_URL = "https://api.gaffa.dev/v1/browser/requests"

# The demo form we'll work with
FORM_URL = "https://demo.gaffa.dev/simulate/form?loadTime=3&showModal=true&modalDelay=5&formType=address"
```

Replace `your_api_key_here` with your actual Gaffa API key from the [Dashboard](https://gaffa.dev/dashboard).

### Extract Form Fields Using `parse_json`

In the code below, you define a function that takes a form URL as input and makes a POST request to the Gaffa API.

The request uses two actions: first, a `wait` action ensures the form element is fully loaded on the page, then the `parse_json` action that uses AI to intelligently analyze the form structure and extract all input fields along with their properties (labels, names, types, placeholders, and required status). The AI understands the context of the form and returns structured JSON data that we can easily work with.

```python
def extract_form_fields(form_url):
    payload = {
        "url": form_url,
        "async": False,
        "settings": {
            "record_request": False,
            "actions": [
                {
                    "type": "wait", 
                    "selector": "form", 
                    "timeout": 10000
                },
                {
                    "type": "parse_json",
                    "data_schema": {
                        "name": "FormFields",
                        "description": "Extract all form input fields",
                        "fields": [
                            {"type": "string", "name": "form_title", "description": "Form title"},
                            {
                                "type": "array",
                                "name": "fields",
                                "description": "List of all input fields",
                                "fields": [
                                    {"type": "string", "name": "label", "description": "Field label"},
                                    {"type": "string", "name": "field_name", "description": "Field name attribute"},
                                    {"type": "string", "name": "field_type", "description": "Input type"},
                                    {"type": "boolean", "name": "required", "description": "Is required?"},
                                    {"type": "string", "name": "placeholder", "description": "Placeholder text"}
                                ]
                            }
                        ]
                    },
                    "instruction": "Extract all form fields with their properties",
                    "model": "gpt-4o-mini",
                    "output_type": "inline"
                }
            ]
        }
    }
    
    headers = {"X-API-Key": GAFFA_API_KEY, "Content-Type": "application/json"}
    response = requests.post(GAFFA_API_URL, json=payload, headers=headers)
    response.raise_for_status()
    result = response.json()
    
    for action in result["data"]["actions"]:
        if action.get("type") == "parse_json":
            return action["output"]
    
    return None
```

### Collect User Input

Next, you need to define a function that takes the extracted form data and interacts with the user in the terminal. The function will display the form title and then loop through each field, prompting the user to fill in the value.

For each field in the form, a label and a required marker, if applicable, are shown. The function ensures that the required fields are not left empty and allows users to skip optional fields by pressing enter. All the user's input is collected into a dictionary where the keys are the field names and the values are what the user entered.

```python
def collect_user_input(form_data):
    print(f"\n{'='*60}")
    print(f"📋 Form: {form_data.get('form_title', 'Unknown Form')}")
    print(f"{'='*60}\n")
    
    user_values = {}
    fields = form_data.get("fields", [])
    
    if not fields:
        print("⚠️  No fields found in the form")
        return user_values
    
    print(f"Please provide values for {len(fields)} field(s):\n")
    
    for i, field in enumerate(fields, 1):
        label = field.get("label", "Unknown Field")
        field_name = field.get("field_name", "")
        required = field.get("required", False)
        placeholder = field.get("placeholder", "")
        
        required_marker = " *" if required else ""
        placeholder_hint = f" (e.g., {placeholder})" if placeholder else ""
        prompt = f"[{i}/{len(fields)}] {label}{required_marker}{placeholder_hint}: "
        
        while True:
            value = input(prompt).strip()
            
            if required and not value:
                print("  ⚠️  This field is required. Please provide a value.")
                continue
            
            if not value and not required:
                print("  ℹ️  Skipping optional field")
                break
            
            user_values[field_name] = value
            break
    
    return user_values
```

### Fill and Submit the Form

You need a function that will take the form URL and the user's input values, then submit the form to Gaffa's browser automation. The function will build a list of actions.

First, it waits for the form to be ready, then creates a `type` action for each field to enter the user's value into the corresponding input element using CSS selectors. Lastly, it adds a `click` action to submit the form and a `capture_screenshot` action to take a full-screen image of the results.

The function makes a POST request with all these actions and returns the response, which includes the screenshot URL if successful.

```python
def fill_form(form_url, field_values):
    if not field_values:
        return None
    
    actions = [
        {
            "type": "wait", 
            "selector": "form", 
            "timeout": 10000
        }
    ]
    
    for field_name, value in field_values.items():
        if value:
            actions.append({
                "type": "type",
                "selector": f"[name='{field_name}']",
                "text": value
            })
    
    actions.extend([
        {"type": "click", "selector": "button[type='submit']"},
        {"type": "capture_screenshot", "size": "fullscreen"}
    ])
    
    payload = {
        "url": form_url,
        "async": False,
        "settings": {
            "record_request": False,
            "actions": actions
        }
    }
    
    headers = {"X-API-Key": GAFFA_API_KEY, "Content-Type": "application/json"}
    response = requests.post(GAFFA_API_URL, json=payload, headers=headers)
    response.raise_for_status()
    
    return response.json()
```

### User Interaction and Execution

Having defined the functions, we can now create a simple command-line interface that allows users to interact with the form.

```python
def main():
    print("\n" + "="*60)
    print("🤖 Gaffa Form Filler")
    print("="*60)
    print("This tool extracts form fields and helps you fill them out.\n")
    
    print("📋 Step 1: Analyzing form...")
    form_data = extract_form_fields(FORM_URL)
    
    if not form_data:
        print("\n❌ Could not extract form fields")
        return
    
    print(f"✅ Found {len(form_data.get('fields', []))} field(s)\n")
    
    print("📝 Step 2: Collecting your input...")
    user_values = collect_user_input(form_data)
    
    if not user_values:
        print("\n⚠️  No values provided. Exiting.")
        return
    
    print(f"\n{'='*60}")
    print("📊 Summary of values to submit:")
    print(f"{'='*60}")
    for field_name, value in user_values.items():
        print(f"  {field_name}: {value}")
    print(f"{'='*60}\n")
    
    confirm = input("Submit this form? (y/n): ").strip().lower()
    if confirm != 'y':
        print("\n❌ Submission cancelled")
        return
    
    print("\n🚀 Step 3: Submitting form...")
    result = fill_form(FORM_URL, user_values)
    
    if not result:
        print("❌ Form submission failed")
        return
    
    print("\n✅ Form submitted successfully!")
    
    if "data" in result and "actions" in result["data"]:
        for action in result["data"]["actions"]:
            if action.get("type") == "capture_screenshot" and "output" in action:
                print(f"📸 Screenshot: {action['output']}")
    
    print("\n🎉 All done!\n")

if __name__ == "__main__":
    main()
```

### Full Script

The full script is available to download from the [Gaffa Python Examples GitHub repo](https://github.com/GaffaAI/GaffaPythonExamples/blob/main/scripts/AutomatedFormFilling/automated_form_filling.py).

### Running the Script

To run the script, simply execute it in your terminal:

```sh
python your_script_name.py
```

### Example output:

```sh
============================================================
🤖 Gaffa Form Filler
============================================================
This tool extracts form fields and helps you fill them out.

📋 Step 1: Analyzing form...
✅ Found 9 field(s)

📝 Step 2: Collecting your input...

============================================================
📋 Form: Form Submission Test
============================================================

Please provide values for 9 field(s):

[1/9] First Name *: John
[2/9] Last Name *: Smith
[3/9] Email *: john@example.com
...

============================================================
📊 Summary of values to submit:
============================================================
  first_name: John
  last_name: Smith
  email: john@example.com
...

Submit this form? (y/n): y

🚀 Step 3: Submitting form...

✅ Form submitted successfully!

🎉 All done!
```
