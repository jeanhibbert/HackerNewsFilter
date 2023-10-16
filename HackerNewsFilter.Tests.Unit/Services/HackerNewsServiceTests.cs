using AutoFixture;
using FluentAssertions;
using HackerNewsFilter.Api.Models;
using HackerNewsFilter.Api.Services;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HackerNewsFilter.Tests.Unit.Services;

public class HackerNewsServiceTests
{
    private readonly IHackerNewsClient _hackerNewsClient 
        = Substitute.For<IHackerNewsClient>();

    private readonly IHackerNewsService _sut;

    private Fixture _fixture;

    public HackerNewsServiceTests()
    {
        _fixture = new Fixture();

        _sut = new HackerNewsService(_hackerNewsClient, 
            new MemoryCache(new MemoryCacheOptions()));
    }

    [Fact]
    public async Task GetBestNewsItemsAsync_ReturnsCorrectLimit_WhenLimitProvided()
    {
        //Arrange
        _hackerNewsClient
            .GetAllBestNewsItemsAsync(default)
            .Returns(new BestNewsItems 
            { 
                BestNewsIds = new List<int> { 1, 2 } 
            });

        _hackerNewsClient
            .GetNewsItemByIdAsync(1, default)
            .Returns(Task.FromResult(new NewsItem { id = 1, score = 10 }));

        _hackerNewsClient
            .GetNewsItemByIdAsync(2, default)
            .Returns(Task.FromResult(new NewsItem { id = 2, score = 11 }));

        //Act
        var limit = 1;
        var bestNewsItems = await _sut.GetBestNewsItemsAsync(limit);

        //Assert
        bestNewsItems.Should().NotBeNullOrEmpty();
        bestNewsItems.Should().HaveCount(1);
        var firstNewsItem = bestNewsItems.Single();
        firstNewsItem.score.Should().Be(11);
        firstNewsItem.id.Should().Be(2);
    }

    [Fact]
    public async Task GetBestNewsItemsAsync_WillFail_WhenHackerNewsClientThrowsAnException()
    {
        //Arrange
        _hackerNewsClient
            .GetAllBestNewsItemsAsync(default)
            .Returns(new BestNewsItems
            {
                BestNewsIds = _fixture.Create<List<int>>()
            });

        _hackerNewsClient
            .GetNewsItemByIdAsync(Arg.Any<int>(), CancellationToken.None)
            .ThrowsForAnyArgs(_ => throw new HttpRequestException());

        //Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => _sut.GetBestNewsItemsAsync(_fixture.Create<int>()));
    }
}
