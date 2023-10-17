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
- HackerNewsHosted background service starts up and caches the news items every 5 minutes
- When an api call is recieved it either grabs the ordered news items from the cache or output cache kicks in if the same limit was used within 60 seconds
    
## Third party frameworks used
- FastEndpoints : https://fast-endpoints.com
- FluentValidation
- Swagger
- Polly : For Http external endpoint retry (on failure) strategies

## Caching strategies
- HackerNewsHostService caches data on startup and every 5 minutes thereafter

## Reference URL
https://github.com/HackerNews/API
