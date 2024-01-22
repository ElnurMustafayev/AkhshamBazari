namespace AkhshamBazari.Controllers;

using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Route("/[controller]/[action]/{statusCode}")]    
    public IActionResult Error(int statusCode)
    {
        return base.Ok($"Status code: {statusCode}!");
    }
}
