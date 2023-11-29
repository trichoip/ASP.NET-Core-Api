using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Contracts.Products;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Entities;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Repositories.Interfaces;
using MediatR;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery : IRequest<ProductResponse>
{
    public Guid Id { get; set; }
    public GetProductByIdQuery()
    {

    }
    public GetProductByIdQuery(Guid id)
    {
        Id = id;
    }
}

internal sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductResponse> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork
            .Repository<Product>()
            .FindByAsync<ProductResponse>(_ => _.Id == query.Id, cancellationToken);

        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        return product;
    }
}
