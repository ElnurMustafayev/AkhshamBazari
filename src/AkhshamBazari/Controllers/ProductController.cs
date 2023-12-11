namespace AkhshamBazari.Controllers;

using AkhshamBazari.Models;
using Microsoft.AspNetCore.Mvc;

public class ProductController : Controller
{
    public ProductController() {}

    [ActionName("Index")]
    public IActionResult ShowAllProducts()
    {
        IEnumerable<Product> products = new List<Product> {
            new Product {
                Id = 777,
                Name = "Test Product"
            },
        };

        return View(model: products);
    }
}