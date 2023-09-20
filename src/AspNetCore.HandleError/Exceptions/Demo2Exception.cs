using System.Globalization;

namespace AspNetCore.HandleError.Exceptions
{
    public class Demo2Exception : Exception
    {
        public Demo2Exception() : base() { }
        public Demo2Exception(string message) : base(message) { }
        public Demo2Exception(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
