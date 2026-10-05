# Selectors

Many Gaffa actions take a `selector` — a short piece of text that tells Gaffa which element on the page to work with, like a button to click or a table to read.

Most people write CSS selectors, and all of those work. But Gaffa uses Playwright's selector engine, so you can also match elements by their text or their visibility. XPath works too.

### Writing selectors in JSON

Your selector goes inside a JSON string, so any double quotes inside it will break the request. Use single quotes instead.

This is valid:

```json
"selector": "input[name='email']"
```

This is not:

```json
"selector": "input[name="email"]"
```

You can also escape the double quotes with backslashes, but single quotes are easier to get right. This matters more than it looks, because most of the extensions below take a quoted argument.

***

### Standard CSS

Every standard CSS selector works, including newer ones like `:has()`, `:is()` and `:not()`. Attribute selectors, combinators and structural selectors all behave exactly as they do in a browser.

Pseudo-elements such as `::before`, `::after` and `::placeholder` can't be targeted, because there's no real element to return.

See the [MDN CSS selectors reference](https://developer.mozilla.org/en-US/docs/Web/CSS/Reference/Selectors) for the full list.

***

### Playwright extensions

Alongside standard CSS selectors, Gaffa supports Playwright's selector extensions.

#### Finding elements by text

| Selector                          | What it matches                                                                    |
| --------------------------------- | ---------------------------------------------------------------------------------- |
| `:has-text('foo')`                | Any element whose contents include that text anywhere inside it. Case-insensitive. |
| `:text('foo')`                    | The smallest element directly containing that text. Case-insensitive, trimmed.     |
| `:text-is('foo')`                 | Exact match only. Case-sensitive, trimmed.                                         |
| `:text-matches('regex', 'flags')` | Matches the element's text against a regular expression.                           |

Use these when a button or link has no useful class or ID, which is common on sites built with utility CSS frameworks.

```json
{ "type": "click", "selector": "button:text('Accept all')" }
```

#### Other extensions

<table><thead><tr><th width="236.577392578125">Selector</th><th>What it does</th></tr></thead><tbody><tr><td><code>:visible</code></td><td>Matches only elements that are actually rendered and visible.</td></tr><tr><td><code>:nth-match(selector, n)</code></td><td>The nth match across the whole page, unlike <code>:nth-child</code> which counts within one parent.</td></tr><tr><td><code>:light()</code></td><td>Matches only the normal page DOM, ignoring content inside web components.</td></tr><tr><td><code>>></code></td><td>Chains selector types together, for example <code>css=... >> xpath=...</code>.</td></tr></tbody></table>

### XPath

You can use XPath anywhere a selector is accepted. Start the selector with `//` or `xpath=`.

```json
{ "type": "click", "selector": "//button[contains(text(), 'Submit')]" }
```

XPath is useful when you need to select an element by its relationship to another one, a parent, or a preceding sibling, which CSS can't express.

***

### Elements inside iframes

An iframe is a separate web page embedded inside the one you requested. Embedded videos, payment forms, maps and comment widgets are usually iframes.

* **Same-origin iframes**, meaning frames served from the same domain as the page, can be reached. Write your selector as normal, and Gaffa will find the element inside the frame; you don't need to target the frame first.
* **Cross-origin iframes**, meaning frames served from a different domain, cannot currently be reached. Selectors won't match elements inside them. Embedded YouTube players, payment widgets and third-party tools are usually cross-origin.

To tell which you're dealing with, compare the iframe's `src` with the page's own URL. A different domain means cross-origin.

### **Elements inside web components**

Some sites build parts of the page as web components, which keep their internal markup in a shadow DOM. Gaffa looks inside open shadow roots automatically, so you can write a selector for an element inside a component without doing anything special.

{% hint style="info" %}
**An element missing from a DOM capture may still be reachable.**

[`capture_dom`](actions/capture-dom.md) and [`generate_simplified_dom`](actions/generate-simplified-dom.md) return the main page only. Iframes and web components both appear as empty tags, with no content. So if you can see something on screen but can't find it in a capture, don't assume it can't be selected. If the frame is same-origin, or the content is inside a web component, try the selector.
{% endhint %}

***

### Examples

**Click a button that has no ID or class**

```json
{ "type": "click", "selector": "button:text('Accept all')" }
```

**Fill a field identified by its label**

```json
{ "type": "type", "selector": "input[aria-label='Email']", "text": "test@test.com" }
```

**Click the retry button in the row whose status says "Failed"**

```json
{ "type": "click", "selector": "tr:has(td:text-is('Failed')) button[aria-label='Retry']" }
```

**Wait for a success message inside a dialog**

```json
{ "type": "wait", "selector": "[role='dialog'] h2:has-text('Success!')", "timeout": 10000 }
```

***

### Traps worth knowing

* **`:has-text()` matches parents too.** `div:has-text('Submit')` matches every `div` containing that text, including the outermost one. Use `:text()` when you want the tightest element, or scope it to a container first.
* **Generated class names change.** Class names produced by build tools are rebuilt on every deploy. Prefer `id`, `data-testid`, `aria-label` or a text selector.
* **Avoid positional selectors.** Playwright has deprecated `:right-of()` and the others, and matching on layout is unreliable — a small change moves things around.
* **Long selectors are hard to debug.** If a selector needs more than two or three conditions, it's usually easier to `click` your way to a narrower part of the page first.

***

### FAQs

#### What kind of selectors does Gaffa support?

Gaffa uses Playwright's selector engine, so you can use any standard CSS selector, Playwright's extensions for matching text and visibility, and XPath expressions.

#### How do I write a selector with quotes inside a JSON request?

Use single quotes inside the selector so the JSON string stays valid, like `"input[name='email']"`. You can also escape double quotes with backslashes. Unescaped double quotes will break the request.

#### How do I select an element that has no ID or class?

Use a text selector like `button:text('Submit')`, or match on a stable attribute such as `aria-label` or `data-testid`. Both are more reliable than generated class names.

#### Can Gaffa select elements inside an iframe?

Same-origin iframes can be reached, so write your selector as normal, without targeting the frame first. Cross-origin iframes served from a different domain can't currently be accessed.

#### Why can't I click something inside an embedded video or payment widget?

Those are usually cross-origin iframes, served from a different domain to the page. Selectors can't currently reach inside them. Same-origin frames work normally.

#### Why can't I find an element in my DOM capture even though I can see it on the page?

It's probably inside an iframe. DOM captures return the main page only, showing iframes as empty tags. If the frame is same-origin, the element may still be selectable — try your selector directly.

#### What's the difference between :has-text() and :text()?

`:has-text()` matches any element containing that text, including parent elements. `:text()` matches only the smallest element directly containing it, which is usually what you want.
