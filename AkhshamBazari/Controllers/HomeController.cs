namespace AkhshamBazari.Controllers;

using System.Net;
using AkhshamBazari.Attributes;
using AkhshamBazari.Controllers.Base;

public class HomeController : ControllerBase
{
    [HttpGet]
    public async Task HomePageAsync()
    {
        using var writer = new StreamWriter(base.HttpContext.Response.OutputStream);

        var pageHtml = await File.ReadAllTextAsync("Views/Home.html");
        await writer.WriteLineAsync(pageHtml);
        base.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
        base.HttpContext.Response.ContentType = "text/html";
    }
}