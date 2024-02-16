namespace AkhshamBazari.Infrastructure.Data.Products.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using AkhshamBazari.Core.Data.Products.Models;
using AkhshamBazari.Core.Data.Products.Repositories;
using AkhshamBazari.Core.Data.Products.Services;

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

    public async Task<IEnumerable<Product>?> GetAllProductsAsync()
    {
        var result = await this.productRepository.GetAllAsync();

        if(result == null || result.Any() == false)
            return null;

        return result;
    }
}