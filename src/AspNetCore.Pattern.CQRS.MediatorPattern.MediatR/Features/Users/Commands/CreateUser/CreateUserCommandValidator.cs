using FluentValidation;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Features.Users.Commands.CreateUser;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);

        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
    }
}