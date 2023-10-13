using AutoFixture;
using DotnetDocsShow.Tests.Integration;
using FluentAssertions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace HackerNewsFilter.Tests.Integration.Endpoints;

public class BroadNewsItemEndpointsTests
{
    private Fixture _fixture;

    public BroadNewsItemEndpointsTests()
    { 
        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetNewsItemById_ReturnOk_WhenNewsItemDoesExist()
    {
        //Arrange
        using var app = new TestApplicationFactory();

        var id = 37850265;
        var httpClient = app.CreateClient();

        //Act
        var response = await httpClient.GetAsync($"/news/{id}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetNewsItemById_ReturnNotFound_WhenNewsItemDoesNotExists()
    {
        //Arrange
        using var app = new TestApplicationFactory();

        var id = _fixture.Create<long>();
        var httpClient = app.CreateClient();

        //Act
        var response = await httpClient.GetAsync($"/news/{id}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
