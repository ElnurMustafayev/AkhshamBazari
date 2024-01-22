#pragma warning disable CS8618

namespace AkhshamBazari.Data;

using AkhshamBazari.Models;
using Microsoft.EntityFrameworkCore;

public class AkhshamBazariDbContext : DbContext {
    public DbSet<Product> Products { get; set; }

    public AkhshamBazariDbContext(DbContextOptions<AkhshamBazariDbContext> options) : base(options) {}
}