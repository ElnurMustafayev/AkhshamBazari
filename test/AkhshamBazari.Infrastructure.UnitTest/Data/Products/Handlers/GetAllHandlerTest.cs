using AkhshamBazari.Core.Data.Products.Models;
using AkhshamBazari.Core.Data.Products.Repositories;
using AkhshamBazari.Infrastructure.Data.Products.Commands;
using AkhshamBazari.Infrastructure.Data.Products.Handlers;

namespace AkhshamBazari.Infrastructure.UnitTest.Data.Products.Handlers;

public class GetAllHandlerTest
{
    private const string correctName = "Test";
    private const double correctPrice = 123;

    private readonly Product correctProduct = new()
    {
        Name = correctName,
        Price = correctPrice
    };

    [Fact]
    public async Task Handle_IfAny_ReturnsAny()
    {
        var repositoryReturningData = new Product[] {
                this.correctProduct,
                this.correctProduct,
                this.correctProduct
            };

        var productRepositoryMock = new Mock<IProductRepository>();

        productRepositoryMock.Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(repositoryReturningData);

        var handler = new GetAllHandler(productRepositoryMock.Object);

        var result = await handler.Handle(new GetAllCommand(), CancellationToken.None);

        Assert.Equal(repositoryReturningData.Length, result.Count());
    }



    [Fact]
    public async Task Handle_IfRepoReturnsEmptyOrNull_ReturnsEmpty()
    {
        var productRepositoryMock = new Mock<IProductRepository>();

        productRepositoryMock.Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(value: null);

        var handler = new GetAllHandler(productRepositoryMock.Object);

        var result = await handler.Handle(new GetAllCommand(), CancellationToken.None);

        Assert.NotNull(result);
    }
}