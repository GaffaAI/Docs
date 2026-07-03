# Parse JSON

{% hint style="info" %}
**Paid Action:** This action consumes credits based on the amount of content parsed. See more [below](parse-json.md#pricing).
{% endhint %}

**Type:** `parse_json`

The `parse_json` action extracts data from web pages and online PDFs. It uses AI to parse web content from text into a pre-defined data schema and return it as a JSON object.

The action lets you convert unstructured content, such as academic papers, forms, and webpages, into JSON objects that you can use in automations, analysis, or further processing.

_This feature currently works for online PDFs and web page text._

### Parameters

<table data-full-width="false"><thead><tr><th width="212">Name</th><th width="130">Type</th><th width="108" data-type="checkbox">Required</th><th>Description</th></tr></thead><tbody><tr><td><code>data_schema_id</code></td><td><code>string</code></td><td>true</td><td>The id of the data schema you have defined that you want to transform the content into.<br><br><strong>You must provide a <code>data_schema</code> or <code>data_schema_id</code> with your request.</strong></td></tr><tr><td><code>data_schema</code></td><td><code>json</code></td><td>true</td><td><p>A JSON object describing the data_schema you want to transform the content into.<br></p><p><strong>You must provide a <code>data_schema</code> or <code>data_schema_id</code> with your request.</strong></p></td></tr><tr><td><code>instruction</code></td><td><code>string</code></td><td>false</td><td>A custom instruction, in addition to any detail you have added to the data schema, that you want to include with this particular parse.</td></tr><tr><td><code>model</code></td><td><code>string</code></td><td>false</td><td>The AI model you wish to use to parse the content into JSON. <br><strong>Default:</strong> <code>gpt-4o-mini</code><br><strong>Accepted</strong>: <code>["gpt-4o-mini"]</code></td></tr><tr><td><code>input_token_cap</code></td><td><code>int</code></td><td>false</td><td>The max number of source input tokens that will be passed to the AI model to parse. This can be used to prevent unnecessary credit usage. If your source input is longer than the token cap, it will be abbreviated.<br><strong>Default:</strong> 1,000,000</td></tr><tr><td><code>selector</code></td><td><code>string</code></td><td>false</td><td>The <a href="https://www.w3schools.com/cssref/css_selectors.php">selector </a>that defines an element you want to parse the content of - this is useful if you are only interested in the contents of a certain element.</td></tr><tr><td><code>output_type</code></td><td><code>string</code></td><td>false</td><td>Should the action output be saved to a file where a URL will be returned or should the parsed  JSON object be included directly in the request.<br><br><strong>Default:</strong> <code>file</code><br><strong>Accepted</strong>: <code>["file", "inline"]</code></td></tr><tr><td><code>max_pages</code></td><td><code>int</code></td><td>false</td><td>If you are parsing a PDF you can specify this parameter to limit the number of pages that are passed to the LLM.<br><br><strong>Default:</strong> no limit</td></tr></tbody></table>

See [universal parameters](./#universal-parameters).

#### Defining Data Schemas

A data schema tells the model exactly what JSON structure to produce.

You can define schemas in two ways:

* **Inline schemas** (defined directly inside the action)
* Reusable schemas (created via the Schema API and referenced by ID in your requests)

### Schema Structure

A schema has:

<table><thead><tr><th width="156">Property</th><th width="138">Type</th><th>Description</th></tr></thead><tbody><tr><td><code>description</code></td><td>string</td><td>Explains what data the schema extracts and provides context to help the AI model understand the extraction goal.<br><strong>Example</strong>: <code>"Extract product details from this e-commerce product page"</code></td></tr><tr><td><code>fields</code></td><td>array</td><td>Each field defines a piece of data to extract from the content. See field properties below.</td></tr><tr><td><code>name</code></td><td>string</td><td>This identifies the schema and should clearly indicate what data it extracts.<br><strong>Example</strong>: <code>"ProductInfo"</code>, <code>"ArticleMetadata"</code>, <code>"ContactForm"</code></td></tr></tbody></table>

Each field in the `fields` array has:

<table><thead><tr><th width="154"></th><th width="143"></th><th></th></tr></thead><tbody><tr><td><code>description</code></td><td>string</td><td><p>Include details about format, handling of missing values, or special cases. </p><p><strong>Example</strong>: <code>"Maximum salary in GBP. If only one value is provided, use the same value for both min and max. Return null if not provided."</code></p></td></tr><tr><td><code>fields</code></td><td>array</td><td>Required only for <code>object</code> and <code>array</code> types.</td></tr><tr><td><code>name</code></td><td>string</td><td>Use clear, descriptive names that follow your preferred naming convention (e.g., <code>snake_case</code> or <code>camelCase</code>). <strong>Example</strong>: <code>"product_name"</code>, <code>"published_date"</code>, <code>"author_email"</code></td></tr><tr><td><code>type</code></td><td>string</td><td>Determines how the AI interprets and structures the extracted data. Must be one of the supported types below.</td></tr></tbody></table>

#### Supported Field Types

| Type     | Description              |
| -------- | ------------------------ |
| array    | List of items            |
| boolean  | True/False               |
| datetime | timestamp                |
| decimal  | Precise decimal          |
| double   | Floating-point number    |
| integer  | Whole number             |
| object   | Nested structured object |
| string   | Text value               |

### Inline Schema Example

```json
"actions": [
  {
    "type": "parse_json",
    "data_schema": {
      "name": "ArticleMetadata",
      "instruction": "Extract metadata from an article",
      "fields": [
        {
          "type": "string",
          "name": "title",
          "description": "Article title"
        },
        {
          "type": "string",
          "name": "author",
          "description": "Author name"
        },
        {
          "type": "datetime",
          "name": "published",
          "description": "Publication date"
        }
      ]
    },
    "model": "gpt-4o-mini",
    "output_type": "inline"
  }
]
```

This example shows:

* **Simple fields** (`string`, `datetime`) for basic data
* **Object fields** for grouped related data with nested `fields`
* **Array fields** for lists of items with nested `fields` defining each item's structure

### Schema Operations

Instead of defining schemas inline each time, you can save them to your Gaffa account and reuse them across multiple requests. This makes your actions more readable, easier to maintain, and ensures consistency when parsing similar content.

#### Creating a Saved Schema

Use the [POST /v1/schemas](https://gaffa.dev/docs/api-reference/post-v1-schemas) endpoint to create a reusable schema:

```bash
curl -L \
  --request POST \
  --url 'https://api.gaffa.dev/v1/schemas' \
  --header 'X-API-Key: YOUR_API_KEY' \
  --header 'Content-Type: application/json' \
  --data '{
    "name": "ProductInfo",
    "instruction": "Extract product details from e-commerce pages",
    "fields": [
      {
        "type": "string",
        "name": "product_name",
        "description": "The product title"
      },
      {
        "type": "decimal",
        "name": "price",
        "description": "Current price"
      },
      {
        "type": "boolean",
        "name": "in_stock",
        "description": "Product availability"
      },
      {
        "type": "object",
        "name": "ratings",
        "description": "Product rating information",
        "fields": [
          {
            "type": "double",
            "name": "average",
            "description": "Average rating score"
          },
          {
            "type": "integer",
            "name": "total_reviews",
            "description": "Number of reviews"
          }
        ]
      },
      {
        "type": "array",
        "name": "tags",
        "description": "Product tags",
        "fields": [
          {
            "type": "string",
            "name": "tag",
            "description": "Individual tag name"
          }
        ]
      }
    ]
  }'
```

**Response:**

```json
{
  "id": "schema_abc123xyz",
  "name": "ProductInfo",
  "description": "Extract product details from e-commerce pages",
  "fields": [...]
}
```

Save the `id` returned in the response, you'll use this to reference the schema in your requests

### Managing Schemas

#### **List all schemas**:

Allows you to view all schemas saved to your account:

Endpoint: [GET /v1/schemas](https://gaffa.dev/docs/api-reference/get-v1-schemas)

```bash
curl -L \
  --url 'https://api.gaffa.dev/v1/schemas' \
  --header 'X-API-Key: YOUR_API_KEY' \
  --header 'Accept: */*'
```

#### **Update a schema**:

Allows you to modify an existing schema by its ID:

Endpoint: [PUT /v1/schemas](https://gaffa.dev/docs/api-reference/put-v1-schemas)

```bash
curl -L \
  --request PUT \
  --url 'https://api.gaffa.dev/v1/schemas/{id}' \
  --header 'X-API-Key: YOUR_API_KEY' \
  --header 'Content-Type: application/json' \
  --data '{
    "id": "schema_abc123xyz",
    "name": "ProductInfo",
    "instruction": "Extract detailed product information from e-commerce pages",
    "fields": [
      {
        "type": "string",
        "name": "product_name",
        "description": "The product title"
      },
      {
        "type": "decimal",
        "name": "price",
        "description": "Current price"
      },
      {
        "type": "string",
        "name": "brand",
        "description": "Product brand name"
      }
    ]
  }'
```

#### **Delete a schema**:

Removes a schema from your account:

Endpoint: [DELETE /v1/schemas/:id](https://gaffa.dev/docs/api-reference/delete-v1-schemas-id)

```bash
curl -L \
  --request DELETE \
  --url 'https://api.gaffa.dev/v1/schemas/{id}' \
  --header 'X-API-Key: YOUR_API_KEY' \
  --header 'Accept: */*'
```

### Common Schema Patterns

Simple List Extraction

```json
{
  "name": "TagList",
  "instruction": "Extract article tags",
  "fields": [
    {
      "type": "array",
      "name": "tags",
      "description": "List of article tags",
      "fields": [
        {
          "type": "string",
          "name": "tag",
          "description": "Individual tag name"
        }
      ]
    }
  ]
}
```

**Nested Objects**

```json
{
  "name": "ProductWithReviews",
  "instruction": "Product details with nested review data",
  "fields": [
    {
      "type": "string",
      "name": "product_name",
      "description": "Product name"
    },
    {
      "type": "object",
      "name": "pricing",
      "description": "Pricing information",
      "fields": [
        {
          "type": "decimal",
          "name": "current_price",
          "description": "Current price"
        },
        {
          "type": "decimal",
          "name": "original_price",
          "description": "Original price before discount"
        },
        {
          "type": "integer",
          "name": "discount_percentage",
          "description": "Discount percentage"
        }
      ]
    }
  ]
}
```

### Pricing

The credits this action uses depend on the model used. Here are the current supported models and their pricing:

| Model         | Input Token Cost                 | Output Token Cost                  |
| ------------- | -------------------------------- | ---------------------------------- |
| `gpt-4o-mini` | 1 credit per 20,000 input tokens | 1 credits per 10,000 output tokens |
