namespace AspNetCore.CleanArchitecture.Project.Demo.Application.Common.Exceptions;

public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base() { }
}
