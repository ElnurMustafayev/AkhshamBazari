namespace AkhshamBazari.Services;

using System.Threading.Tasks;
using AkhshamBazari.Dtos;
using AkhshamBazari.Models;
using AkhshamBazari.Repositories.Base;
using AkhshamBazari.Services.Base;

public class ProductService : IProductService
{
    private readonly IProductRepository productRepository;

    public ProductService(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task AddNewProductAsync(ProductDto productDto)
    {
        var product = new Product {
            Name = productDto.Name,
            Price = productDto.Price,
        };

        if(product.Price < 0) {
            throw new ArgumentException("Price can not be less than 0!");
        }

        else if(string.IsNullOrWhiteSpace(product.Name)) {
            throw new ArgumentException("Name can not be Empty!");
        }

        await this.productRepository.InsertProductAsync(product);
    }
}