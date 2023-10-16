using AutoFixture;
using DotnetDocsShow.Tests.Integration;
using FluentAssertions;
using HackerNewsFilter.Api.Contracts.Responses;
using HackerNewsFilter.Tests.Integration.Extensions;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace HackerNewsFilter.Tests.Integration.Endpoints;

public class GetNewsItemEndpointsTests
{
    private Fixture _fixture;

    public GetNewsItemEndpointsTests()
    { 
        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetNewsItemById_ReturnOk_WhenNewsItemDoesExist()
    {
        //Arrange
        using var app = new TestApplicationFactory();

        var limit = _fixture.CreateInt(20, 40);
        var httpClient = app.CreateClient();

        //Act
        var response = await httpClient.GetAsync($"news?limit={limit}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseText = await response.Content.ReadAsStringAsync();
        var newsItemsResult = JsonSerializer.Deserialize<GetBestNewsItemsResponse>(responseText);
        newsItemsResult.BestNewsItems.Should().NotBeNullOrEmpty();
        newsItemsResult.BestNewsItems.Should().HaveCount(limit);
    }

    [Fact]
    public async Task GetNewsItemById_ReturnBadRequest_WhenValidationFails()
    {
        //Arrange
        using var app = new TestApplicationFactory();

        var limit = -1;
        var httpClient = app.CreateClient();

        //Act
        var response = await httpClient.GetAsync($"news?limit={limit}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
