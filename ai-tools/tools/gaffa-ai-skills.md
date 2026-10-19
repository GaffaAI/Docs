---
description: Set up your account and ship your first project in minutes.
---

# Gaffa AI Skills

The Gaffa AI skills teach your AI coding tool everything it needs to build with the Gaffa API.

Once installed, your agent can:

* write and review code that calls Gaffa
* use Gaffa to find information on a live website
* diagnose a failing Gaffa request
* prepare a support report when something goes wrong

You do not need to paste API details or Gaffa documentation into every chat.

Gaffa includes four skills:

* **`gaffa-authoring`** — Writes, edits, ports, and reviews code that calls the Gaffa API. It runs automatically when you ask for Gaffa code and checks the live docs before it writes anything.
* **`gaffa-find`** — Finds one piece of information on a website from a URL and a plain-language goal. It tries, checks the result, and refines until it gets it.
* **`gaffa-debug`** — Works out why a request failed from its recording or `brq_` ID, then suggests the smallest useful fix.
* **`gaffa-support`** — When you're stuck, it tries to solve the problem first, then packages a redacted report you can email to support.

They work with Claude Code, Codex, GitHub Copilot, Cursor, Antigravity, and Pi.

When you install the skills with the [Gaffa CLI](gaffa-cli.md) or as a [plugin](gaffa-ai-skills.md#install-as-a-plugin), you also get the [Gaffa docs MCP server](gaffa-docs-mcp-server.md), which lets your agent look things up in the Gaffa documentation while it works.

## Quick start

{% stepper %}
{% step %}
### Install with the Gaffa CLI (recommended)

Run this in your project folder:

```bash
npx @gaffa-dev/cli install
```

Pick your tools and a scope when it asks. The CLI installs the skills and the Gaffa docs MCP server for Claude Code, Codex, GitHub Copilot, Cursor, Antigravity, and Pi. See Gaffa CLI for every option.
{% endstep %}

{% step %}
### Set your API key

The skills read your key from the `GAFFA_API_KEY` environment variable and nowhere else. They never ask you for it, write it to disk, or log it. You can create a key from your [dashboard](https://gaffa.dev/dashboard/api-keys). Keep it out of version control.

**macOS and Linux:** add this line to your shell profile (for example `~/.zshrc` or `~/.bashrc`), then open a new terminal:

```bash
export GAFFA_API_KEY='your_gaffa_api_key_here'
```

**Windows (PowerShell):** run this once, then open a new terminal:

```powershell
setx GAFFA_API_KEY "your_gaffa_api_key_here"
```
{% endstep %}

{% step %}
### Allow Gaffa through your network

The skills call `api.gaffa.dev` and read docs from `gaffa.dev`. If your tool runs in a sandbox, or you're behind a proxy that blocks outbound traffic, allow `gaffa.dev` and `*.gaffa.dev`, or requests will fail with a connectivity error.

If neither applies to you, skip this step.
{% endstep %}

{% step %}
#### Restart your tool and try it

Restart or reload your AI tool so it loads the skills and your API key. Then ask your agent to do something with Gaffa. See Example prompts for ideas.
{% endstep %}
{% endstepper %}

{% hint style="info" %}
The skills use your Gaffa credits when they run requests. `gaffa-find` has a cost cap you can tune, and `gaffa-debug` only spends credits on an optional re-run, after asking you first. `gaffa-support` doesn't use any credits.
{% endhint %}

## Installation options

There are three ways to install the skills. Whichever you choose, steps 2 to 4 of the [Quick start](gaffa-ai-skills.md#quick-start) still apply.

### Install with the CLI (recommended)

The quickest way, and the one the Quick start uses. It works the same for all six tools and adds the docs MCP server for you.

```bash
npx @gaffa-dev/cli install
```

See [Gaffa CLI](gaffa-cli.md) for all the commands and options, including how to install without prompts.

### Install as a plugin

If you'd rather use your tool's own plugin system, you can install Gaffa as a plugin in Claude Code, Codex, Cursor, and Antigravity. For GitHub Copilot and Pi, use the [CLI](gaffa-ai-skills.md#install-with-the-gaffa-cli-recommended).

{% tabs %}
{% tab title="Claude Code" %}
{% stepper %}
{% step %}
#### Add the Gaffa marketplace

```bash
claude plugin marketplace add https://github.com/GaffaAI/gaffa-for-ai
```

{% hint style="warning" %}
Use the full URL. Claude Code clones the `owner/repo` shorthand over SSH by default, which fails if you don't have a GitHub SSH key.
{% endhint %}
{% endstep %}

{% step %}
#### Install the plugin

```bash
claude plugin install gaffa@gaffa --scope project
```

You can choose where it's installed with `--scope`:

* `user`: for you, in every project. This is the default.
* `project`: shared with everyone who works on the project. Writes to `.claude/settings.json`.
* `local`: only for you, in this project. Writes to `.claude/settings.local.json` and isn't shared.
{% endstep %}

{% step %}
#### Start a new Claude Code session

Close your current session and start a new one so Claude Code loads the skills.
{% endstep %}
{% endstepper %}
{% endtab %}

{% tab title="Codex" %}
{% stepper %}
{% step %}
#### Add the Gaffa marketplace

```bash
codex plugin marketplace add https://github.com/GaffaAI/gaffa-for-ai
```
{% endstep %}

{% step %}
#### Install the plugin

```bash
codex plugin add gaffa@gaffa
```
{% endstep %}

{% step %}
#### Start a new Codex session

Close your current session and start a new one so Codex loads the skills.

In Codex, you start a skill with `$`, for example `$gaffa-find`, or pick it from the skills menu.
{% endstep %}
{% endstepper %}
{% endtab %}

{% tab title="Cursor" %}
Cursor plugins work across Cursor desktop, web, and the Cursor CLI, so you only need to install Gaffa once.

{% hint style="info" %}
You need Cursor 2.4 or later.
{% endhint %}

**Install in Cursor**

{% stepper %}
{% step %}
#### Open the plugin settings

Open Cursor Settings and search for **Plugins**.
{% endstep %}

{% step %}
#### Add the Gaffa repository

Click **+ Add** and select **From a GitHub Repository**, then paste:

```
https://github.com/GaffaAI/gaffa-for-ai
```
{% endstep %}

{% step %}
#### Choose a scope and import

Choose **User** to install it for yourself, or **Team** to share it with your team. Then click **Import**.
{% endstep %}

{% step %}
#### Add the plugin

Once the import finishes, click **Add**. Reload Cursor if the plugin doesn't appear straight away.
{% endstep %}
{% endstepper %}

**Using Cursor CLI**

If you use the Cursor CLI, you can install with one command:

```bash
cursor-agent plugin marketplace add https://github.com/GaffaAI/gaffa-for-ai
```

The plugin works across all of Cursor's agent surfaces, including the Cursor CLI. If you've already installed it in Cursor, you don't need to install it again for the CLI.
{% endtab %}

{% tab title="Antigravity" %}
Gaffa can be used with Antigravity 2.0, Antigravity CLI, and the Antigravity IDE.

**Antigravity 2.0 or IDE**

{% stepper %}
{% step %}
#### Clone the plugin

To make Gaffa available in all your workspaces:

```bash
git clone --depth 1 https://github.com/GaffaAI/gaffa-for-ai ~/.gemini/config/plugins/gaffa
```

Or, to install it for one project only, run this in the project folder:

```bash
git clone --depth 1 https://github.com/GaffaAI/gaffa-for-ai .agents/plugins/gaffa
```
{% endstep %}

{% step %}
#### Remove the Git history (project install only)

If you want to commit the project-level plugin for your team, remove the clone's Git history first:

```bash
rm -rf .agents/plugins/gaffa/.git
```

Otherwise, Git may treat the folder as a separate repository instead of storing its files normally. Skip this step for a global installation.
{% endstep %}

{% step %}
#### Restart Antigravity

Restart or reload Antigravity so it loads the skills.
{% endstep %}
{% endstepper %}

**Antigravity CLI**

If you use Antigravity CLI, you can install the plugin directly from GitHub:

```bash
agy plugin install https://github.com/GaffaAI/gaffa-for-ai
```

Restart your Antigravity CLI session afterwards.
{% endtab %}
{% endtabs %}

### Install by copying

Every skill is a self-contained folder that follows the open [Agent Skills](https://agentskills.io/) format (a `SKILL.md` file with a name and description). That means you can copy the skills you want into the folder your tool reads. The skills don't depend on each other, so copy only the ones you need.

Get the folders from the `skills/` directory of the [gaffa-for-ai repository](https://github.com/GaffaAI/gaffa-for-ai), then copy them into the right place for your tool:

<table><thead><tr><th width="179.0206298828125">Tool</th><th width="243.44805908203125">Project folder</th><th>Personal folder</th></tr></thead><tbody><tr><td>Claude Code</td><td><code>.claude/skills/</code></td><td><code>~/.claude/skills/</code></td></tr><tr><td>Cursor (2.4+)</td><td><code>.agents/skills/</code> or <code>.cursor/skills/</code></td><td><code>~/.agents/skills/</code> or <code>~/.cursor/skills/</code></td></tr><tr><td>Codex CLI</td><td><code>.agents/skills/</code></td><td><code>~/.agents/skills/</code></td></tr><tr><td>Antigravity</td><td><code>.agents/skills/</code></td><td>Expand the section below for your version</td></tr></tbody></table>

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

### If you installed with the CLI

Run `install` again:

```bash
npx @gaffa-dev/cli install
```

It fetches the latest skills and removes any that are no longer part of Gaffa. Restart or reload your tool afterwards.

### If you installed as a plugin

{% tabs %}
{% tab title="Claude Code" %}
{% stepper %}
{% step %}
#### Update the Gaffa marketplace

```bash
claude plugin marketplace update gaffa
```
{% endstep %}

{% step %}
#### Update the plugin

```bash
claude plugin update gaffa@gaffa
```
{% endstep %}

{% step %}
#### Start a new Claude Code session

Close your current session and start a new one so Claude Code loads the updated skills.
{% endstep %}
{% endstepper %}
{% endtab %}

{% tab title="Codex" %}
{% stepper %}
{% step %}
#### Update the Gaffa marketplace

```bash
codex plugin marketplace upgrade gaffa
```
{% endstep %}

{% step %}
#### Start a new Codex session

Close your current session and start a new one so Codex loads the updated skills.
{% endstep %}
{% endstepper %}
{% endtab %}

{% tab title="Cursor" %}
{% stepper %}
{% step %}
#### Install the update

In Cursor, open **Settings → Plugins**, find Gaffa, and install the available update.

Or update it from the Cursor CLI:

```bash
cursor-agent plugin marketplace update gaffa
```

You only need to do one of these. The plugin works across Cursor and the Cursor CLI, so you don't need to update it in both.
{% endstep %}

{% step %}
#### Reload Cursor

Reload Cursor if the update doesn't appear straight away.
{% endstep %}
{% endstepper %}
{% endtab %}

{% tab title="Antigravity" %}
**Antigravity 2.0 or IDE**

{% stepper %}
{% step %}
#### Pull the latest version

For a global installation:

```bash
git -C ~/.gemini/config/plugins/gaffa pull
```

Or, for a project installation, run this in the project folder:

```bash
git -C .agents/plugins/gaffa pull
```
{% endstep %}

{% step %}
#### Restart Antigravity

Restart or reload Antigravity so it loads the updated skills.
{% endstep %}
{% endstepper %}

**Antigravity CLI**

{% stepper %}
{% step %}
#### Uninstall the plugin

```bash
agy plugin uninstall gaffa
```
{% endstep %}

{% step %}
#### Install it again

```bash
agy plugin install https://github.com/GaffaAI/gaffa-for-ai
```
{% endstep %}

{% step %}
#### Restart Antigravity CLI

Restart your Antigravity CLI session, so it loads the updated skills.
{% endstep %}
{% endstepper %}
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

**Still stuck?** Run `/gaffa-support`, or [contact Gaffa](https://gaffa.dev/contact).
