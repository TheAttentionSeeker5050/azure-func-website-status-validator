# Azure Function: Website Status Validator

This azure function will validate sites given by the request body, and then make curl requests to each of these websites to check which ones return a http response code 200, then return a json response with the websites and their statuses.

## Request structure

The request sent to the url path /validate-urls must have the following body:

```bash
# requests body
{
    urls: [
        { url: your-website-url, name: wesbite-name },
        { url: your-website-url, name: wesbite-name },
        { url: your-website-url, name: wesbite-name },
        { url: your-website-url, name: wesbite-name },
        ...
    ]
}
```

The subsequent response must be the following:

```bash
# response body
{
    urls: [
        { url: your-website-url, name: wesbite-name, status: your-website-status },
        { url: your-website-url, name: wesbite-name, status: your-website-status },
        { url: your-website-url, name: wesbite-name, status: your-website-status },
        { url: your-website-url, name: wesbite-name, status: your-website-status },
        ...
    ]
}

# statuses can be "up" (if return fetch ok status), "unauthorized" 
# (if return http 401 or 403), or "down" (if not ok)
```

**Note**: This url accepts both POST and GET requests.

## Next Plans

- Create a HTTP triggered function that displays a Razor page (index at "/" route url) where users can enter these websites via a form and make the requests to the api function. Another consideration would be to have a separate mini-project for this, or a GitHub Page. I will see which one is better.