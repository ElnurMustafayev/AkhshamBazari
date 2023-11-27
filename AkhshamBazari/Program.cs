using System.Net;
using System.Reflection;
using AkhshamBazari.Controllers;
using AkhshamBazari.Controllers.Base;


async Task HomePageAsync(HttpListenerContext context)
{
    using var writer = new StreamWriter(context.Response.OutputStream);

    // home page
    var pageHtml = await File.ReadAllTextAsync("Views/Home.html");
    await writer.WriteLineAsync(pageHtml);
    context.Response.StatusCode = (int)HttpStatusCode.OK;
    context.Response.ContentType = "text/html";
}




HttpListener httpListener = new HttpListener();

const int port = 8080;
httpListener.Prefixes.Add($"http://*:{port}/");

httpListener.Start();

System.Console.WriteLine($"Server started on port {port}...");

while (true)
{
    var context = await httpListener.GetContextAsync();

    var endpointItems = context.Request.Url.AbsolutePath?.Split("/", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    if (endpointItems == null || endpointItems.Any() == false)
    {
        await HomePageAsync(context);
        continue;
    }

    var controllerType = Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(t => t.BaseType == typeof(ControllerBase))
        .FirstOrDefault(t => t.Name.ToLower() == $"{endpointItems[0]}controller");

    if (controllerType == null)
    {
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        continue;
    }

    var controllerMethod = controllerType
        .GetMethods()
        .FirstOrDefault(m => m.Name.ToLower().Contains(context.Request.HttpMethod.ToLower()));

    if(controllerMethod == null) {
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        continue;
    }

    var controller = Activator.CreateInstance(controllerType) as ControllerBase;
    controllerMethod.Invoke(controller, parameters: new[] { context });
}