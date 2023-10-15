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

## Integration Tests
- Periodically refresh the cache based on a time window

## Improvements outstanding
- Could add some unit tests around the HackerNewsService and HackerNewsClient
- Count optimize initial loads by only populating top n news items requested, when they are requested.
This would increase complexity but only marginally
