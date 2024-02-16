namespace AkhshamBazari.Core.Data.Products.Repositories;

using AkhshamBazari.Core.Data.Products.Models;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task InsertProductAsync(Product product);
}