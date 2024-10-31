using APICatalogo.Controllers;
using APICatalogo.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoxUnitTestes.UnitTests;

public class GetProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public GetProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.mapper);
    }

    [Fact]
    public async Task GetProdutoById_OkResult()
    {
        //Arrange 
        var prodId = 2;

        //Act
        var data = await _controller.Get(prodId);

        //var okresult = Assert.IsType<OkObjectResult>(data.Result);
        //Assert.Equal(200, okresult.StatusCode);

        //Assert (fluentassertions)
        data.Result.Should().BeOfType<OkObjectResult>()
                            .Which.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetProdutoById_Return_NotFound()
    {
        //Arrange 
        var prodId = 999;

        //Act
        var data = await _controller.Get(prodId);

        //Assert
        data.Result.Should().BeOfType<NotFoundObjectResult>()
                            .Which.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetProdutoById_Return_BadRequest()
    {
        //Arrange 
        var prodId = 1;

        //Act
        var data = await _controller.Get(prodId);

        //Assert
        data.Result.Should().BeOfType<BadRequestObjectResult>()
                            .Which.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task GetProdutoById_Return_ListOfProdutoDTO()
    {
        //Act
        var data = await _controller.GetAsync();

        //Assert
        data.Result.Should().BeOfType<OkObjectResult>()
                            .Which.Value.Should().BeAssignableTo<IEnumerable<ProdutoDTO>>()
                            .And.NotBeNull();
    }
    [Fact]
    public async Task GetProdutoById_Return_BadResquestResult()
    {
        //Act
        var data = await _controller.GetAsync();

        //Assert
        data.Result.Should().BeOfType<BadRequestResult>();
    }
}
