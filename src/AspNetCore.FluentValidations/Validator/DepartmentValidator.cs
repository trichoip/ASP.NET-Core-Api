using AspNetCore.FluentValidations.Models;
using FluentValidation;

namespace AspNetCore.FluentValidations.Validator
{
    public class DepartmentValidator : AbstractValidator<Department>
    {

        public DepartmentValidator()
        {
            RuleFor(d => d.Id)
                .NotNull().WithMessage("Id is required")
                .GreaterThan(0).WithMessage("Id must be greater than 0");

            RuleFor(d => d.Name)
                .NotNull().WithMessage("Name is required")
                .Length(10, 20).WithMessage("Name must be between 10 and 20 characters");
        }
    }
}
