using AkhshamBazari.Dtos;

namespace AkhshamBazari.Services.Base;

public interface IProductService
{
    Task AddNewProductAsync(ProductDto productDto);
}