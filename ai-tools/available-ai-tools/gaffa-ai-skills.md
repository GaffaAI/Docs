---
description: Set up your account and ship your first project in minutes.
---

# Gaffa AI Skills

The Gaffa AI skills teach your AI coding tool (Cursor, Claude, Codex, Antigravity, etc.) everything it needs to build on the Gaffa API. Once installed, your agent can write and review code that calls Gaffa, use Gaffa to find information on a live website, diagnose a failing request, and put together a support report, all without you pasting API details into the chat.

There are four skills, and they work in Claude Code, Codex, GitHub Copilot, Cursor, Antigravity, and Pi. The plugin install also adds the Gaffa docs [MCP server](gaffa-mcp-server.md), so your agent can look things up in these docs as it works.\
\
The Gaffa AI skills teach your AI coding tool everything it needs to build with the Gaffa API.

**Once installed, your agent can:**

* write and review code that calls Gaffa
* use Gaffa to find information on a live website
* diagnose a failing Gaffa request
* prepare a support report when something goes wrong

You do not need to paste API details or Gaffa documentation into every chat.

Gaffa includes four skills:

1. `gaffa-authoring` — Writes, edits, ports, and reviews code that calls the Gaffa API. It runs automatically when you ask for Gaffa code and checks the live docs before it writes anything.
2. `gaffa-find` — Finds one piece of information on a website from a URL and a plain-language goal. It tries, checks the result, and refines until it gets it.
3. `gaffa-debug` — Works out why a request failed from its recording or `brq_` ID, then suggests the smallest useful fix.
4. `gaffa-support` — When you're stuck, it tries to solve the problem first, then packages a redacted report you can email to support.

They work with Claude Code, Codex, Cursor, Antigravity, GitHub Copilot, and Pi.

When you install Gaffa as a plugin, you also get the [Gaffa docs MCP server](/broken/pages/SwNfUh4YIdklcr2rg0p5), which lets your agent look things up in the Gaffa documentation while it works.

## Quick start

If you use Claude Code, run these two commands in your terminal:

```shellscript
claude plugin marketplace add https://github.com/GaffaAI/gaffa-for-ai
claude plugin install gaffa@gaffa --scope project
```

