using DotnetDocsShow.Structured.Mediator.Services;
using FastEndpoints;
using HackerNewsFilter.Structured.Mediator.Contracts.Requests;
using HackerNewsFilter.Structured.Mediator.Contracts.Responses;
using HackerNewsFilter.Structured.Mediator.Mapping;
using Microsoft.AspNetCore.Authorization;

namespace HackerNewsFilter.Structured.Mediator.Endpoints;

[HttpGet("news/{fetchCount:int}"), AllowAnonymous]
public class GetBestNewsItemsEndpoint : Endpoint<GetBestNewsItemsRequest, GetBestNewsItemsResponse>
{
    private readonly IHackerNewsService _hackerNewsService;

    public GetBestNewsItemsEndpoint(IHackerNewsService customerService)
    {
        _hackerNewsService = customerService;
    }

    public override async Task HandleAsync(GetBestNewsItemsRequest request, CancellationToken cancellationToken)
    {
        var bestNewsItems = await _hackerNewsService.GetBestNewsItemsAsync(request.FetchCount);

        if (bestNewsItems is null || !bestNewsItems.Any())
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        var result = bestNewsItems.ToNewsItemsResponse();
        await SendOkAsync(result, cancellationToken);
    }
}