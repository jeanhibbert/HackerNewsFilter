using AutoFixture;
using HackerNewsFilter.Tests.Integration;
using FastEndpoints;
using FluentAssertions;
using HackerNewsFilter.Api.Contracts.Responses;
using HackerNewsFilter.Tests.Integration.Extensions;
using System.Collections.Generic;
using System.Linq;
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
    public async Task CanGetBestNews_WillReturnOk_WhenNewsItemsExist()
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
        var newsItemsResultList = JsonSerializer.Deserialize<List<NewsItemResult>>(responseText);
        newsItemsResultList.Should().NotBeNullOrEmpty();
        newsItemsResultList.Should().HaveCount(limit);
    }

    [Fact]
    public async Task CanGetBestNewsMultipleTimes_WillReturnOk_WhenNewsItemsExist()
    {
        //Arrange
        using var app = new TestApplicationFactory();

        var request1Limit = _fixture.CreateInt(20, 40);
        var request2Limit = _fixture.CreateInt(50, 80);

        var httpClient = app.CreateClient();

        //Act
        var response1 = await httpClient.GetAsync($"news?limit={request1Limit}");
        var response2 = await httpClient.GetAsync($"news?limit={request2Limit}");

        //Assert
        response1.StatusCode.Should().Be(HttpStatusCode.OK);
        var response1Text = await response1.Content.ReadAsStringAsync();
        var newsItemsResultList1 = JsonSerializer.Deserialize<List<NewsItemResult>>(response1Text);
        newsItemsResultList1.Should().NotBeNullOrEmpty();
        newsItemsResultList1.Should().HaveCount(request1Limit);

        response2.StatusCode.Should().Be(HttpStatusCode.OK);
        var response2Text = await response1.Content.ReadAsStringAsync();
        var newsItemsResultList2 = JsonSerializer.Deserialize<List<NewsItemResult>>(response2Text);
        newsItemsResultList2.Should().NotBeNullOrEmpty();
        newsItemsResultList2.Should().HaveCount(request1Limit);
    }

    [Fact]
    public async Task GetBestNews_WillReturnErrorDetails_WhenValidationFails()
    {
        //Arrange
        using var app = new TestApplicationFactory();

        var limit = -1;
        var httpClient = app.CreateClient();

        //Act
        var response = await httpClient.GetAsync($"news?limit={limit}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseText = await response.Content.ReadAsStringAsync();
        var validationResult = JsonSerializer.Deserialize<ErrorResponse>(responseText, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        validationResult.Message.Should().Be("One or more errors occurred!");
        validationResult.Errors.Should().HaveCount(1);
        validationResult.Errors.Single().Key.Should().Be("limit");
        validationResult.Errors.Single().Value.Single().Should().Be("limit must be greater than zero");
    }
}
