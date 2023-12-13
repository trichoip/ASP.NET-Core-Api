using System.Globalization;

namespace AspNetCore.Api.HandleError.Exceptions;

public class AppException : Exception
{
    public AppException() : base("exception") { }

    public AppException(string message) : base(message) { }

    public AppException(string message, Exception innerException) : base(message, innerException) { }

    public AppException(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, message, args)) { }
}
