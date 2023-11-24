using System.Data.Common;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Text.Json;
using AkhshamBazari.Models;
using Dapper;


const string connectionString = "Server=localhost;Database=AkhshamBazariDb;User Id=sa;Password=Admin9264!!;";
HttpListener httpListener = new HttpListener();

const int port = 8080;
httpListener.Prefixes.Add($"http://*:{port}/");

httpListener.Start();


string GetHtml<T>(IEnumerable<T> products)
{
    Type type = typeof(T);

    var props = type.GetProperties();

    StringBuilder sb = new StringBuilder(100);
    sb.Append("<ul>");

    foreach (var product in products)
    {
        foreach (var prop in props)
        {
            sb.Append($"<li><span>{prop.Name}: </span>{prop.GetValue(product)}</li>");
        }
        sb.Append("<br/>");
    }
    sb.Append("</ul>");

    return sb.ToString();
}


System.Console.WriteLine($"Server started on port {port}...");

while (true)
{
    var context = await httpListener.GetContextAsync();
    using var writer = new StreamWriter(context.Response.OutputStream);

    // GET: /products
    // GET: /product?id=123
    // POST: /product Request Body
    // PUT: /product?id=123 Request Body
    // DELETE: /product?id=123

    var endpointItems = context.Request.RawUrl?.Split("/", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

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
        // http://localhost:8080/products
        case "products":
            if (context.Request.HttpMethod == HttpMethod.Get.ToString())
            {
                using var connection = new SqlConnection(connectionString);
                var products = await connection.QueryAsync<Product>("select * from Products");

                var productsHtml = GetHtml(products);
                await writer.WriteLineAsync(productsHtml);
                context.Response.ContentType = "text/html";

                // var productsJson = JsonSerializer.Serialize(products);
                // await writer.WriteLineAsync(productsJson);
                // context.Response.ContentType = "application/json";

                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            else {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            }


            break;
    }
}