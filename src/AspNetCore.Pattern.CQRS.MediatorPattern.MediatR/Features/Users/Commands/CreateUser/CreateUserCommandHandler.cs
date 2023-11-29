using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Abstractions.Messaging;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Contracts.Users;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Entities;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Repositories.Interfaces;
using Mapster;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Users.Commands.CreateUser;

internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, UserResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = request.Adapt<User>();

        await _unitOfWork.Repository<User>().CreateAsync(user, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return user.Adapt<UserResponse>();
    }
}