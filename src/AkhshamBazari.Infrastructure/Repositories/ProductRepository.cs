namespace AkhshamBazari.Infrastructure.Repositories;

using AkhshamBazari.Core.Models;
using Microsoft.EntityFrameworkCore;
using AkhshamBazari.Core.Repositories;
using AkhshamBazari.Infrastructure.Data;

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