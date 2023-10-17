using HackerNewsFilter.Api.Services;

namespace HackerNewsFilter.Api.HostedService;

public sealed class HackerNewsHostedService : IHostedService
{
    private readonly IHackerNewsService _hackerNewsService;
    private readonly Task _completedTask = Task.CompletedTask;
    private readonly ILogger<HackerNewsHostedService> _logger;
    private readonly PeriodicTimer _timer;
    private readonly CancellationTokenSource _cts = new();
    private const int GetBestNewsCachePeriodInMinutes = 5;

    private Task _timerTask;
    private int _executionCount = 0;
    
    public HackerNewsHostedService(IHackerNewsService hackerNewsService, ILogger<HackerNewsHostedService> logger) 
    {
        ArgumentNullException.ThrowIfNull(hackerNewsService, nameof(hackerNewsService));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _hackerNewsService = hackerNewsService;   
        _logger = logger;

        _timer = new(TimeSpan.FromMinutes(GetBestNewsCachePeriodInMinutes));
    }

    public async Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("{Service} is running.", nameof(HackerNewsHostedService));
        _timerTask = CacheBestNewsAsync(stoppingToken);
        await _hackerNewsService.FetchAndCacheNewsItemsAsync(_cts.Token);
    }

    private async Task CacheBestNewsAsync(object state)
    {
        try
        {
            while (await _timer.WaitForNextTickAsync(_cts.Token))
            {
                await _hackerNewsService.FetchAndCacheNewsItemsAsync(_cts.Token);
                _logger.LogInformation("{Service} is finished caching best news items", nameof(HackerNewsHostedService));
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("{Service} was cancelled.", nameof(HackerNewsHostedService));
        }
    }

    public async Task StopAsync(CancellationToken stoppingToken)
    {
        if (_timerTask is null)
        {
            return;
        }

        _cts.Cancel();
        await _timerTask;
        _cts.Dispose();
        _logger.LogInformation("{Service} is stopping.", nameof(HackerNewsHostedService));
    }
}