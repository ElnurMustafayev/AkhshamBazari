using System.Reflection;
using AkhshamBazari.Core.Models;
using AkhshamBazari.Core.Repositories;
using AkhshamBazari.Core.Services.Base;
using AkhshamBazari.Infrastructure.Data;
using AkhshamBazari.Infrastructure.Repositories;
using AkhshamBazari.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AkhshamBazariDbContext>(options => {
    const string connectionStringKey = "AkhshamBazariDb";
    var connectionString = builder.Configuration.GetConnectionString(connectionStringKey);

    if(string.IsNullOrWhiteSpace(connectionString)) {
        #pragma warning disable CA2208
        throw new ArgumentNullException(paramName: nameof(connectionString),
                                        message: $"EF Connection strign is empty. Key '{connectionStringKey}' not found!");
        #pragma warning restore 
    }

    //options.UseSqlServer(connectionString);

    options.UseSqlServer(connectionString, useSqlOptions => {
        useSqlOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
    });
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");

app.UseStaticFiles();

app.UseRouting();

app.MapDefaultControllerRoute();

app.Run();