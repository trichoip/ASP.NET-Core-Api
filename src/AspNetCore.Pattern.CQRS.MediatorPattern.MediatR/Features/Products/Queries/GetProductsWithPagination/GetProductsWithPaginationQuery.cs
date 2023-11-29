using AspNetCore.Helpers.Helpers;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Contracts.Products;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Entities;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Repositories.Interfaces;
using MediatR;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Products.Queries.GetProductsWithPagination;

public sealed record GetProductsWithPaginationQuery : IRequest<PaginatedResponse<ProductResponse>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetProductsWithPaginationQuery() { }

    public GetProductsWithPaginationQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

internal sealed class GetProductsWithPaginationQueryHandler : IRequestHandler<GetProductsWithPaginationQuery, PaginatedResponse<ProductResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductsWithPaginationQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginatedResponse<ProductResponse>> Handle(GetProductsWithPaginationQuery query, CancellationToken cancellationToken)
    {
        var products = await _unitOfWork.Repository<Product>()
                                .FindAsync<ProductResponse>(
                                    query.PageNumber,
                                    query.PageSize,
                                    orderBy: _ => _.OrderBy(_ => _.Name),
                                    cancellationToken: cancellationToken);

        return await products.ToPaginatedResponseAsync();
    }
}
