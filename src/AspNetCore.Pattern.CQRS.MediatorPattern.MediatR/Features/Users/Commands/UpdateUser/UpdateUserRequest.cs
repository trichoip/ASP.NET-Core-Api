namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Users.Commands.UpdateUser;

public sealed record UpdateUserRequest(string FirstName, string LastName);
