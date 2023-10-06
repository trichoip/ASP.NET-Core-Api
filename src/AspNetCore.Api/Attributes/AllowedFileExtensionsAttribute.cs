using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Api.Attributes;

public class AllowedFileExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _allowedExtensions;

    public AllowedFileExtensionsAttribute(params string[] allowedExtensions)
    {
        _allowedExtensions = allowedExtensions;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            if (file == null)
            {
                return ValidationResult.Success;
            }

            var fileExtension = Path.GetExtension(file.FileName);

            if (_allowedExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"Invalid file format. Supported formats: {string.Join(", ", _allowedExtensions)}");
            }
        }

        return ValidationResult.Success;
    }
}
