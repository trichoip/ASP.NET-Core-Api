using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;

namespace AspNetCore.Api.Helpers.Extensions;
public static class UtilitiesExtensions
{

    public static U? Map<T, U>(this T? value, Func<T, U> mapper)
        where T : class
        where U : class
    {
        if (value == null) return null;
        return mapper(value);
    }

    public static void IfPresent<T>(this T? value, Action<T> consumer) where T : class
    {
        if (value != null)
        {
            consumer(value);
        }
    }

    public static T OrElseThrow<T, X>(this T? value, Func<X> exceptionSupplier) where X : Exception
    {
        return value ?? throw exceptionSupplier.Invoke();
    }

    public static T OrElse<T>(this T? value, T other) where T : class
    {
        return value ?? other;
    }

    public static T OrElseGet<T>(this T? value, Func<T> other) where T : class
    {
        return value ?? other();
    }

    public static T ThrowIfNull<T>(this T obj, string paramName)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(paramName);
        }
        return obj;
    }

    public static string ThrowIfNullOrEmpty(this string str, string paramName)
    {
        if (string.IsNullOrEmpty(str))
        {
            throw new ArgumentException("Parameter was empty", paramName);
        }
        return str;
    }

    public static int ConvertToInteger(this string @this)
    {
        if (!int.TryParse(@this, out var result))
        {
            throw new ArgumentException("The string is not a valid integer", @this);
        }
        return result;
    }

    public static bool IsNullOrEmpty(this IEnumerable? @this)
    {
        if (@this != null)
        {
            return !@this.GetEnumerator().MoveNext();
        }
        return true;
    }

    public static T? Find<T>(this T[] items, Predicate<T> predicate)
    {
        return Array.Find(items, predicate);
    }

    public static T[] FindAll<T>(this T[] items, Predicate<T> predicate)
    {
        return Array.FindAll(items, predicate);
    }

    // same feature RemoveAll
    public static bool RemoveIf<T>(this ICollection<T> collection, Func<T, bool> filter)
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));
        bool removed = false;
        var itemsToRemove = collection.Where(filter).ToList();
        foreach (var item in itemsToRemove)
        {
            collection.Remove(item);
            removed = true;
        }
        return removed;
    }

    public static T DeepClone<T>(this T obj)
    {
        var jsonString = JsonSerializer.Serialize(obj);
        return JsonSerializer.Deserialize<T>(jsonString);
    }

    public static string GetIpAddress(HttpContext context)
    {
        var ipAddress = string.Empty;

        var remoteIpAddress = context.Connection.RemoteIpAddress;

        if (remoteIpAddress != null)
        {
            if (remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
            {
                remoteIpAddress = Dns.GetHostEntry(remoteIpAddress).AddressList
                    .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
            }

            if (remoteIpAddress != null) ipAddress = remoteIpAddress.ToString();

            Console.WriteLine(ipAddress);
            return ipAddress;
        }

        return "127.0.0.1";
    }

    public static string GetIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());

        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                Console.WriteLine(ip.ToString());
                return ip.ToString();
            }
        }

        return "127.0.0.1";
    }

    // UtilitiesExtensions.GetLocalIPv4(NetworkInterfaceType.Wireless80211)
    public static string GetLocalIPv4(NetworkInterfaceType _type)
    {
        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        Console.WriteLine(ip.Address.ToString());
                        return ip.Address.ToString();
                    }
                }
            }
        }
        return "127.0.0.1";
    }

    internal static string ConvertToRFC3339(DateTime date)
    {
        if (date.Kind == DateTimeKind.Unspecified)
        {
            date = date.ToUniversalTime();
        }
        return date.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", DateTimeFormatInfo.InvariantInfo);
    }
    public static DateTime? GetDateTimeFromString(string raw)
    {
        DateTime result;
        if (!DateTime.TryParse(raw, out result))
        {
            return null;
        }
        return result;
    }
    public static T CheckEnumValue<T>(T value, string paramName) where T : struct
    {
        CheckArgument(
            Enum.IsDefined(typeof(T), value),
            paramName,
            "Value {0} not defined in enum {1}", value, typeof(T).Name);
        return value;
    }
    public static void CheckArgument<T1, T2>(bool condition, string paramName, string format, T1 arg0, T2 arg1)
    {
        if (!condition)
        {
            throw new ArgumentException(string.Format(format, arg0, arg1), paramName);
        }
    }
    public static string GetStringFromDateTime(DateTime? date)
    {
        if (!date.HasValue)
        {
            return null;
        }
        return ConvertToRFC3339(date.Value);
    }
    public static DateTimeOffset? GetDateTimeOffsetFromString(string raw) =>
        raw is null
        ? null
        : DateTimeOffset.ParseExact(raw, "yyyy-MM-dd'T'HH:mm:ss.FFF'Z'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
    public static string GetStringFromDateTimeOffset(DateTimeOffset? date) =>
       date is null
       ? null
       // While FFF sounds like it should work, we really want to produce no subsecond parts or 3 digits.
       : date.Value.Millisecond == 0 ? date.Value.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'")
       : date.Value.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture);
}
