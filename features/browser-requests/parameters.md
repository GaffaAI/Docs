# Parameters

Parameters are the top-level settings that control the fundamental behaviour of your automation. These parameters define where your request goes, how it's routed, whether it runs synchronously or asynchronously, and how caching is handled.

Below you'll find detailed documentation for each available parameter.

## Proxy servers

Gaffa makes it super simple to proxy your traffic through a global network of residential proxies. Setting `proxy_location` in your request will allow you to utilize one of our partner third-party proxy services to gain local access to a site.

Not setting a `proxy_location` will mean the request does not use a proxy server and will use a generic datacenter IP.

### Available Locations

| Proxy Server Location | Country Code |
| --------------------- | ------------ |
| United States         | `us`         |
| Ireland               | `ie`         |
| Singapore             | `sg`         |
| France                | `fr`         |

{% hint style="info" %}
At the moment, all our servers are in one location, but we aim to deploy local machines at our proxy locations to improve realistic end-user load times. If this interests you, please contact support.
{% endhint %}

### IP Types

Currently, all our IP addresses are residential IP addresses, which are procured through reputable third parties.

### IP Rotation

IP rotation is an essential part of any web data scraping or automation task. In Gaffa, each browser request is treated as unique. We regularly rotate the IP addresses used, so you should assume each request is made from a different IP address than the last.

{% hint style="info" %}
We are working to support a wider range of IP address scenarios, including static IPs in the future, and to enable more trusted proxies for requests that require enhanced security (logins, etc.).
{% endhint %}

### Restrictions

Whilst we'll do our best to provide access to as wide a range of sites as possible, we may have to restrict access to certain sites to prevent abuse of our service or of other services. Our proxy partners may also enforce restrictions on certain sites and categories of sites that we don't have any control over.

***

## Caching

`max_cache_age`: integer (seconds)

When we were building Gaffa, we noticed that many existing scraping tools don't let users easily share their scraped web data, even though many users request the same pages on the same sites. Not only is this a waste of a user's allowance, but it also puts a burden on the site owners who are serving the same data to different users for the same purpose. Because of this, we have created a service-wide cache in Gaffa.

### How it works

When making a browser request, you can provide a `max_cache_age` parameter that is **a number in seconds equal to or greater than 0**. This value denotes the maximum age of data you would accept from the API.\
\
If another user of our service has requested the same URL with exactly the same parameters and actions as you in this timeframe, the response will be returned to you immediately and will not be processed by one of our browsers. If there are multiple identical requests in the given timeframe, then the most recent will be returned.\
\
This will save you time waiting for a response and credits, because requests returned from the cache don't use any bandwidth.

***

## **Settings**

The `settings` object allows you to configure how your browser requests behave. It currently supports six parameters that control request recording, media downloads, execution time limits, ad blocking, redirect logging, and browser actions.

You can read more about all available settings parameters [here](settings.md).
