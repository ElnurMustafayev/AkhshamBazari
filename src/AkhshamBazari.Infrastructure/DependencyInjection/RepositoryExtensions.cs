namespace AkhshamBazari.Infrastructure.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

public static class RepositoryExtensions
{
    public static void InitRepositories(this IServiceCollection serviceCollection) {
        serviceCollection.AddScoped<
            Core.Data.Products.Repositories.IProductRepository, 
            Data.Products.Repositories.ProductRepository>();
    }
}