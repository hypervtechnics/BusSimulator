using System;
using System.Collections.Generic;

namespace BusSimulator.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> DropLast<T>(this IEnumerable<T> source, int n)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (n < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "Argument n should be non-negative.");
            }

            return InternalDropLast(source, n);
        }

        private static IEnumerable<T> InternalDropLast<T>(IEnumerable<T> source, int n)
        {
            Queue<T> buffer = new Queue<T>(n + 1);

            foreach (T x in source)
            {
                buffer.Enqueue(x);

                if (buffer.Count == n + 1)
                    yield return buffer.Dequeue();
            }
        }
    }
}
