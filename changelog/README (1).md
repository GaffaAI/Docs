# August

### API Updates

#### `loop` action now available

You can now repeat a sequence of nested actions to paginate through multi-page results in a single browser request, rather than sending a request per page. Set `max_iterations` or `iterations` to control how many times it runs, and `stop_on_fail` to have it exit automatically once a "next page" control disappears. Marked as beta while we gather feedback. [Read the docs.](https://gaffa.dev/docs/features/browser-requests/actions/loop)

#### `log_redirects` setting now available

You can now see every URL a browser request passes through before it reaches the final page. Set `log_redirects` to `true` in `settings`, and each redirect hop, including any triggered by actions like `click`, is returned in a new `redirects` field. This is useful for checking campaign links, auditing affiliate routing, or debugging unexpected destinations. [Read the docs.](https://app.gitbook.com/s/yUba6osOT5MkKiV0wmgr/features/browser-requests/settings#redirect-logging)

#### New API Playground example: pagination with `loop`

Added a ready-to-run template showing `loop` in action, paginating through a listing page and capturing each one. Test it directly in the Playground or read through the request and response. [View the example.](https://gaffa.dev/docs/features/browser-requests/api-playground-examples/loop-through-pagination)
