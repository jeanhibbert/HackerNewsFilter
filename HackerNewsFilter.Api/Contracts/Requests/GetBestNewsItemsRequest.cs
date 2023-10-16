namespace HackerNewsFilter.Api.Contracts.Requests;

public record GetBestNewsItemsRequest
{
    public int limit { get; set; }
}