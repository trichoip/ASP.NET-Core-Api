using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Entities;
using AutoMapper;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Products.Commands.CreateProduct;

[AutoMap(typeof(Product), ReverseMap = true)]
public sealed record CreateProductRequest
{
    public string Name { get; set; } = default!;
    public string Detail { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
