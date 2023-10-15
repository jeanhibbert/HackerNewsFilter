namespace HackerNewsFilter.Structured.Mediator.Contracts.Responses;

public class GetBestNewsItemsResponse
{
    public IEnumerable<NewsItemResponse> BestNewsItems { get; internal set; }
}
