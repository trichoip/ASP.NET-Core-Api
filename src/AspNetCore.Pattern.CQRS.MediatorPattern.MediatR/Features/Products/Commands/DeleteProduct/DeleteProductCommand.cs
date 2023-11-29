using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Entities;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Repositories.Interfaces;
using MediatR;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Products.Commands.DeleteProduct;

public sealed record DeleteProductCommand : IRequest
{
    public Guid Id { get; set; }

    public DeleteProductCommand()
    {

    }

    public DeleteProductCommand(Guid id)
    {
        Id = id;
    }
}

internal sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    async Task IRequestHandler<DeleteProductCommand>.Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Repository<Product>().FindByIdAsync(request.Id, cancellationToken);
        if (product == null)
        {
            throw new ArgumentException("Product Not Found.");
        }

        await _unitOfWork.Repository<Product>().DeleteAsync(product);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

}
