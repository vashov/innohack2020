using System.Collections.Generic;
using System.Linq;

namespace TestHackBlazor.Shared.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool HasAny<T>(this IEnumerable<T> e)
        {
            return e != null && e.Any();
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> e)
        {
            return !e.HasAny();
        }
    }
}
