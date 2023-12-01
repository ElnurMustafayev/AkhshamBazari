namespace AkhshamBazari.Controllers;

using System.Data.SqlClient;
using System.Net;
using System.Text.Json;
using AkhshamBazari.Controllers.Base;
using AkhshamBazari.Models;
using AkhshamBazari.Extensions;
using Dapper;
using AkhshamBazari.Attributes;

public class ProductController : ControllerBase
{
    private const string ConnectionString = "Server=localhost;Database=AkhshamBazariDb;User Id=sa;Password=Admin9264!!;";

    [HttpGet("GetAll")]
    public async Task GetProductsAsync(HttpListenerContext context)
    {
        using var writer = new StreamWriter(context.Response.OutputStream);

        using var connection = new SqlConnection(ConnectionString);
        var products = await connection.QueryAsync<Product>("select * from Products");

        var productsHtml = products.GetHtml();
        await writer.WriteLineAsync(productsHtml);
        context.Response.ContentType = "text/html";

        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpGet("GetById")]
    public async Task GetProductByIdAsync(HttpListenerContext context)
    {
        var productIdToGetObj = context.Request.QueryString["id"];

        if (productIdToGetObj == null || int.TryParse(productIdToGetObj, out int productIdToGet) == false)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var product = await connection.QueryFirstOrDefaultAsync<Product>(
            sql: "select top 1 * from Products where Id = @Id",
            param: new { Id = productIdToGet });

        if (product is null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        using var writer = new StreamWriter(context.Response.OutputStream);
        await writer.WriteLineAsync(JsonSerializer.Serialize(product));

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }



    [HttpPost("Create")]
    public async Task PostProductAsync(HttpListenerContext context)
    {
        using var reader = new StreamReader(context.Request.InputStream);
        var json = await reader.ReadToEndAsync();

        var newProduct = JsonSerializer.Deserialize<Product>(json);

        if (newProduct == null || newProduct.Price == null || string.IsNullOrWhiteSpace(newProduct.Name))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var products = await connection.ExecuteAsync(
            @"insert into Products (Name, Price) 
        values(@Name, @Price)",
            param: new
            {
                newProduct.Name,
                newProduct.Price,
            });

        context.Response.StatusCode = (int)HttpStatusCode.Created;
    }

    [HttpDelete]
    public async Task DeleteProductAsync(HttpListenerContext context)
    {
        var productIdToDeleteObj = context.Request.QueryString["id"];

        if (productIdToDeleteObj == null || int.TryParse(productIdToDeleteObj, out int productIdToDelete) == false)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var deletedRowsCount = await connection.ExecuteAsync(
            @"delete Products
        where Id = @Id",
            param: new
            {
                Id = productIdToDelete,
            });

        if (deletedRowsCount == 0)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpPut]
    public async Task PutProductAsync(HttpListenerContext context)
    {
        var productIdToUpdateObj = context.Request.QueryString["id"];

        if (productIdToUpdateObj == null || int.TryParse(productIdToUpdateObj, out int productIdToUpdate) == false)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var reader = new StreamReader(context.Request.InputStream);
        var json = await reader.ReadToEndAsync();

        var productToUpdate = JsonSerializer.Deserialize<Product>(json);

        if (productToUpdate == null || productToUpdate.Price == null || string.IsNullOrEmpty(productToUpdate.Name))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var affectedRowsCount = await connection.ExecuteAsync(
            @"update Products
        set Name = @Name, Price = @Price
        where Id = @Id",
            param: new
            {
                productToUpdate.Name,
                productToUpdate.Price,
                Id = productIdToUpdate
            });

        if (affectedRowsCount == 0)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }
}