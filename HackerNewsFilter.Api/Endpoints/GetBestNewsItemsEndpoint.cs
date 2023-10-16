using HackerNewsFilter.Api.Services;
using FastEndpoints;
using HackerNewsFilter.Api.Contracts.Requests;
using HackerNewsFilter.Api.Contracts.Responses;
using HackerNewsFilter.Api.Mapping;
using HackerNewsFilter.Api.Validators;

namespace HackerNewsFilter.Api.Endpoints;

public class GetBestNewsItemsEndpoint : Endpoint<GetBestNewsItemsRequest, List<NewsItemResult>>
{
    private readonly IHackerNewsService _hackerNewsService;

    private readonly int OutPutCacheTimeOutInSeconds = 60;

    public GetBestNewsItemsEndpoint(IHackerNewsService customerService)
    {
        _hackerNewsService = customerService;
    }

    public override void Configure()
    {
        Get("news");
        AllowAnonymous();
        Options(x => x.CacheOutput(p => p.SetVaryByQuery("limit")
            .Expire(TimeSpan.FromSeconds(OutPutCacheTimeOutInSeconds))));
        Validator<GetBestNewsItemsValidator>();
    }

    public override async Task HandleAsync(GetBestNewsItemsRequest request, CancellationToken cancellationToken)
    {
        var bestNewsItems = await _hackerNewsService.GetBestNewsItemsAsync(request.limit);

        if (bestNewsItems is null || !bestNewsItems.Any())
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        var result = bestNewsItems.ToNewsItemsResultList();
        await SendOkAsync(result, cancellationToken);
    }
}