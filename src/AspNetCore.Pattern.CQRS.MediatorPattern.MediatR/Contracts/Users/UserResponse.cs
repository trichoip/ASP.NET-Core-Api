namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Contracts.Users;

public sealed record UserResponse(int Id, string FirstName, string LastName);
