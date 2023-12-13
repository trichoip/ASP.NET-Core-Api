namespace AspNetCore.Api.HandleError.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException() : base("Value invalid.") { }
    public BadRequestException(string message) : base(message) { }
    public BadRequestException(string message, Exception innerException) : base(message, innerException) { }
}
