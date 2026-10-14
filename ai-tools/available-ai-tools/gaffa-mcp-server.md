---
description: >-
  Connect your AI assistant or coding agent to the Gaffa docs MCP server so it
  can search and read the Gaffa documentation while it writes code for you.
---

# Gaffa MCP Server

Gaffa provides a [Model Context Protocol (MCP)](https://modelcontextprotocol.io) server that gives AI tools like Claude, Cursor, VS Code, Codex, etc live access to the Gaffa documentation. Once it's connected, your assistant can look up actions, parameters, and examples on its own, instead of relying on whatever it remembers or on context you paste in.

The server URL is:

```shellscript
https://gaffa.dev/docs/~gitbook/mcp
```

It doesn't need an API key or any other authentication.

{% hint style="info" %}
The MCP server gives your assistant access to the **documentation**, not to your Gaffa account. It can't send browser requests or spend credits. When your assistant writes code that calls the Gaffa API, that code still uses your own API key.
{% endhint %}

## MCP server or llms.txt?

Both give an AI assistant accurate context about Gaffa. The difference is how that context gets there.

* [**llms.txt**](https://gaffa.dev/docs/llms-full.txt) is a single file you paste into a chat. It works with any assistant, but it's a snapshot of the docs and uses up space in the conversation.
* **The MCP server** lets the assistant search and fetch only the pages it needs, when it needs them, and always returns the latest version of the docs.

If your tool supports MCP, use the MCP server. If it doesn't, use llms.txt.

## Available tools

The server exposes four tools. Your assistant decides when to use them, so you don't need to call them yourself.

<table><thead><tr><th width="213.289794921875">Tool</th><th>What it does</th></tr></thead><tbody><tr><td><code>searchDocumentation</code></td><td>Searches the Gaffa docs and returns matching sections with links to the full pages.</td></tr><tr><td><code>getPage</code></td><td>Fetches the full content of a docs page by URL.</td></tr><tr><td><code>askQuestion</code></td><td>Answers a plain-language question about Gaffa, with links to the source pages.</td></tr><tr><td><code>sendFeedback</code></td><td>Reports a problem in the docs (outdated, contradictory or missing information) to the Gaffa team so we can fix it.</td></tr></tbody></table>

## Install the MCP server

Pick your tool below. In every case, you are adding a remote (HTTP) MCP server with the URL above. We name it `gaffa-docs` in these examples, but you can use any name.

{% tabs %}
{% tab title="Claude Code" %}
Run this command in your terminal:

```bash
claude mcp add --transport http gaffa-docs https://gaffa.dev/docs/~gitbook/mcp
```

This adds the server for your current project only. Add `--scope user` to make it available in all your projects, or `--scope project` to save it in a `.mcp.json` file that you can commit and share with your team.

Run `/mcp` inside Claude Code to check that `gaffa-docs` is connected.
{% endtab %}

{% tab title="Claude Desktop / Claude.ai" %}
1. Open **Settings** and go to **Connectors**.
2. Click **Add custom connector**.
3. Enter `Gaffa Docs` as the name and `https://gaffa.dev/docs/~gitbook/mcp` as the URL, then click **Add**.

Custom connectors added on Claude.ai are also available in the Claude Desktop app.
{% endtab %}

{% tab title="Cursor" %}
Add the server to `.cursor/mcp.json` in your project, or to `~/.cursor/mcp.json` to use it in every project:

```json
{
  "mcpServers": {
    "gaffa-docs": {
      "url": "https://gaffa.dev/docs/~gitbook/mcp"
    }
  }
}
```

Then open **Cursor Settings → MCP** and check that `gaffa-docs` shows as enabled.
{% endtab %}

{% tab title="VS Code" %}
Add the server to `.vscode/mcp.json` in your project:

```json
{
  "servers": {
    "gaffa-docs": {
      "type": "http",
      "url": "https://gaffa.dev/docs/~gitbook/mcp"
    }
  }
}
```

Open the file and click **Start** above the server entry. The Gaffa tools are then available to GitHub Copilot in **Agent** mode.
{% endtab %}

{% tab title="Codex" %}
Add the server to `~/.codex/config.toml`:

```toml
[mcp_servers.gaffa-docs]
url = "https://gaffa.dev/docs/~gitbook/mcp"
```

Restart Codex, then run `/mcp` to check that `gaffa-docs` is listed.
{% endtab %}

{% tab title="Windsurf / Antigravity" %}
Windsurf and Antigravity use `serverUrl` instead of `url`. Add this to your MCP config file (in Windsurf, `~/.codeium/windsurf/mcp_config.json`):

```json
{
  "mcpServers": {
    "gaffa-docs": {
      "serverUrl": "https://gaffa.dev/docs/~gitbook/mcp"
    }
  }
}
```

Reload the MCP servers from the settings panel so the change takes effect.
{% endtab %}

{% tab title="Other tools" %}
Any tool that supports remote MCP servers over HTTP can connect. Add a new server in your tool's MCP settings and use:

* **URL:** `https://gaffa.dev/docs/~gitbook/mcp`
* **Transport:** HTTP (sometimes called "Streamable HTTP")
* **Authentication:** none
{% endtab %}
{% endtabs %}

## Install with the Gaffa plugin

If you use Claude Code, Codex, Cursor, Antigravity, GitHub Copilot, or Pi, you can install the [Gaffa plugin](/broken/pages/fAOBlTMBNWAj9PM61KbB#quick-start) instead. It registers this MCP server for you and also adds skills that help your agent write, debug, and test Gaffa code.&#x20;

## Using the MCP server

Once it's connected, ask your assistant about Gaffa as you normally would. It will look things up in the docs when it needs to. For example:

> _"Using the Gaffa docs, write a Python script that opens a page, waits for the table to load and prints it to PDF."_

> _"What parameters does Gaffa's `scroll` action accept?"_

> _"How long does Gaffa keep the output files from my browser requests?"_

Mentioning "Gaffa docs" in your prompt makes it more likely that the assistant uses the MCP server instead of answering from memory. Most tools also show when a tool call is made, so you can see which pages it read.

## Troubleshooting

* **The server doesn't connect.** Check that your tool can reach `gaffa.dev`. If you're behind a proxy, firewall, or sandbox that blocks outbound traffic, allow `gaffa.dev` (and `api.gaffa.dev` if your code will call the API).
* **The assistant isn't using the docs.** Check that the server shows as connected in your tool's MCP settings, then ask it directly to "search the Gaffa docs".
* **Your tool only supports local (stdio) servers.** Use llms.txt instead, or connect through a local bridge such as [`mcp-remote`](https://www.npmjs.com/package/mcp-remote).
