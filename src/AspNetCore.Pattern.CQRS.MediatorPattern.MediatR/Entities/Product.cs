using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Common;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Entities;

public class Product : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Detail { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}