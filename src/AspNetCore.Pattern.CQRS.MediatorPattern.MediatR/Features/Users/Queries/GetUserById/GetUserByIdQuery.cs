using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Abstractions.Messaging;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Contracts.Users;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(int UserId) : IQuery<UserResponse>;
