namespace HackerNewsFilter.Api.Contracts.Responses;

public class GetBestNewsItemsResponse
{
    public IEnumerable<NewsItemResponse> BestNewsItems { get; internal set; }
}
