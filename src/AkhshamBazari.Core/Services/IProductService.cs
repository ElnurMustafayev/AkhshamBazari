namespace AkhshamBazari.Core.Services.Base;

using AkhshamBazari.Core.Models;

public interface IProductService
{
    Task AddNewProductAsync(Product product);
}