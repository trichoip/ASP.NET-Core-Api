using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Abstractions.Messaging;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Contracts.Users;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Users.Commands.CreateUser;

public sealed record CreateUserCommand(string FirstName, string LastName) : ICommand<UserResponse>;
