# HackerNewsFilter

## Implementation specification
- Get full list of best news items
- Populate the item list
- Cache the item list
- All further lookups can get Top(n) news items from cache
- Periodically refresh the cache based on a time window

## Third party frameworks used
- FastEndpoints
- FluentValidation
- Swagger
- Polly : For Http external endpoint retry strategies

## Caching strategies
- Wanted to use a combination of output caching for multiple calls with the same fetchCount, as well as in memory caching of the full best news item list (ordered) with a refresh time window of 5 minutes.
This cache refresh strategies can be monitored and optimised over time.
- The internal in memory caching strategy could be optimized further by incrementally fetching more news items to be hydrated in the cached NewsItemList as fetchCount increases. i.e. As fetch count get bigger fetch more NewsItems and don't fetch all the best news items in one go. This was done for simplicity.

## Improvements outstanding
- Could add some unit tests around the HackerNewsService and HackerNewsClient
