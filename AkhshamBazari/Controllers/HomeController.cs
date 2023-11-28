namespace AkhshamBazari.Controllers;

using System.Net;
using AkhshamBazari.Controllers.Base;

public class HomeController : ControllerBase
{
    public async Task HomePageAsync(HttpListenerContext context)
    {
        using var writer = new StreamWriter(context.Response.OutputStream);

        var pageHtml = await File.ReadAllTextAsync("Views/Home.html");
        await writer.WriteLineAsync(pageHtml);
        context.Response.StatusCode = (int)HttpStatusCode.OK;
        context.Response.ContentType = "text/html";
    }
}