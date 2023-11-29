using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Entities;
using AutoMapper;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Contracts.Products;

// Product -> ProductResponse
[AutoMap(typeof(Product))]
public sealed record ProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Detail { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
