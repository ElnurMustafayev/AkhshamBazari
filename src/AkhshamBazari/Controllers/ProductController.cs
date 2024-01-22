namespace AkhshamBazari.Controllers;

using AkhshamBazari.Dtos;
using AkhshamBazari.Models;
using AkhshamBazari.Repositories.Base;
using Microsoft.AspNetCore.Mvc;

public class ProductController : Controller
{
    private readonly IProductRepository productRepository;

    public ProductController(IProductRepository productRepository) {
        this.productRepository = productRepository;
    }

    public async Task<IActionResult> Index()
    {
        var products = await this.productRepository.GetAllAsync();

        return View(model: products);
    }

    [HttpGet]
    public IActionResult Create() {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm]ProductDto dto) {
        await this.productRepository.InsertProductAsync(new Product {
            Name = dto.Name,
            Price = dto.Price,
        });

        return RedirectToAction("Index");
    }
}