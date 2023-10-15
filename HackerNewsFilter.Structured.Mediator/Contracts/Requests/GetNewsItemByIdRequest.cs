namespace HackerNewsFilter.Structured.Mediator.Contracts.Requests;

public record GetNewsItemByIdRequest
{
    public int Id { get; set; }

    public GetNewsItemByIdRequest(int id)
    {
        Id = id;
    }
}