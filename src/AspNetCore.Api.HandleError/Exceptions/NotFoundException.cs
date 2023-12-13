namespace AspNetCore.Api.HandleError.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : base("The entity was not found.") { }

    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string message, Exception innerException) : base(message, innerException) { }

    public NotFoundException(string name, object key) : base($"The entity {name} with the identifier ({key}) was not found.") { }
}
