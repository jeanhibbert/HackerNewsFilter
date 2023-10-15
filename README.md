# HackerNewsFilter

## How to run the application
- Press F5 will launch the swagger UI

## Enahancements I would make given time
- Add docker support
- Add additional integration tests around the HackerNewsService, HackerNewsClient & GetBestNewsItemsEndpoint
- Add additional unit tests around the HackerNewsService, HackerNewsClient & GetBestNewsItemsEndpoint

## Implementation specification
- Get full list of best news items
- Populate the item list
- Cache the item ordered list (5 minutes)
- All further lookups can get Top(n) news items from cache
- Periodically refresh the cache based on a time window

## Third party frameworks used
- FastEndpoints : https://fast-endpoints.com
- FluentValidation
- Swagger
- Polly : For Http external endpoint retry (on failure) strategies

## Caching strategies
- Wanted to use a combination of output caching (60 second timeout) for multiple calls with the same fetchCount query parameter, as well as in memory caching of the full best news item list (ordered by score) with a refresh time window of 5 minutes.
This way the cache refresh strategies are used together can be optimised over time.

## Reference URLS
https://github.com/HackerNews/API
https://hacker-news.firebaseio.com/v0/item/21233041.json