# HackerNewsFilter

## How to run the application
- Press F5 will launch the swagger UI

## Enahancements I would make given time
- Add docker support
- Add additional integration tests around the HackerNewsService, HackerNewsClient & GetBestNewsItemsEndpoint (with validation check)
- Add additional unit tests around the HackerNewsService, HackerNewsClient & GetBestNewsItemsEndpoint (with validation check)
- Add performance and load tests
- Setup Polly retry in program.cs & perform additional encapsulation of service setup into extension methods

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
- Wanted to use a combination of output caching (60 second timeout) for multiple calls with the same limit query parameter, as well as in memory caching of the full best news item list (ordered by score) with a refresh time window of 5 minutes.
The cache refresh strategies can be optimised over time.

## Reference URL
https://github.com/HackerNews/API
