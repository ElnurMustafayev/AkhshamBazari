namespace AkhshamBazari.Infrastructure.DependencyInjection;

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

public static class MediatRExtensions
{
    public static void InitMediatR(this IServiceCollection serviceCollection) {
        serviceCollection.AddMediatR(configurations => {
            configurations.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
    }
}