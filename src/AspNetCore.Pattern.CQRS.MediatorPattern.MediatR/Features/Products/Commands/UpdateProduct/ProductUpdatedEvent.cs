using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Common;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Entities;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Products.Commands.UpdateProduct;

public record ProductUpdatedEvent : BaseEvent
{
    public Product Product { get; }

    public ProductUpdatedEvent(Product Product)
    {
        Product = Product;
    }
}
