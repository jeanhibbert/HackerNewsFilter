using HackerNewsFilter.Structured.Mediator.Models;

namespace HackerNewsFilter.Structured.Mediator.Services;

public interface IHackerNewsClient
{
    Task<NewsItem> GetNewsItemByIdAsync(int id);
    Task<BestNewsItems> GetAllBestNewsItemsAsync();
}

public class HackerNewsClient : IHackerNewsClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HackerNewsClient(IHttpClientFactory httpClientFactory)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory, nameof(httpClientFactory));

        _httpClientFactory = httpClientFactory;
    }

    public async Task<NewsItem> GetNewsItemByIdAsync(int id)
    {
        var httpClient = _httpClientFactory.CreateClient(Config.HackerNewsBaseUrlName);

        var response = await httpClient.GetAsync($"item/{id}.json");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<NewsItem>();
    }

    public async Task<BestNewsItems> GetAllBestNewsItemsAsync()
    {
        var httpClient = _httpClientFactory.CreateClient(Config.HackerNewsBaseUrlName);

        var response = await httpClient.GetAsync("beststories.json");
        response.EnsureSuccessStatusCode();

        var bestNewIds = await response.Content.ReadFromJsonAsync<List<int>>();
        return new BestNewsItems 
        { 
            BestNewsIds = bestNewIds
        };
    }
}
