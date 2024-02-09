namespace AkhshamBazari.Infrastructure.Services;

using System.Threading.Tasks;
using AkhshamBazari.Core.Models;
using AkhshamBazari.Core.Repositories;
using AkhshamBazari.Core.Services.Base;

public class ProductService : IProductService
{
    private readonly IProductRepository productRepository;

    public ProductService(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task AddNewProductAsync(Product product)
    {
        if(product.Price < 0) {
            throw new ArgumentException("Price can not be less than 0!");
        }

        else if(string.IsNullOrWhiteSpace(product.Name)) {
            throw new ArgumentException("Name can not be Empty!");
        }

        await this.productRepository.InsertProductAsync(product);
    }
}