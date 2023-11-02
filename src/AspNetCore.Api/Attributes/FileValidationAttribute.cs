using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Api.Attributes;

public class FileValidationAttribute : ValidationAttribute
{
    private string[] AllowedExtensions { get; set; }
    private long MaxSizeInBytes { get; set; }

    public FileValidationAttribute(long maxSizeInBytes, params string[] allowedExtensions)
    {
        AllowedExtensions = allowedExtensions;
        MaxSizeInBytes = maxSizeInBytes;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile singleFile)
        {
            return ValidateFile(singleFile);
        }

        if (value is ICollection<IFormFile> fileCollection)
        {
            if (fileCollection.Count > 5)
            {
                return new ValidationResult("Maximum 5 files are allowed.");
            }
            foreach (var file in fileCollection)
            {
                var validationResult = ValidateFile(file);
                if (validationResult != ValidationResult.Success)
                {
                    return validationResult;
                }
            }
        }

        return ValidationResult.Success;
    }

    private ValidationResult ValidateFile(IFormFile file)
    {
        if (file == null)
        {
            return ValidationResult.Success!;
        }
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (AllowedExtensions != null && !AllowedExtensions.Contains(fileExtension))
        {
            return new ValidationResult($"Only {string.Join(", ", AllowedExtensions)} files are allowed.");
        }

        if (file.Length > MaxSizeInBytes)
        {
            return new ValidationResult($"File size should not exceed {MaxSizeInBytes / (1024 * 1024)} MB.");
        }

        return ValidationResult.Success!;
    }
}
