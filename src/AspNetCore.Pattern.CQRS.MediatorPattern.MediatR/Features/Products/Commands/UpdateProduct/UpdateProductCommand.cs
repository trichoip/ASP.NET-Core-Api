using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Entities;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Repositories.Interfaces;
using AutoMapper;

using MediatR;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Products.Commands.UpdateProduct;

[AutoMap(typeof(Product), ReverseMap = true)]
public sealed record UpdateProductCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Detail { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

internal sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    async Task IRequestHandler<UpdateProductCommand>.Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Repository<Product>().FindByIdAsync(request.Id, cancellationToken);
        if (product == null)
        {
            throw new ArgumentException("Product Not Found.");
        }

        _mapper.Map(request, product);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

}
