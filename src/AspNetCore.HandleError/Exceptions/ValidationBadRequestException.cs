using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCore.HandleError.Exceptions;
public class ValidationBadRequestException : Exception
{
    public ModelStateDictionary ModelState { get; set; } = default!;

    public ValidationBadRequestException() : base("Value invalid.") { }

    public ValidationBadRequestException(string message) : base(message) { }

    public ValidationBadRequestException(string message, Exception innerException) : base(message, innerException) { }
    public ValidationBadRequestException(ModelStateDictionary modelState) : base("Multiple errors occurred. See error details.")
    {
        ModelState = modelState;
    }

    public ValidationBadRequestException(IEnumerable<IdentityError> errors) : base("Multiple errors occurred. See error details.")
    {
        ModelState = new ModelStateDictionary();
        errors.ToList().ForEach(error => ModelState.AddModelError(error.Code, error.Description));
    }
}
