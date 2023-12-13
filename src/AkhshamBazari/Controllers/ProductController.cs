namespace AkhshamBazari.Controllers;

using System.Data.SqlClient;
using AkhshamBazari.Dtos;
using AkhshamBazari.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;

[Route("/[controller]")]
public class ProductController : Controller
{
    private const string ConnectionString = "Server=localhost;Database=AkhshamBazariDb;User Id=sa;Password=Admin9264!!;";
    
    public ProductController() {}

    [HttpGet]
    [ActionName("Index")]
    public async Task<IActionResult> ShowAllProducts()
    {
        using var connection = new SqlConnection(ConnectionString);
        
        var products = await connection.QueryAsync<Product>("select * from Products");

        return View(model: products);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody]ProductDto dto) {
        using var connection = new SqlConnection(ConnectionString);
        
        var products = await connection.ExecuteAsync(
            sql: "insert into Products (Name, Price) values (@Name, @Price);",
            param: new { dto.Name, dto.Price });

        return Created(base.Url.ActionLink()!, "Success!");
    }
}