using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Abstractions.Messaging;
using MediatR;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(int UserId, string FirstName, string LastName) : ICommand<Unit>;
