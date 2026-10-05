---
description: >-
  We use API Keys for authenticating requests to our API. In this document we'll
  explain how you can manage and use the keys for your account.
---

# API Authentication

## Creating Keys

Once your account is approved, you will need to create an API key to send your requests to our API. \
\
Go to your account [**Dashboard > API Keys**](https://gaffa.dev/dashboard/api-tokens) and create a new key with a name. Once the key is created, copy the value, and you can immediately start using it to make requests.

{% hint style="info" %}
You can create as many keys as you wish, but always remember to treat the key as a secret and do not reveal it in public blog posts or GitHub repositories. If someone uses your leaked key to make requests, we won't be responsible!
{% endhint %}

## Deleting Keys

If you are worried you have exposed your Gaffa API key, or just want to periodically rotate your keys, you can create a new key and then delete your old keys. Deleted keys will immediately stop working for new API requests, but past browser requests made using old keys will still be available.

## Authenticating Requests

Our API is secured with a customer header `X-API-Key` whose value should be any current API key in your account. That's all you need to add to your request!
