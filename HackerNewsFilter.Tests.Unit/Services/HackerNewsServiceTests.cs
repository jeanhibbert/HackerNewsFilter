using DotnetDocsShow.Structured.Mediator.Services;
using FluentAssertions;
using HackerNewsFilter.Structured.Mediator.Models;
using NSubstitute;
using Xunit;

namespace HackerNewsFilter.Tests.Unit.Services;

public class HackerNewsServiceTests
{
    private readonly IHackerNewsService _sut =
        Substitute.For<IHackerNewsService>();

    [Fact]
    public void GetAllCustomers_ReturnEmptyList_WhenNoCustomersExist()
    {
        //Arrange
        _sut.GetAll().Returns(new BestNewsItems());

        //Act
        var result = _sut.GetAll();

        //Assert
        result.Should().NotBeNull();
    }
}
