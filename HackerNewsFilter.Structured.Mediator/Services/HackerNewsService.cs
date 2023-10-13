using HackerNewsFilter.Structured.Mediator.Models;
using HackerNewsFilter.Structured.Mediator.Services;

namespace DotnetDocsShow.Structured.Mediator.Services;

public interface IHackerNewsService
{
    Task<NewsItem> GetNewsItemByIdAsync(long id);

    Task<BestNewsItems> GetAll();
}

public class HackerNewsService : IHackerNewsService
{
    private readonly IHackerNewsClient _hackerNewsClient;

    public HackerNewsService(IHackerNewsClient hackerNewsClient) 
    { 
        ArgumentNullException.ThrowIfNull(hackerNewsClient, nameof(hackerNewsClient));

        _hackerNewsClient = hackerNewsClient;
    }

    public Task<NewsItem> GetNewsItemByIdAsync(long id)
    {
        return _hackerNewsClient.GetNewsItemByIdAsync(id);
    }

    public Task<BestNewsItems> GetAll()
    {
        return _hackerNewsClient.GetBestNewsItemsAsync();
    }
}
