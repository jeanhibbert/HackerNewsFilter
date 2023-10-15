using HackerNewsFilter.Api.Models;
using Polly;
using Polly.Retry;

namespace HackerNewsFilter.Api.Services;

public interface IHackerNewsClient
{
    Task<NewsItem> GetNewsItemByIdAsync(int id, CancellationToken cancellationToken);
    Task<BestNewsItems> GetAllBestNewsItemsAsync(CancellationToken cancellationToken);
}

public class HackerNewsClient : IHackerNewsClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    
    private readonly AsyncRetryPolicy _asyncRetryPolicy;
    public HackerNewsClient(IHttpClientFactory httpClientFactory)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory, nameof(httpClientFactory));

        _httpClientFactory = httpClientFactory;

        _asyncRetryPolicy = Policy
             .Handle<HttpRequestException>()
             .WaitAndRetryAsync(
                retryCount: 3, 
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            );
    }

    public async Task<NewsItem> GetNewsItemByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient(Constants.HackerNewsBaseUrlName);

        return await _asyncRetryPolicy.ExecuteAsync(async () =>
        {
            var response = await httpClient.GetAsync($"item/{id}.json", cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<NewsItem>();
        });
    }

    public async Task<BestNewsItems> GetAllBestNewsItemsAsync(CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient(Constants.HackerNewsBaseUrlName);

        return await _asyncRetryPolicy.ExecuteAsync(async () =>
        {
            var response = await httpClient.GetAsync("beststories.json", cancellationToken);
            response.EnsureSuccessStatusCode();

            var bestNewIds = await response.Content.ReadFromJsonAsync<List<int>>();
            return new BestNewsItems
            {
                BestNewsIds = bestNewIds
            };
        });
    }
}
