namespace AkhshamBazari.Controllers;

using System.Data.SqlClient;
using System.Net;
using System.Text.Json;
using AkhshamBazari.Controllers.Base;
using AkhshamBazari.Models;
using Dapper;
using AkhshamBazari.Extensions;
using AkhshamBazari.Attributes;

public class ProductController : ControllerBase
{
    private const string ConnectionString = "Server=localhost;Database=AkhshamBazariDb;User Id=sa;Password=Admin9264!!;";

    [HttpGet("GetAll")]
    public async Task GetProductsAsync()
    {
        using var writer = new StreamWriter(base.HttpContext.Response.OutputStream);

        using var connection = new SqlConnection(ConnectionString);
        var products = await connection.QueryAsync<Product>("select * from Products");

        var productsHtml = products.GetHtml();
        await writer.WriteLineAsync(productsHtml);
        base.HttpContext.Response.ContentType = "text/html";

        base.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpGet("GetById")]
    public async Task GetProductByIdAsync()
    {
        var productIdToGetObj = base.HttpContext.Request.QueryString["id"];

        if (productIdToGetObj == null || int.TryParse(productIdToGetObj, out int productIdToGet) == false)
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var product = await connection.QueryFirstOrDefaultAsync<Product>(
            sql: "select top 1 * from Products where Id = @Id",
            param: new { Id = productIdToGet });

        if (product is null)
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        using var writer = new StreamWriter(base.HttpContext.Response.OutputStream);
        await writer.WriteLineAsync(JsonSerializer.Serialize(product));

        base.HttpContext.Response.ContentType = "application/json";
        base.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
    }



    [HttpPost("Create")]
    public async Task PostProductAsync()
    {
        using var reader = new StreamReader(base.HttpContext.Request.InputStream);
        var json = await reader.ReadToEndAsync();

        var newProduct = JsonSerializer.Deserialize<Product>(json);

        if (newProduct == null || newProduct.Price == null || string.IsNullOrWhiteSpace(newProduct.Name))
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
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

        base.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
    }

    [HttpDelete]
    public async Task DeleteProductAsync()
    {
        var productIdToDeleteObj = base.HttpContext.Request.QueryString["id"];

        if (productIdToDeleteObj == null || int.TryParse(productIdToDeleteObj, out int productIdToDelete) == false)
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
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
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        base.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpPut]
    public async Task PutProductAsync()
    {
        var productIdToUpdateObj = base.HttpContext.Request.QueryString["id"];

        if (productIdToUpdateObj == null || int.TryParse(productIdToUpdateObj, out int productIdToUpdate) == false)
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var reader = new StreamReader(base.HttpContext.Request.InputStream);
        var json = await reader.ReadToEndAsync();

        var productToUpdate = JsonSerializer.Deserialize<Product>(json);

        if (productToUpdate == null || productToUpdate.Price == null || string.IsNullOrEmpty(productToUpdate.Name))
        {
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
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
            base.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        base.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
    }
}