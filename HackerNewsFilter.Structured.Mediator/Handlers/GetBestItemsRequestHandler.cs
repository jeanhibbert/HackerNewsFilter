using DotnetDocsShow.Structured.Mediator.Services;
using MediatR;

namespace DotnetDocsShow.Structured.Mediator.Handlers;

public record GetBestItemsRequest : IRequest<IResult>;

public class GetBestItemsRequestHandler : IRequestHandler<GetBestItemsRequest, IResult>
{
    private readonly IHackerNewsService _hackerNewsService;

    public GetBestItemsRequestHandler(IHackerNewsService hackerNewsService)
    {
        _hackerNewsService = hackerNewsService;
    }

    public async Task<IResult> Handle(GetBestItemsRequest request, CancellationToken cancellationToken)
    {
        var bestNewsItems = await _hackerNewsService.GetAll();
        var response = Results.Ok(bestNewsItems);
        return response;
    }
}
