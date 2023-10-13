using DotnetDocsShow.Tests.Integration;
using HackerNewsFilter.Structured.Mediator.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace HackerNewsFilter.Tests.Integration.Services;
public class HackerNewsClientTests
{
    [Fact]
    public async Task CanGetHackerNewsClientItem_WithSuccessResult()
    {
        using var app = new TestApplicationFactory();

        var hackerNewsClient = app.Services.GetRequiredService<IHackerNewsClient>();
        var response = await hackerNewsClient.GetNewsItemByIdAsync(21233229);

        Assert.NotNull(hackerNewsClient);
    }

    [Fact]
    public async Task CanGetBestHackerNewsItems_WithSuccessResult()
    {
        using var app = new TestApplicationFactory();

        var hackerNewsClient = app.Services.GetRequiredService<IHackerNewsClient>();

        var response = await hackerNewsClient.GetBestNewsItemsAsync();
        Assert.NotNull(hackerNewsClient);
    }
}
