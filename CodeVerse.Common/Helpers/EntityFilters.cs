using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeVerse.Common
{
    public static class EntityFilters
    {
        public static List<T> GetCasted<T>(this List<T> list, T lol)
        {
            return list
                .Where(q => q is T)
                .Select(q => (T)q)
                .ToList();
        }

        public static T[] GetCasted<T>(this T[] list)
        {
            return list
                .Where(q => q is T)
                .Select(q => (T)q)
                .ToArray();
        }
    }
}
