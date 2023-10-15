using AutoFixture;
using DotnetDocsShow.Tests.Integration;
using FluentAssertions;
using HackerNewsFilter.Structured.Mediator.Services;
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
        var response = await hackerNewsClient.GetNewsItemByIdAsync(newsItemId);

        //Assert
        response.Should().NotBeNull();
    }

    [Fact]
    public async Task CanGetHackerNewsItem_WithDescendents_WithSuccessResult()
    {
        //Arrange
        var newsItemId = 21233041;

        using var app = new TestApplicationFactory();
        var hackerNewsClient = app.Services.GetRequiredService<IHackerNewsClient>();

        //Act
        var response = await hackerNewsClient.GetNewsItemByIdAsync(newsItemId);

        //Assert
        response.Should().NotBeNull();
        response.descendants.Should().NotBe(0);
        response.id.Should().NotBe(0);
        response.by.Should().NotBeNullOrWhiteSpace();
        response.url.Should().NotBeNullOrWhiteSpace();
        response.time.Should().NotBe(0);
    }

    [Fact]
    public async Task CanGetBestHackerNewsItems_WithSuccessResult()
    {
        //Arrange
        using var app = new TestApplicationFactory();
        var hackerNewsClient = app.Services.GetRequiredService<IHackerNewsClient>();

        //Act
        var response = await hackerNewsClient.GetAllBestNewsItemsAsync();

        //Assert
        response.Should().NotBeNull();
        response.BestNewsIds.Should().NotBeNullOrEmpty();
    }
}
