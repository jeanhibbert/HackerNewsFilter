using System.Text.Json.Serialization;
using DotnetDocsShow.Structured.Mediator.Services;
using MediatR;

namespace DotnetDocsShow.Structured.Mediator.Handlers;

public record GetNewsItemByIdRequest : IRequest<IResult>
{
    [JsonPropertyName("id")]
    public long Id { get; init; }

    public GetNewsItemByIdRequest(long id)
    {
        Id = id;
    }
}

public class GetNewsItemByIdRequestHandler : IRequestHandler<GetNewsItemByIdRequest, IResult>
{
    private readonly IHackerNewsService _hackerNewsService;

    public GetNewsItemByIdRequestHandler(IHackerNewsService hackerNewsService)
    {
        _hackerNewsService = hackerNewsService;
    }

    public async Task<IResult> Handle(GetNewsItemByIdRequest request, CancellationToken cancellationToken)
    {
        var newsItem = await _hackerNewsService.GetNewsItemByIdAsync(request.Id);
        var response = newsItem is not null ? Results.Ok(newsItem) : Results.NotFound();
        return response;
    }
}
