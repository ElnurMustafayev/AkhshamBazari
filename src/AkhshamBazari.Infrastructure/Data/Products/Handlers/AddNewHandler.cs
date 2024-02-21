namespace AkhshamBazari.Infrastructure.Data.Products.Handlers;

using AkhshamBazari.Core.Data.Products.Models;
using AkhshamBazari.Core.Data.Products.Repositories;
using AkhshamBazari.Infrastructure.Data.Products.Commands;
using MediatR;

public class AddNewHandler : IRequestHandler<AddNewCommand>
{
    private readonly IProductRepository productRepository;

    public AddNewHandler(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }
    public async Task Handle(AddNewCommand request, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(request.Name)) {
            throw new ArgumentException("Name can not be empty!");
        }

        if(request.Price < 0) {
            throw new ArgumentException("Price can not be less than 0!");
        }

        var newProduct = new Product {
            Name = request.Name,
            Price = request.Price,
        };

        await this.productRepository.InsertProductAsync(newProduct);
    }
}