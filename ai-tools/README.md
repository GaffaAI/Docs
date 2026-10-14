---
description: Use Gaffa with your favourite AI coding tools.
layout:
  width: default
  title:
    visible: true
  description:
    visible: true
  tableOfContents:
    visible: true
  outline:
    visible: true
  pagination:
    visible: true
  metadata:
    visible: true
  tags:
    visible: true
  actions:
    visible: true
  anchors:
    visible: true
---

# Gaffa AI Tools

AI assistants write much better code when they have accurate, up-to-date context about the API they're using. Gaffa gives your AI tools that context, so you can ask for working Gaffa code without pasting documentation into every chat.

### What's available

There are three ways to connect Gaffa to your AI tools:

* [**AI skills**](available-ai-tools/gaffa-ai-skills.md) - teach your coding agent to write and review Gaffa code, find information on a live website, debug a failing request, and prepare a support report. They work in Claude Code, Codex, Cursor, GitHub Copilot, Antigravity, and Pi, and the plugin install sets up the MCP server for you as well.
* [**MCP server**](available-ai-tools/gaffa-mcp-server.md) - gives any tool that supports MCP live access to the Gaffa docs. Your assistant searches and reads only the pages it needs, and always gets the latest version. It needs no API key, and it can't access your account or spend credits.
* [**llms.txt**](https://gaffa.dev/docs/llms-full.txt) - a single file with a concise overview of the Gaffa API that you can paste into any AI assistant, such as ChatGPT or Claude. It's a snapshot of the docs, so use it when your tool doesn't support MCP.&#x20;

### Where to start

* **Building with Gaffa in a coding agent?** Install the AI skills.
* **Want your assistant to look things up in the docs?** Connect the MCP server.
* **Using a chat assistant without MCP?** Use llms.txt.

