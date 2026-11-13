# October

### AI Tools

#### Gaffa AI skills and Gaffa CLI, now available

Your coding agent can now write and review Gaffa code, find information on a live website, debug a failing request, and prepare a support report when something goes wrong. The AI skills work in Claude Code, Codex, Cursor, GitHub Copilot, Antigravity, and Pi. Install them with one command using the Gaffa CLI, which also sets up the Gaffa docs MCP server for you:

```shellscript
npx @gaffa-dev/cli install
```

[Read the docs.](https://app.gitbook.com/s/Mlx6Dm0tMl9eKr0tbRvJ/)

### Platform Improvements

#### Improved reliability on complex sites

We've been working on the browser infrastructure behind Gaffa's scraping browser. This means fewer failed requests and more reliable access to complex public sites, with no changes needed to your existing requests.

### Samples, Blog & Tutorials

#### Why AI Adoption Is Fueling Demand for Better Web Data Extraction

We wrote a piece for SourceForge on how AI has changed the economics of web scraping. LLMs can now generate and fix selectors in seconds, but bot detection has evolved just as fast, and keeping a scraper running in production now means managing fingerprinting, proxies, sessions, and constant detection changes. The article covers why asking your agent to rebuild that infrastructure tends to make things worse, and why giving agents a dedicated retrieval layer keeps that complexity out of your codebase. [Read the article.](https://sourceforge.net/articles/why-ai-adoption-is-fueling-demand-for-better-web-data-extraction/)

#### Scrape Twitter (X) Without an Account

We published a tutorial on scraping public tweets with Python and Gaffa’s `capture_dom` action, with no X login, developer credentials, or local browser. It walks through using `wait` to load the profile, collecting tweet URLs from the captured HTML, and saving each post’s text, timestamp, and engagement counts as JSON. It also covers how to get an exact UTC timestamp from the tweet ID itself. The full code is on GitHub. [Read the blog.](https://gaffa.dev/blog/how-to-scrape-twitter-without-authentication-using-gaffa)
