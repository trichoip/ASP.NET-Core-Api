using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Contracts.Products;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Entities;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Repositories.Interfaces;
using MediatR;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Products.Queries.GetAllProducts;

public sealed record GetAllProductsQuery : IRequest<IEnumerable<ProductResponse>>;

internal sealed class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllProductsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _unitOfWork
            .Repository<Product>()
            .FindAsync<ProductResponse>(cancellationToken: cancellationToken);

        return products;
    }
}
