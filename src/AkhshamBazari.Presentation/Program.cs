using AkhshamBazari.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.InitRepositories();
builder.Services.InitDbContext(builder.Configuration, System.Reflection.Assembly.GetExecutingAssembly());
builder.Services.InitMediatR();

var app = builder.Build();

app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");

app.UseStaticFiles();

app.UseRouting();

app.MapDefaultControllerRoute();

app.Run();