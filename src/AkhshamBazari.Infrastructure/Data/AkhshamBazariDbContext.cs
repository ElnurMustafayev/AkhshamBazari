#pragma warning disable CS8618

namespace AkhshamBazari.Infrastructure.Data;

using AkhshamBazari.Core.Data.Products.Models;
using Microsoft.EntityFrameworkCore;

public class AkhshamBazariDbContext : DbContext {
    public DbSet<Product> Products { get; set; }

    public AkhshamBazariDbContext(DbContextOptions<AkhshamBazariDbContext> options) : base(options) {}
    // public AkhshamBazariDbContext() {}

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     string connectionString = "Server=localhost;Database=AkhshamBazariDb;User Id=sa;Password=Admin9264!!;TrustServerCertificate=True;";
    //     optionsBuilder.UseSqlServer(connectionString);
    //     base.OnConfiguring(optionsBuilder);
    // }
}