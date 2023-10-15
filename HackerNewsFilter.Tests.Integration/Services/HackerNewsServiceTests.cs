using AutoFixture;
using DotnetDocsShow.Api.Services;
using DotnetDocsShow.Tests.Integration;
using FluentAssertions;
using HackerNewsFilter.Tests.Integration.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HackerNewsFilter.Tests.Integration.Services;
public class HackerNewsServiceTests
{
    private Fixture _fixture;

    public HackerNewsServiceTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task CanGetBestNewsItems_WillReturnItemsRequested()
    {
        //Arrange
        var fetchCount = _fixture.CreateInt(1, 40);

        using var app = new TestApplicationFactory();
        var hackerNewsService = app.Services.GetRequiredService<IHackerNewsService>();

        //Act
        var firstResponse = await hackerNewsService.GetBestNewsItemsAsync(fetchCount);
        fetchCount++;
        var secondResponse = await hackerNewsService.GetBestNewsItemsAsync(fetchCount);

        //Assert
        firstResponse.Should().NotBeNull();
        secondResponse.Should().NotBeNull();
        secondResponse.Count.Should().Be(firstResponse.Count + 1);
    }

    [Fact]
    public async Task CanGetBestNewsItems_WillOrderTheNewsItemsCorrectly()
    {
        //Arrange
        var fetchCount = _fixture.CreateInt(1, 40);

        using var app = new TestApplicationFactory();
        var hackerNewsService = app.Services.GetRequiredService<IHackerNewsService>();

        //Act
        var response = await hackerNewsService.GetBestNewsItemsAsync(fetchCount);

        //Assert
        response.Should().NotBeNull();
        response.Count.Should().Be(fetchCount);
        var expectedList = response.OrderByDescending(x => x.score);
        response.SequenceEqual(expectedList);
    }
}
