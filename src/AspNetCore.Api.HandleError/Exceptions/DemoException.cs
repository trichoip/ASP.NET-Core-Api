using System.Globalization;

namespace AspNetCore.Api.HandleError.Exceptions;

public class DemoException : Exception
{
    public DemoException() : base() { }
    public DemoException(string message) : base(message) { }
    public DemoException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}
