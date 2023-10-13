using HackerNewsFilter.Structured.Mediator.Models;

namespace HackerNewsFilter.Structured.Mediator.Services;

public interface IHackerNewsClient
{
    Task<NewsItem> GetNewsItemByIdAsync(long id);
    Task<BestNewsItems> GetBestNewsItemsAsync();
}

public class HackerNewsClient : IHackerNewsClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HackerNewsClient(IHttpClientFactory httpClientFactory)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory, nameof(httpClientFactory));

        _httpClientFactory = httpClientFactory;
    }

    public async Task<NewsItem> GetNewsItemByIdAsync(long id)
    {
        var httpClient = _httpClientFactory.CreateClient("HackerNews");

        var response = await httpClient.GetAsync($"item/{id}.json");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<NewsItem>();
    }

    public async Task<BestNewsItems> GetBestNewsItemsAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("HackerNews");

        var response = await httpClient.GetAsync("beststories.json");
        response.EnsureSuccessStatusCode();

        var bestNewIds = await response.Content.ReadFromJsonAsync<List<long>>();
        return new BestNewsItems 
        { 
            BestNewsIds = bestNewIds
        };
    }
}