Then [set your API key](gaffa-ai-skills.md#before-you-start) and ask your agent to do something with Gaffa.

{% hint style="info" %}
Use the full URL in the first command. Claude Code clones the `owner/repo` shorthand over SSH by default, which fails if you don't have a GitHub SSH key.
{% endhint %}

Using another tool? Jump to [Set up your tool](gaffa-ai-skills.md#set-up-your-tool).

## Before you start

Two things apply to every tool.

**1. Set your API key**

The skills read your key from the `GAFFA_API_KEY` environment variable and nowhere else. They never ask you for it, write it to disk, or log it. Set it once in your shell profile so you don't have to paste it each session:

```shellscript
export GAFFA_API_KEY='your_gaffa_api_key_here'
```

You can create a key from your [dashboard](https://gaffa.dev/dashboard/api-keys). Keep it out of version control.

**2. Allow Gaffa through your network**

The skills call `api.gaffa.dev` and read docs from `gaffa.dev`. If your tool runs in a sandbox, or you're behind a proxy that blocks outbound traffic, allow `gaffa.dev` and `*.gaffa.dev` first, or requests will fail with a connectivity error.

{% hint style="warning" %}
The skills use your Gaffa credits when they run requests. `gaffa-find` has a cost cap you can tune, and `gaffa-debug` only spends credits on an optional re-run, after asking you first. `gaffa-support` doesn't use any credits.
{% endhint %}

## Set up your tool

{% tabs %}
{% tab title="Claude Code" %}
Install the plugin with the two commands from the [Quick Start](gaffa-ai-skills.md#quick-start). You can choose where it's installed with `--scope`:

* `user`: for you, in every project. This is the default.
* `project`: shared with everyone who works on the project. Writes to `.claude/settings.json`.
* `local`: only for you, in this project. Writes to `.claude/settings.local.json` and isn't shared.

Start a new Claude Code session afterwards so it loads the skills.
{% endtab %}

{% tab title="Codex" %}
The command below adds the Gaffa marketplace, then install the plugin from it:

```bash
codex plugin marketplace add https://github.com/GaffaAI/gaffa-for-ai
codex plugin add gaffa@gaffa
```

Start a new Codex session afterwards so it loads the skills. In Codex, you start a skill with `$`, for example `$gaffa-find`, or pick it from the skills menu.
{% endtab %}

{% tab title="Cursor" %}
Cursor plugins work across Cursor desktop, web, and the Cursor CLI, so you only need to install Gaffa once.

You need Cursor 2.4 or later.

#### Install in Cursor

1. Open Cursor Settings and search for Plugins.
2. Click **+ Add** and select **From a GitHub Repository**.
3. Paste `https://github.com/GaffaAI/gaffa-for-ai`.
4. Choose a scope: **User** to install it for yourself, or **Team** to share it with your team.
5. Click **Import**.
6. Once the import finishes, click **Add**.

Reload Cursor afterwards if the plugin does not appear immediately.

#### Using Cursor CLI

If you use the Cursor CLI, you can install with one command:

```shellscript
cursor-agent plugin marketplace add https://github.com/GaffaAI/gaffa-for-ai
```

The same installed plugin is available across Cursor's agent surfaces, including Cursor CLI. You do not need a separate Gaffa installation for the CLI.
{% endtab %}

{% tab title="Antigravity" %}
Gaffa can be used with Antigravity 2.0, Antigravity CLI, and the Antigravity IDE.

#### Antigravity 2.0 or IDE

For a global installation:

```bash
git clone --depth 1 https://github.com/GaffaAI/gaffa-for-ai ~/.gemini/config/plugins/gaffa
```

This makes Gaffa available across your workspaces.

For one project only:

```bash
git clone --depth 1 https://github.com/GaffaAI/gaffa-for-ai .agents/plugins/gaffa
```

If you want to commit the project-level plugin for your team, remove the clone's Git history first:

```bash
rm -rf .agents/plugins/gaffa/.git
```

Otherwise, Git may treat the folder as another repository instead of storing its files normally.

Restart or reload Antigravity afterwards.

#### Antigravity CLI

If you use Antigravity CLI, you can install the plugin directly from GitHub:

```shellscript
agy plugin install https://github.com/GaffaAI/gaffa-for-ai
```

Restart your Antigravity CLI session afterwards.
{% endtab %}

{% tab title="GitHub Copilot" %}
#### Using GitHub Copilot CLI

Add the Gaffa marketplace and install the plugin:

```bash
copilot plugin marketplace add GaffaAI/gaffa-for-ai
copilot plugin install gaffa@gaffa
```

Restart your Copilot session afterwards so it loads the skills.

#### Using Copilot in VS Code, GitHub Copilot app, code review, or cloud agent

Copy the Gaffa skills into a folder Copilot reads.

From your project folder:

```shellscript
git clone --depth 1 https://github.com/GaffaAI/gaffa-for-ai /tmp/gaffa-for-ai
mkdir -p .agents/skills
cp -r /tmp/gaffa-for-ai/skills/* .agents/skills/
```

To make the skills available to you across projects:

```shellscript
mkdir -p ~/.agents/skills
cp -r /tmp/gaffa-for-ai/skills/* ~/.agents/skills/
```

Restart or reload Copilot afterwards so it picks up the skills.

This installs the skills but not the Gaffa docs MCP server. See[ Install by copying](gaffa-ai-skills.md#install-by-copying) to add the MCP server manually.
{% endtab %}

{% tab title="Pi" %}
Install Gaffa skills directly from GitHub:

```shellscript
pi install https://github.com/GaffaAI/gaffa-for-ai
```

Restart Pi afterwards so it loads the Gaffa skills.
{% endtab %}
{% endtabs %}

## Install by copying

Every skill is a self-contained folder that follows the open Agent Skills format (a `SKILL.md` file with a name and description). That means you can copy the skills you want into the folder your tool reads. The skills don't depend on each other, so copy only the ones you need.

Get the folders from the `skills/` directory of the [gaffa-for-ai repository](https://github.com/GaffaAI/gaffa-for-ai), then copy them into the right place for your tool:

<table><thead><tr><th width="179.0206298828125">Tool</th><th width="243.44805908203125">Project folder</th><th>Personal folder</th></tr></thead><tbody><tr><td>Claude Code</td><td><code>.claude/skills/</code></td><td><code>~/.claude/skills/</code></td></tr><tr><td>Cursor (2.4+)</td><td><code>.agents/skills/</code> or <code>.cursor/skills/</code></td><td><code>~/.agents/skills/</code> or <code>~/.cursor/skills/</code></td></tr><tr><td>Codex CLI</td><td><code>.agents/skills/</code></td><td><code>~/.agents/skills/</code></td></tr><tr><td>GitHub Copilot</td><td><code>.agents/skills/</code></td><td><code>~/.agents/skills/</code></td></tr><tr><td>Pi</td><td><code>.pi/skills/</code></td><td><code>~/.pi/agent/skills/</code></td></tr><tr><td>Antigravity</td><td><code>.agents/skills/</code></td><td>Expand the section below for your version</td></tr></tbody></table>

<details>

<summary><strong>Antigravity personal folder by version</strong></summary>

The personal folder for Antigravity depends on which version you use, so pick the one that matches your setup.

* Antigravity 2.0 uses `~/.gemini/config/skills/`
* The Antigravity IDE uses `~/.gemini/antigravity/skills/`
* The Antigravity CLI uses `~/.gemini/antigravity-cli/skills/`

The project folder is the same for all three, so use that if you are unsure.

</details>

For example, to install `gaffa-authoring` into a Claude Code project:

```shellscript
cp -r skills/gaffa-authoring .claude/skills/gaffa-authoring
```

Restart or reload your tool afterwards so it picks up the new skills.

{% hint style="warning" %}
Copying the folders installs the skills but not the docs MCP server, which the plugin install adds for you. To add it yourself, point your tool at `https://gaffa.dev/docs/~gitbook/mcp`. It needs no authentication. Without it, the skills still work by fetching these docs over plain HTTP, just more slowly.
{% endhint %}

## Update the skills

How you update Gaffa depends on how you installed it.

{% tabs %}
{% tab title="Claude Code" %}
Update the Gaffa marketplace:

```shellscript
claude plugin marketplace update gaffa
```

Then update the Gaffa plugin:

```shellscript
claude plugin update gaffa@gaffa
```

Start a new Claude Code session afterwards.
{% endtab %}

{% tab title="Codex" %}
Refresh the Gaffa marketplace:

```bash
codex plugin marketplace upgrade gaffa
```

Start a new Codex session afterwards.
{% endtab %}

{% tab title="Cursor" %}
In Cursor, open **Settings → Plugins**, find **Gaffa**, and install the available update.

**Or update it from the CLI:**

```shellscript
cursor-agent plugin marketplace update gaffa
```

Reload Cursor afterwards if needed.

Because Cursor plugins work across the editor and Cursor CLI, you do not need to update a separate CLI copy.
{% endtab %}

{% tab title="GitHub Copilot" %}
Update Gaffa:

```shellscript
copilot plugin update gaffa
```

Restart your Copilot session afterwards.

#### Other Copilot surfaces

Replace the copied skill folders with the latest versions from the Gaffa repository.
{% endtab %}

{% tab title="Antigravitty" %}
For a global installation:

```shellscript
cd ~/.gemini/config/plugins/gaffa
git pull
```

For a project installation:

```shellscript
cd .agents/plugins/gaffa
git pull
```

Restart or reload Antigravity afterwards.

For Antigravity CLI:

```shellscript
agy plugin uninstall gaffa
agy plugin install https://github.com/GaffaAI/gaffa-for-ai
```
{% endtab %}

{% tab title="Pi" %}
Update installed Pi packages:

```shellscript
pi update --extensions
```

Restart Pi afterwards.
{% endtab %}
{% endtabs %}

## Example prompts

Try these once you're set up.

**Write code for a Gaffa request** (`gaffa-authoring`)

> Write a script that gets the price from this product page: https://example.com/product

The agent checks the Gaffa docs, works out the right selector from the real page, and writes a script that sends a browser request and returns the price.

**Find one piece of information** (`gaffa-find`)

> /gaffa-find Find the opening hours on https://example.com

The agent loads the page with Gaffa, works out where the opening hours are, extracts them, and checks the result before it stops.

**Work out why a request failed** (`gaffa-debug`)

> /gaffa-debug Why did brq\_V2P6PqrZpycFtbc7mtXE4tsNbeg2N6 fail?

The agent looks up the request and its recording, sorts the failure (a wrong action, a page that changed, bot detection, timing), and proposes a minimal fix. If the request wasn't recorded, it tells you and offers to re-run it with recording on after asking you first.

**Ask for help** (`gaffa-support`)

> /gaffa-support My request keeps timing out and I can't work out why

The agent tries to solve the problem first. If it can't, it makes a local report with your secrets removed, which you can email to [support@gaffa.dev](mailto:support@gaffa.dev).

### Troubleshooting

**"Authentication" error when a skill sends its first request.** Your API key is missing or wrong. Check `GAFFA_API_KEY` is set in the same terminal or environment your tool runs in, then restart the tool so it picks the value up.

**Connectivity error, or the agent says the docs are unavailable.** Your environment is blocking `gaffa.dev`. Allow `gaffa.dev` and `*.gaffa.dev` in your sandbox or proxy settings.

**A skill doesn't show up after installing.** Restart or reload your tool. Skills are picked up when the tool starts.

**Still stuck?** Run `/gaffa-support`, or [contat Gaffa](https://gaffa.dev/contact).
