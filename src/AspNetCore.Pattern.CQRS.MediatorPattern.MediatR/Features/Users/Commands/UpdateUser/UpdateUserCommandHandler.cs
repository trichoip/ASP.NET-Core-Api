using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Abstractions.Messaging;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Entities;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Repositories.Interfaces;
using Mapster;
using MediatR;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Users.Commands.UpdateUser;

internal sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, Unit>
{

    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(IUnitOfWork unitOfWork)
    {

        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<User>().FindByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException();
        }

        request.Adapt(user);

        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
