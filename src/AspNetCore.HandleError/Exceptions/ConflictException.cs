namespace AspNetCore.HandleError.Exceptions;
public class ConflictException : Exception
{
    public ConflictException() : base("Entity already exists!") { }

    public ConflictException(string message) : base(message) { }

    public ConflictException(string message, Exception innerException) : base(message, innerException) { }

    public ConflictException(string name, object key) : base($"Entity {name} ({key}) already exists!") { }
}
