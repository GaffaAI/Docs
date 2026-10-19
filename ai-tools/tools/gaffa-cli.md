---
description: Set up the Gaffa AI skills in your AI coding tools with one command.
---

# Gaffa CLI

The Gaffa CLI installs the [Gaffa AI skills](gaffa-ai-skills.md) into your AI coding tools and registers the [Gaffa docs MCP server](gaffa-docs-mcp-server.md), so your agent is ready to build with Gaffa. It works with Claude Code, Codex, GitHub Copilot, Cursor, Antigravity, and Pi.

## Quick start

{% stepper %}
{% step %}
### Run the installer

Run this in the folder of the project you want to set up:

```bash
npx @gaffa-dev/cli install
```

The CLI finds the AI tools on your machine, asks which ones to set up and where to install, then copies the skills and registers the docs MCP server.

You don't need to install the CLI first. `npx` comes with Node.js, and it downloads and runs the latest version of the CLI for you. The first time you run it, npx asks `Ok to proceed?`. Type `y` to continue.
{% endstep %}

{% step %}
### Set your API key

The CLI itself doesn't need an API key, but the skills it installs do, because they send requests to the Gaffa API. Set up your key as described in [Authentication](gaffa-cli.md#authentication).
{% endstep %}

{% step %}
### Restart your tool

Restart or reload your AI tool so it loads the skills and your API key.
{% endstep %}
{% endstepper %}

## Authentication

The skills read your key from the `GAFFA_API_KEY` environment variable and nowhere else. They never ask you for it, write it to disk, or log it. You can create a key from your [dashboard](https://gaffa.dev/dashboard/api-keys). Keep it out of version control.

**macOS and Linux:** add this line to your shell profile (for example `~/.zshrc` or `~/.bashrc`), then open a new terminal:

```bash
export GAFFA_API_KEY='your_gaffa_api_key_here'
```

**Windows (PowerShell):** run this once, then open a new terminal:

```powershell
setx GAFFA_API_KEY "your_gaffa_api_key_here"
```

Restart your AI tool after setting the key so it picks up the new value.

## Supported tools

Use these IDs with the `--tools` flag:

<table><thead><tr><th width="301.13775634765625">Tool</th><th>ID</th></tr></thead><tbody><tr><td>Claude Code</td><td><code>claude-code</code></td></tr><tr><td>Codex</td><td><code>codex</code></td></tr><tr><td>GitHub Copilot</td><td><code>copilot</code></td></tr><tr><td>Cursor</td><td><code>cursor</code></td></tr><tr><td>Antigravity</td><td><code>antigravity</code></td></tr><tr><td>Pi</td><td><code>pi</code></td></tr></tbody></table>

## Commands

### `install`

Copies the Gaffa skills into the tools you pick and registers the Gaffa docs MCP server in each tool's MCP config. If you already have other MCP servers set up, it leaves them as they are.

```bash
npx @gaffa-dev/cli install
```

With no flags, `install` asks you two things:

* **Which tools to set up.** The default is every supported tool it finds on your machine.
* **Which scope to install into:**
  * `project` (the default) installs into the current folder, so you can share the skills with everyone on the project. To share them, commit the skills folder together with its `.gaffa-skills.json` file.
  * `personal` installs into your home folder, so the skills are available to you in every project.

To skip the questions, pass your choices as flags. This is useful in scripts and CI:

```bash
npx @gaffa-dev/cli install --tools=claude-code,cursor --scope=personal -y
```

<table><thead><tr><th width="235.23583984375">Flag</th><th>What it does</th></tr></thead><tbody><tr><td><code>--tools=a,b</code></td><td>The tools to set up, as a comma-separated list of tool IDs. The default is the tools found on your machine.</td></tr><tr><td><code>--scope=project</code></td><td>Where to install: <code>project</code> or <code>personal</code>. The default is <code>project</code>.</td></tr><tr><td><code>-y</code>, <code>--yes</code></td><td>Use the defaults for anything you haven't passed, without asking.</td></tr><tr><td><code>--no-mcp</code></td><td>Install the skills without registering the docs MCP server.</td></tr></tbody></table>

With a project install, Claude Code asks you to approve the MCP server the first time you use it.

### `doctor`

Shows which supported tools are on your machine, where each one keeps its config, and whether the Gaffa skills are set up in it. It also tells you when your installed skills are older than the latest version. `doctor` only reads your setup and never changes anything.

```bash
npx @gaffa-dev/cli doctor
```

Add `--json` to get the same report as structured output for scripts:

```bash
npx @gaffa-dev/cli doctor --json
```

### `uninstall`

Removes the skills that `install` wrote for a scope, and the docs MCP server it registered.

```bash
npx @gaffa-dev/cli uninstall --scope=project
```

If you've edited a skill since you installed it, `uninstall` leaves it in place and tells you, so your changes are never lost. If you've changed the MCP server entry to point somewhere else, that's left alone too.

<table><thead><tr><th width="204.89129638671875">Flag</th><th>What it does</th></tr></thead><tbody><tr><td><code>--scope=project</code></td><td>The scope to remove from: <code>project</code> or <code>personal</code>. The default is <code>project</code>.</td></tr><tr><td><code>-y</code>, <code>--yes</code></td><td>Use the defaults without asking.</td></tr><tr><td><code>--no-mcp</code></td><td>Remove the skills but keep the docs MCP server registered.</td></tr></tbody></table>

### Other options

To list the commands and flags:

```bash
npx @gaffa-dev/cli --help
```

To print the CLI version:

```bash
npx @gaffa-dev/cli --version
```

### Update the skills

Run `install` again:

```bash
npx @gaffa-dev/cli install
```

It fetches the latest skills, replaces the ones you have, and removes any skills that are no longer part of Gaffa. Restart your tool afterwards. Run `doctor` to check whether you're behind the latest version.
