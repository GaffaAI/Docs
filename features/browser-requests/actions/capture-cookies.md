# Capture Cookies

**Type:** `capture_cookies`

This action will capture the browser cookies currently saved for the web page you are on and return them as a JSON object with key/values.

<details>

<summary>Tips for using <code>capture_cookies</code> action</summary>

* Run this after the steps that set the cookies you want, such as logging in or accepting a cookie pop-up.
* Treat the output like a password. Session cookies are credentials.
* Use it to work out why a page looks different to what you expected; the cookies often explain it.
* Pair it with a screenshot when you're debugging a login, so you can see the page and its cookies together.

</details>

### Parameters

See [universal parameters](./#universal-parameters).

### Usage

Capture the cookies of the current page

```
"actions": [
    {
      "type": "capture_cookies"
    }
]
```

### FAQs

#### When do I use capture\_cookies?

Use it when you need to see or save the session a page is running on after a login, after accepting cookies, or when a page behaves oddly, and you want to investigate why.

#### What does capture\_cookies return?

It returns a JSON object containing the cookie names and values the browser holds for the page, captured at the moment the action runs in your list.

#### When should I run capture\_cookies in my action list?

Run it after anything that sets the cookies you care about, like logging in or dismissing a banner. If you capture first, those cookies won't exist yet.

#### How do I capture cookies after logging into a site?

Use [`type`](type.md) for the username and password, [`click`](click.md) for the submit button, a [`wait`](wait.md) for something on the logged-in page, then `capture_cookies`.

#### Can I reuse captured cookies in another request?

Not at the moment. Each browser request starts a fresh session, so cookies captured in one request can't be sent with the next. We're working on it. If this would be useful for what you're building, email [support](mailto:support@gaffa.dev).

#### What information does capture\_cookies return?

Cookie names and values only. Domain, expiry and flags like `httpOnly` aren't included in the output at the moment.
