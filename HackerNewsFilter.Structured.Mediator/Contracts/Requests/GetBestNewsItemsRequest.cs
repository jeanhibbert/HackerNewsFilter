namespace HackerNewsFilter.Api.Contracts.Requests;

public record GetBestNewsItemsRequest
{
    public int FetchCount { get; set; }
}