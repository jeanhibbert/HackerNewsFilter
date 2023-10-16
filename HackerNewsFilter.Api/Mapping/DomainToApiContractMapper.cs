using HackerNewsFilter.Api.Contracts.Responses;
using HackerNewsFilter.Api.Models;

namespace HackerNewsFilter.Api.Mapping;

public static class DomainToApiContractMapper
{
    public static GetBestNewsItemsResponse ToNewsItemsResponse(this IEnumerable<NewsItem> newsItems)
    {
        return new GetBestNewsItemsResponse
        {
            BestNewsItems = newsItems.Select(x => new NewsItemResult
            {
                Title = x.title,
                Uri = x.url,
                PostedBy = x.by,
                Time = DateTimeOffset.FromUnixTimeSeconds(x.time),
                Score = x.score,
                CommentCount = x.descendants
            })
        };
    }
}
