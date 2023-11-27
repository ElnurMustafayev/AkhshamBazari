using System.Data.SqlClient;
using System.Net;
using System.Text.Json;
using AkhshamBazari.Models;
using AkhshamBazari.Services;
using Dapper;

const string connectionString = "Server=localhost;Database=AkhshamBazariDb;User Id=sa;Password=Admin9264!!;";

async Task GetProductsAsync(HttpListenerContext context) {
    using var writer = new StreamWriter(context.Response.OutputStream);

    using var connection = new SqlConnection(connectionString);
    var products = await connection.QueryAsync<Product>("select * from Products");

    var productsHtml = products.GetHtml();
    await writer.WriteLineAsync(productsHtml);
    context.Response.ContentType = "text/html";

    // var productsJson = JsonSerializer.Serialize(products);
    // await writer.WriteLineAsync(productsJson);
    // context.Response.ContentType = "application/json";

    context.Response.StatusCode = (int)HttpStatusCode.OK;
}

async Task PostProductAsync(HttpListenerContext context) {
    using var reader = new StreamReader(context.Request.InputStream);
    var json = await reader.ReadToEndAsync();

    var newProduct = JsonSerializer.Deserialize<Product>(json);

    if (newProduct == null || newProduct.Price == null || string.IsNullOrWhiteSpace(newProduct.Name))
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return;
    }

    using var connection = new SqlConnection(connectionString);
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

async Task DeleteProductAsync(HttpListenerContext context) {
    var productIdToDeleteObj = context.Request.QueryString["id"];

    if (productIdToDeleteObj == null || int.TryParse(productIdToDeleteObj, out int productIdToDelete) == false)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return;
    }

    using var connection = new SqlConnection(connectionString);
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

async Task PutProductAsync(HttpListenerContext context) {
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

    using var connection = new SqlConnection(connectionString);
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


HttpListener httpListener = new HttpListener();

const int port = 8080;
httpListener.Prefixes.Add($"http://*:{port}/");

httpListener.Start();

System.Console.WriteLine($"Server started on port {port}...");

while (true)
{
    var context = await httpListener.GetContextAsync();
    using var writer = new StreamWriter(context.Response.OutputStream);

    var endpointItems = context.Request.Url.AbsolutePath?.Split("/", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    // http://localhost:8080/
    if (endpointItems == null || endpointItems.Length == 0)
    {
        // home page
        var pageHtml = await File.ReadAllTextAsync("Views/Home.html");
        await writer.WriteLineAsync(pageHtml);
        context.Response.StatusCode = (int)HttpStatusCode.OK;
        context.Response.ContentType = "text/html";

        continue;
    }

    switch (endpointItems[0].ToLower())
    {
        case "products":
            // GET: http://localhost:8080/products
            if (context.Request.HttpMethod == HttpMethod.Get.ToString())
            {
                await GetProductsAsync(context);
            }
            // POST: http://localhost:8080/products
            else if (context.Request.HttpMethod == HttpMethod.Post.ToString() && context.Request.ContentType == "application/json")
            {
                await PostProductAsync(context);
            }
            // DELETE: http://localhost:8080/products?id=123
            else if (context.Request.HttpMethod == HttpMethod.Delete.ToString())
            {
                await DeleteProductAsync(context);
            }
            // PUT: http://localhost:8080/products?id=123
            else if (context.Request.HttpMethod == HttpMethod.Put.ToString() && context.Request.ContentType == "application/json")
            {
                await PutProductAsync(context);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            }


            break;
    }
}