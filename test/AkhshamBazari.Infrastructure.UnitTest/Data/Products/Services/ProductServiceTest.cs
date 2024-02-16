#pragma warning disable CS8625

namespace AkhshamBazari.Infrastructure.UnitTest.Data.Products.Services;

using AkhshamBazari.Core.Data.Products.Models;
using AkhshamBazari.Core.Data.Products.Repositories;
using AkhshamBazari.Infrastructure.Data.Products.Services;

public class ProductServiceTest
{
    private const string correctName = "Test";
    private const double correctPrice = 123;

    private readonly Product correctProduct = new()
    {
        Name = correctName,
        Price = correctPrice
    };

    [Fact]
    public async Task AddNewProductAsync_SendCorrectProduct_Works()
    {
        var productRepositoryMock = new Mock<IProductRepository>();
        //productRepositoryMock.Setup(repository => repository.InsertProductAsync(new Product()));

        var service = new ProductService(productRepositoryMock.Object);

        await service.AddNewProductAsync(this.correctProduct);
    }

    [Theory]
    [InlineData(correctName, -10.0)]
    [InlineData("  ", correctPrice)]
    [InlineData("", correctPrice)]
    [InlineData(null, correctPrice)]
    public async Task AddNewProductAsync_SendIncorrectProduct_ThrowsArgumentException(string? name, double? price)
    {
        var service = new ProductService(null);

        var newProduct = new Product
        {
            Name = name,
            Price = price,
        };

        await Assert.ThrowsAsync<ArgumentException>(async () => await service.AddNewProductAsync(newProduct));
    }



    [Fact]
    public async Task GetAllAsync_IfAny_ReturnsAny()
    {
        var repositoryReturningData = new Product[] {
                this.correctProduct,
                this.correctProduct,
                this.correctProduct
            };

        var productRepositoryMock = new Mock<IProductRepository>();

        productRepositoryMock.Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(repositoryReturningData);

        var service = new ProductService(productRepositoryMock.Object);

        var result = await service.GetAllProductsAsync();

        Assert.Equal(repositoryReturningData.Length, result.Count());
    }



    [Fact]
    public async Task GetAllAsync_IfEmpty_ReturnsNull()
    {
        var productRepositoryMock = new Mock<IProductRepository>();

        productRepositoryMock.Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(Enumerable.Empty<Product>());

        var service = new ProductService(productRepositoryMock.Object);

        var result = await service.GetAllProductsAsync();
        
        Assert.Null(result);
    }
}