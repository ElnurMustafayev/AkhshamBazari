namespace AkhshamBazari.Infrastructure.Data.Products.Commands;

using AkhshamBazari.Core.Data.Products.Models;
using MediatR;

public class GetAllCommand : IRequest<IEnumerable<Product>>
{
    
}