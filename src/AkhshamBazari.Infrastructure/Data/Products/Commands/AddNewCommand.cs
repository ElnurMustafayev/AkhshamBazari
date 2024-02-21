using MediatR;

namespace AkhshamBazari.Infrastructure.Data.Products.Commands;

public class AddNewCommand : IRequest
{
    public double? Price { get; set; }
    public string? Name { get; set; }
}