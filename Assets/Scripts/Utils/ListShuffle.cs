using System;
using System.Collections.Generic;

namespace TestGame
{
    public static class ListShuffle
    {
        private static readonly Random rng = new();

        internal static void Shuffle<T>(this List<T> list)
        {
            var n = list.Count;
            while (n-- > 1)
            {
                var k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}