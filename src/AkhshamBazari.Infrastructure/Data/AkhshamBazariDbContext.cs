#pragma warning disable CS8618

namespace AkhshamBazari.Infrastructure.Data;

using AkhshamBazari.Core.Data.Products.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AkhshamBazariDbContext : IdentityDbContext<IdentityUser, IdentityRole, string> {
    public DbSet<Product> Products { get; set; }

    public AkhshamBazariDbContext(DbContextOptions<AkhshamBazariDbContext> options) : base(options) {}
}