using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Common;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Entities;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Repositories.Interfaces;
using AutoMapper;
using MediatR;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Products.Commands.CreateProduct;

// IRequest: single request being handled by a single handler
public sealed record CreateProductCommand(CreateProductRequest ProductRequest) : IRequest<Product>;
internal sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    async Task<Product> IRequestHandler<CreateProductCommand, Product>.Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(request.ProductRequest);
        await _unitOfWork.Repository<Product>().CreateAsync(product, cancellationToken);

        product.AddDomainEvent(new ProductCreatedEvent(product));

        await _unitOfWork.CommitAsync(cancellationToken);
        return product;
    }
}

// INotification: single request by multiple handlers
public sealed record ProductCreatedEvent(Product Product) : BaseEvent;

internal sealed class EmailHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public EmailHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Repository<Product>().FindByIdAsync(notification.Product.Id, cancellationToken);
        product!.Name += $" evt: Email sent";
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}

internal sealed class CacheInvalidationHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public CacheInvalidationHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Repository<Product>().FindByIdAsync(notification.Product.Id, cancellationToken);
        product!.Name += $" evt: Cache Invalidated";
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}