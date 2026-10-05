# August

### API Updates

#### `loop` action now available

You can now repeat a sequence of nested actions to paginate through multi-page results in a single browser request, rather than sending a request per page. Set `max_iterations` or `iterations` to control how many times it runs, and `stop_on_fail` to have it exit automatically once a "next page" control disappears. Marked as beta while we gather feedback. [Read the docs.](https://gaffa.dev/docs/features/browser-requests/actions/loop)

#### New API Playground example: pagination with `loop`

Added a ready-to-run template showing `loop` in action, paginating through a listing page and capturing each one. Test it directly in the Playground or read through the request and response. [View the example.](https://gaffa.dev/docs/features/browser-requests/api-playground-examples/loop-through-pagination)
