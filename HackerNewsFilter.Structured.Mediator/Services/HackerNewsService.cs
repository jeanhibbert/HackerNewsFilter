using HackerNewsFilter.Structured.Mediator;
using HackerNewsFilter.Structured.Mediator.Models;
using HackerNewsFilter.Structured.Mediator.Services;
using Microsoft.Extensions.Caching.Memory;

namespace DotnetDocsShow.Structured.Mediator.Services;

public interface IHackerNewsService
{
    Task<List<NewsItem>> GetBestNewsItemsAsync(int fetchCount);
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

    public async Task<List<NewsItem>> GetBestNewsItemsAsync(int fetchCount)
    {
        var bestNewsItems = await _cache.GetOrCreateAsync(Config.GetBestNewsKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(GetBestNewsCachePeriodInMinutes);
            return await FetchAllNewsItems();
        });

        return bestNewsItems.Take(fetchCount).ToList();
    }

    public async Task<List<NewsItem>> FetchAllNewsItems()
    {
        var bestNewsItems = await _hackerNewsClient.GetAllBestNewsItemsAsync();
        
        var itemFetchTaskList = bestNewsItems.BestNewsIds.Select(_hackerNewsClient.GetNewsItemByIdAsync);
        
        var result = await Task.WhenAll(itemFetchTaskList);

        return result
            .OrderByDescending(x => x.descendants)
            .ToList();
    }
}
