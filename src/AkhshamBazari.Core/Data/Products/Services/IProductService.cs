namespace AkhshamBazari.Core.Data.Products.Services;

using AkhshamBazari.Core.Data.Products.Models;

public interface IProductService
{
    Task AddNewProductAsync(Product product);
    Task<IEnumerable<Product>?> GetAllProductsAsync();
}