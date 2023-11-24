using System.Net;
using System.Text;
using System.Text.Json;
using AkhshamBazari.Models;

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
            if (context.Request.HttpMethod == HttpMethod.Get.ToString())
            {
                var products = new List<Product> {
                    new Product {
                        Id = 1,
                        Name = "IPhone 15",
                        Price = 1800,
                    },
                    new Product {
                        Id = 2,
                        Name = "Tv",
                        Price = null,
                    },
                    new Product {
                        Id = 3,
                        Name = null,
                        Price = 5000,
                    },
                    new Product {
                        Id = 4,
                        Name = "Bob",
                        Price = 90000,
                    }
                };

                var productsHtml = GetHtml(products);
                await writer.WriteLineAsync(productsHtml);
                context.Response.ContentType = "text/html";

                // var productsJson = JsonSerializer.Serialize(products);
                // await writer.WriteLineAsync(productsJson);
                // context.Response.ContentType = "application/json";

                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }

            break;
    }
}