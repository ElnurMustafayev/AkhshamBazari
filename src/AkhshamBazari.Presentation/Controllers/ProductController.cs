namespace AkhshamBazari.Presentation.Controllers;

using AkhshamBazari.Core.Models;
using AkhshamBazari.Core.Repositories;
using AkhshamBazari.Core.Services.Base;
using AkhshamBazari.Presentation.Dtos;
using Microsoft.AspNetCore.Mvc;

public class ProductController : Controller
{
    private readonly IProductRepository productRepository;
    private readonly IProductService productService;

    public ProductController(IProductRepository productRepository,
    IProductService productService) {
        this.productRepository = productRepository;
        this.productService = productService;
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
        try {
            var newProduct = new Product {
                Name = dto.Name,
                Price = dto.Price,
            };

            await this.productService.AddNewProductAsync(newProduct);

            return RedirectToAction("Index");
        }
        catch(ArgumentException ex) {
            return base.BadRequest(ex.Message);
        }
        catch(Exception ex) {
            // this.logger.Add(ex);

            return base.StatusCode(500, "Something went wrong!");
        }
    }
}