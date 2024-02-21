namespace AkhshamBazari.Presentation.Controllers;

using AkhshamBazari.Core.Data.Products.Models;
using AkhshamBazari.Core.Data.Products.Repositories;
using AkhshamBazari.Infrastructure.Data.Products.Commands;
using AkhshamBazari.Presentation.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class ProductController : Controller
{
    private readonly ISender sender;

    public ProductController(ISender sender) {
        this.sender = sender;
    }

    public async Task<IActionResult> Index()
    {
        var products = await this.sender.Send(new GetAllCommand());
        //var products = await this.productRepository.GetAllAsync();

        return View(model: products);
    }

    [HttpGet]
    public IActionResult Create() {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm]ProductDto dto) {
        try {
            var newProductCommand = new AddNewCommand {
                Name = dto.Name,
                Price = dto.Price,
            };

            await this.sender.Send(newProductCommand);

            return RedirectToAction("Index");
        }
        catch(ArgumentException ex) {
            return base.BadRequest(ex.Message);
        }
        catch {
            // this.logger.Add(ex);

            return base.StatusCode(500, "Something went wrong!");
        }
    }
}