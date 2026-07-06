---
description: >-
  An example request that uses Gaffa to extract structured data from an online
  PDF.
---

# Parse PDF to Structured JSON

_The following example is a request we've pre-built to show you Gaffa's capabilities against our_ [_demo site_](https://demo.gaffa.dev)_._&#x20;

This example demonstrates how to extract data from PDF documents. Gaffa downloads the PDF and uses AI to intelligently parse the content according to your schema, making it perfect for building research databases, citation managers, or literature review tools.

**This feature currently works for online PDFs.**

## API Request

The request below uses the [POST endpoint](https://gaffa.dev/docs/api-reference/post-v1-browser-requests) to download a demo research paper from the hosted PDFs, wait for it to load, and then parse the first page to extract author information and paper metadata.

```json
{
  "url": "https://demo.gaffa.dev/simulate/pdf/ReasoningAboutActionAndChange.pdf",
  "proxy_location": null,
  "async": false,
  "max_cache_age": 0,
  "settings": {
    "record_request": false,
    "actions": [
      {
        "type": "download_file"
      },
      {
        "type": "parse_json",
        "data_schema": {
          "name": "AcademicPaper",
          "description": "Schema for parsing academic paper summary and author information",
          "fields": [
            {
              "type": "string",
              "name": "title",
              "description": "The full title of the academic paper"
            },
            {
              "type": "string",
              "name": "abstract",
              "description": "The paper's abstract or summary"
            },
            {
              "type": "array",
              "name": "authors",
              "description": "List of authors who contributed to the paper",
              "fields": [
                {
                  "type": "string",
                  "name": "name",
                  "description": "Author's full name as it appears in the paper"
                },
                {
                  "type": "array",
                  "name": "affiliations",
                  "description": "Institutional affiliations for this author",
                  "fields": [
                    {
                      "type": "string",
                      "name": "institution",
                      "description": "Name of the university or research institution"
                    },
                    {
                      "type": "string",
                      "name": "department",
                      "description": "Department or division name"
                    },
                    {
                      "type": "string",
                      "name": "city",
                      "description": "City where the institution is located"
                    },
                    {
                      "type": "string",
                      "name": "country",
                      "description": "Country of the institution"
                    }
                  ]
                },
                {
                  "type": "string",
                  "name": "email",
                  "description": "Author's contact email address if provided"
                }
              ]
            },
            {
              "type": "array",
              "name": "keywords",
              "description": "Key terms and topics covered in the paper",
              "fields": [
                {
                  "type": "string",
                  "name": "keyword",
                  "description": "Individual keyword or phrase"
                }
              ]
            }
          ]
        },
        "instruction": "Parse this academic paper focusing on the title, abstract, author information, and keywords typically found on the first page. Extract all author names, their institutional affiliations with department and location details, and their contact information.",
        "model": "gpt-4o-mini",
        "output_type": "inline",
        "max_pages": 1
      }
    ]
  }
}
```

## Actions

{% content-ref url="../actions/download-file.md" %}
[download-file.md](../actions/download-file.md)
{% endcontent-ref %}

{% content-ref url="../actions/parse-json.md" %}
[parse-json.md](../actions/parse-json.md)
{% endcontent-ref %}

## Response

The parsed data is returned as a structured JSON object matching your schema:

```json
{
    "data": {
        "id": "brq_VYfyVifa26oMpmX4YDeNN3iJDrhK3a",
        "url": "https://demo.gaffa.dev/simulate/pdf/ReasoningAboutActionAndChange.pdf",
        "state": "completed",
        "credit_usage": 0,
        "http_status_code": 200,
        "from_cache": false,
        "started_at": "2025-12-01T06:09:43.6125439Z",
        "completed_at": "2025-12-01T06:09:57.5453161Z",
        "running_time": "00:00:13.9327722",
        "page_load_time": "00:00:00.8959680",
        "actions": [
            {
                "id": "act_VYfyVhGPwQjur9XAu5XA47n2FozYfK",
                "type": "download_file",
                "timestamp": "2025-12-01T06:09:46.509484Z",
                "output": "https://storage.gaffa.dev/brq/downloads/brq_VYfyVifa26oMpmX4YDeNN3iJDrhK3a/ReasoningAboutActionAndChange.pdf"
            },
            {
                "id": "act_VYfyVjNHWzECbraio6xS6MqhYhiDWP",
                "type": "parse_json",
                "timestamp": "2025-12-01T06:09:57.5453056Z",
                "output": {
                    "title": "Reasoning about Action and Change",
                    "abstract": "This chapter presents the state of research concerning the formalisation of an agent reasoning about a dynamic system which can be partially observed and acted upon. We first define the basic concepts of the area: system states, ontic and epistemic actions, observations; then the basic reasoning processes: prediction, progression, regression, postdiction, filtering, abduction, and extrapolation. We then recall the classical action representation problems and show how these problems are solved in some standard frameworks. For space reasons, we focus on these major settings: the situation calculus, STRIPS and some propositional action languages, dynamic logic, and dynamic Bayesian networks. We finally address a special case of progression, namely belief update.",
                    "authors": [
                        {
                            "name": "Florence Dupin de Saint-Cyr",
                            "affiliations": [
                                {
                                    "institution": "IRIT-CNRS. Université Paul Sabatier",
                                    "department": "",
                                    "city": "Toulouse",
                                    "country": "France"
                                }
                            ],
                            "email": ""
                        },
                        {
                            "name": "Andreas Herzig",
                            "affiliations": [
                                {
                                    "institution": "IRIT-CNRS. Université Paul Sabatier",
                                    "department": "",
                                    "city": "Toulouse",
                                    "country": "France"
                                }
                            ],
                            "email": ""
                        },
                        {
                            "name": "Jérôme Lang",
                            "affiliations": [
                                {
                                    "institution": "CNRS, Université Paris-Dauphine, PSL Research University, LAMSADE",
                                    "department": "",
                                    "city": "Paris",
                                    "country": "France"
                                }
                            ],
                            "email": ""
                        },
                        {
                            "name": "Pierre Marquis",
                            "affiliations": [
                                {
                                    "institution": "CRIL-CNRS, Université d’Artois & Institut Universitaire de France",
                                    "department": "",
                                    "city": "Lens",
                                    "country": "France"
                                }
                            ],
                            "email": ""
                        }
                    ],
                    "keywords": []
                },
                "reference": "https://storage.gaffa.dev/brq/downloads/brq_VYfyVifa26oMpmX4YDeNN3iJDrhK3a/ReasoningAboutActionAndChange.pdf"
            }
        ]
    }
}
```
