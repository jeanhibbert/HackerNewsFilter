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
        var newsItemsResultList = JsonSerializer.Deserialize<List<NewsItemResult>>(responseText);
        newsItemsResultList.Should().NotBeNullOrEmpty();
        newsItemsResultList.Should().HaveCount(limit);
    }

    [Fact]
    public async Task GetNewsItemById_ReturnErrorDetails_WhenValidationFails()
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
