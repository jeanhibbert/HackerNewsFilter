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
            var httpResponse = await httpClient.GetAsync($"item/{id}.json", cancellationToken);
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Get News Item Request failed with status code {httpResponse.StatusCode}");
            }

            return await httpResponse.Content.ReadFromJsonAsync<NewsItem>();
        });
    }

    public async Task<BestNewsItems> GetAllBestNewsItemsAsync(CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient(Constants.HackerNewsBaseUrlName);

        return await _asyncRetryPolicy.ExecuteAsync(async () =>
        {
            var httpResponse = await httpClient.GetAsync("beststories.json", cancellationToken);
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Best Stories Request failed with status code {httpResponse.StatusCode}");
            }

            var bestNewIds = await httpResponse.Content.ReadFromJsonAsync<List<int>>();
            return new BestNewsItems
            {
                BestNewsIds = bestNewIds
            };
        });
    }
}
