using AutoFixture;
using DotnetDocsShow.Tests.Integration;
using FluentAssertions;
using System.Net;
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

        var fetchCount = _fixture.Create<int>();
        var httpClient = app.CreateClient();

        //Act
        var response = await httpClient.GetAsync($"news/{fetchCount}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
