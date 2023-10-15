namespace HackerNewsFilter.Structured.Mediator.Contracts.Requests;

public record GetBestNewsItemsRequest
{
    public int FetchCount { get; set; }
}