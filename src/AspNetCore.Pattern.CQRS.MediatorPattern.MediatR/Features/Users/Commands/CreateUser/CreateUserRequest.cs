namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Users.Commands.CreateUser;

public sealed record CreateUserRequest(string FirstName, string LastName);
