using System.Collections;
using System.ComponentModel;

namespace AspNetCore.Api.Helpers.Extensions;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class CollectionExtensions
{

    public static T? Find<T>(this T[] items, Predicate<T> predicate)
    {
        return Array.Find(items, predicate);
    }

    public static T[] FindAll<T>(this T[] items, Predicate<T> predicate)
    {
        return Array.FindAll(items, predicate);
    }

    public static bool IsNullOrEmpty(this IEnumerable @this)
    {
        if (@this != null)
        {
            return !@this.GetEnumerator().MoveNext();
        }

        return true;
    }
}
