using System.Collections;
using System.Collections.Generic;

namespace CoreDomain.Scripts.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T> list)
        {
            return list == null || list.Count == 0;
        }
    }
}