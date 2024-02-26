namespace AkhshamBazari.Infrastructure.DependencyInjection;

using System.Reflection;
using AkhshamBazari.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DbContextExtensions
{
    public static void InitDbContext(this IServiceCollection serviceCollection, IConfiguration configuration, Assembly migrationsAssembly) {
        serviceCollection.AddDbContext<AkhshamBazariDbContext>(options => {
            const string connectionStringKey = "AkhshamBazariDb";
            var connectionString = configuration.GetConnectionString(connectionStringKey);

            if(string.IsNullOrWhiteSpace(connectionString)) {
                #pragma warning disable CA2208
                throw new ArgumentNullException(paramName: nameof(connectionString),
                                                message: $"EF Connection strign is empty. Key '{connectionStringKey}' not found!");
                #pragma warning restore 
            }

            options.UseSqlServer(connectionString, useSqlOptions => {
                useSqlOptions.MigrationsAssembly(migrationsAssembly.FullName);
            });
        });

        serviceCollection.AddIdentityCore<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AkhshamBazariDbContext>();
    }
}