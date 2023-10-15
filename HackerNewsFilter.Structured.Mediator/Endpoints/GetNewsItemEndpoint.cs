using DotnetDocsShow.Structured.Mediator.Services;
using FastEndpoints;
using HackerNewsFilter.Structured.Mediator.Contracts.Requests;
using HackerNewsFilter.Structured.Mediator.Contracts.Responses;
using HackerNewsFilter.Structured.Mediator.Mapping;
using Microsoft.AspNetCore.Authorization;

namespace HackerNewsFilter.Structured.Mediator.Endpoints;

[HttpGet("news/{id:int}"), AllowAnonymous]
public class GetCustomerEndpoint : Endpoint<GetNewsItemByIdRequest, NewsItemResponse>
{
    private readonly IHackerNewsService _hackerNewsService;

    public GetCustomerEndpoint(IHackerNewsService customerService)
    {
        _hackerNewsService = customerService;
    }

    public override async Task HandleAsync(GetNewsItemByIdRequest request, CancellationToken cancellationToken)
    {
        var newsItem = await _hackerNewsService.GetNewsItemByIdAsync(request.Id);

        if (newsItem is null)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        var newsItemResponse = newsItem.ToNewsItemResponse();
        await SendOkAsync(newsItemResponse, cancellationToken);
    }
}