namespace AkhshamBazari.Infrastructure.Data.Products.Repositories;

using Microsoft.EntityFrameworkCore;
using AkhshamBazari.Infrastructure.Data;
using AkhshamBazari.Core.Data.Products.Repositories;
using AkhshamBazari.Core.Data.Products.Models;

public class ProductRepository : IProductRepository
{
    private readonly AkhshamBazariDbContext dbContext;

    public ProductRepository(AkhshamBazariDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await this.dbContext.Products.ToListAsync();
    }

    public async Task InsertProductAsync(Product product) {
        await this.dbContext.Products.AddAsync(product);
        await this.dbContext.SaveChangesAsync();
    }
}