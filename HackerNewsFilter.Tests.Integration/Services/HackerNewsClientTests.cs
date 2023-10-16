using AutoFixture;
using FluentAssertions;
using HackerNewsFilter.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace HackerNewsFilter.Tests.Integration.Services;
public class HackerNewsClientTests
{
    private Fixture _fixture;
    public HackerNewsClientTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task CanGetAnyHackerNewsClientItem_WithSuccessResult()
    {
        //Arrange
        var newsItemId = _fixture.Create<int>();

        using var app = new TestApplicationFactory();
        var hackerNewsClient = app.Services.GetRequiredService<IHackerNewsClient>();

        //Act
        var newsItem = await hackerNewsClient.GetNewsItemByIdAsync(newsItemId, default);

        //Assert
        newsItem.Should().NotBeNull();
    }

    [Fact]
    public async Task CanGetHackerNewsItem_WithDescendents_WithSuccessResult()
    {
        //Arrange
        var newsItemId = 21233041;

        using var app = new TestApplicationFactory();
        var hackerNewsClient = app.Services.GetRequiredService<IHackerNewsClient>();

        //Act
        var newsItem = await hackerNewsClient.GetNewsItemByIdAsync(newsItemId, default);

        //Assert
        newsItem.Should().NotBeNull();
        newsItem.descendants.Should().NotBe(0);
        newsItem.id.Should().NotBe(0);
        newsItem.by.Should().NotBeNullOrWhiteSpace();
        newsItem.url.Should().NotBeNullOrWhiteSpace();
        newsItem.time.Should().NotBe(0);
    }

    [Fact]
    public async Task CanGetBestHackerNewsItems_WithSuccessResult()
    {
        //Arrange
        using var app = new TestApplicationFactory();
        var hackerNewsClient = app.Services.GetRequiredService<IHackerNewsClient>();

        //Act
        var newsItems = await hackerNewsClient.GetAllBestNewsItemsAsync(default);

        //Assert
        newsItems.Should().NotBeNull();
        newsItems.BestNewsIds.Should().NotBeNullOrEmpty();
    }
}
