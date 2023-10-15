using HackerNewsFilter.Structured.Mediator.Contracts.Responses;
using HackerNewsFilter.Structured.Mediator.Models;

namespace HackerNewsFilter.Structured.Mediator.Mapping;

public static class DomainToApiContractMapper
{
    public static NewsItemResponse ToNewsItemResponse(this NewsItem newsItem)
    {
        return new NewsItemResponse
        {
            id = newsItem.id
        };
    }

    public static GetBestNewsItemsResponse ToNewsItemsResponse(this IEnumerable<NewsItem> newsItems)
    {
        return new GetBestNewsItemsResponse
        {
            BestNewsItems = newsItems.Select(x => new NewsItemResponse
            {
               id = x.id
            })
        };
    }
}
