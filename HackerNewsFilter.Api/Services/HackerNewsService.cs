using HackerNewsFilter.Api;
using HackerNewsFilter.Api.Models;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsFilter.Api.Services;

public interface IHackerNewsService
{
    Task<List<NewsItem>> GetBestNewsItemsAsync(int limit, CancellationToken cancellationToken = default);
}

public class HackerNewsService : IHackerNewsService
{
    private readonly IHackerNewsClient _hackerNewsClient;
    private readonly IMemoryCache _cache;

    private readonly int GetBestNewsCachePeriodInMinutes = 5;

    public HackerNewsService(IHackerNewsClient hackerNewsClient, IMemoryCache cache) 
    { 
        ArgumentNullException.ThrowIfNull(hackerNewsClient, nameof(hackerNewsClient));
        ArgumentNullException.ThrowIfNull(cache, nameof(cache));

        _hackerNewsClient = hackerNewsClient;
        _cache = cache;
    }

    public async Task<List<NewsItem>> GetBestNewsItemsAsync(int limit, CancellationToken cancellationToken = default)
    {
        var bestNewsItems = await _cache.GetOrCreateAsync(Constants.GetBestNewsKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(GetBestNewsCachePeriodInMinutes);
            return await FetchAllNewsItems(cancellationToken);
        });

        return bestNewsItems.Take(limit).ToList();
    }

    private async Task<List<NewsItem>> FetchAllNewsItems(CancellationToken cancellationToken)
    {
        var bestNewsItems = await _hackerNewsClient.GetAllBestNewsItemsAsync(cancellationToken);
        
        var itemFetchTaskList = bestNewsItems.BestNewsIds.Select(x => _hackerNewsClient.GetNewsItemByIdAsync(x, cancellationToken));
        
        var result = await Task.WhenAll(itemFetchTaskList);

        return result
            .OrderByDescending(x => x.score)
            .ToList();
    }
}
