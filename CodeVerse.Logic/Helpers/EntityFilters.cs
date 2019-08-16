using CodeVerse.Common.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeVerse.Common
{
    public static class EntityFilters
    {
        public static List<T> Casted<T, S>(this List<S> list) where T : class
        {
            return list
                .Where(q => q is T)
                .Select(q => q as T)
                .ToList();
        }

        public static void test()
        {
            var lol = new List<PlayerCommand>();
            var scans = lol.Casted<ScanCommand, PlayerCommand>();
        }
    }
}
