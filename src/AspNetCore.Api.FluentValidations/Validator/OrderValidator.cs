using AspNetCore.Api.FluentValidations.Models;
using FluentValidation;

namespace AspNetCore.Api.FluentValidations.Validator;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(x => x.Total).GreaterThan(0).WithMessage("Total GreaterThan(0)");
        RuleFor(x => x).Must(x => x.Total > 0).WithMessage("Total Must() GreaterThan(0)");
    }
}
