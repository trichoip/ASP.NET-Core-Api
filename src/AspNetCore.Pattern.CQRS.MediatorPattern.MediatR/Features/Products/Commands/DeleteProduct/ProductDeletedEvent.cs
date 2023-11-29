using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Common;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Entities;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Products.Commands.DeleteProduct;

public record ProductDeletedEvent : BaseEvent
{
    public Product Product { get; }

    public ProductDeletedEvent(Product Product)
    {
        Product = Product;
    }
}
