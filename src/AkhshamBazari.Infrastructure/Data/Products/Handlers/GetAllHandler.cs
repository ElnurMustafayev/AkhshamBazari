namespace AkhshamBazari.Infrastructure.Data.Products.Handlers;

using System.Threading;
using System.Threading.Tasks;
using AkhshamBazari.Core.Data.Products.Models;
using AkhshamBazari.Core.Data.Products.Repositories;
using AkhshamBazari.Infrastructure.Data.Products.Commands;
using MediatR;

public class GetAllHandler : IRequestHandler<GetAllCommand, IEnumerable<Product>>
{
    private readonly IProductRepository productRepository;

    public GetAllHandler(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> Handle(GetAllCommand request, CancellationToken cancellationToken)
    {
        var result = await this.productRepository.GetAllAsync();

        if(result is null || result.Any() == false) {
            return Enumerable.Empty<Product>();
        }

        return result;
    }
}