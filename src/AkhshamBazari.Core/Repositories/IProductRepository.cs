namespace AkhshamBazari.Core.Repositories;

using AkhshamBazari.Core.Models;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task InsertProductAsync(Product product);
}