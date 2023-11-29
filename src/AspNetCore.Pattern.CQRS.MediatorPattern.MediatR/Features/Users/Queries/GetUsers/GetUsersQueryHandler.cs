using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Abstractions.Messaging;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Contracts.Users;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Entities;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Repositories.Interfaces;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Users.Queries.GetUsers;

internal sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IList<UserResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUsersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IList<UserResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _unitOfWork
            .Repository<User>()
            .FindAsync<UserResponse>(cancellationToken: cancellationToken);

        return users;
    }
}