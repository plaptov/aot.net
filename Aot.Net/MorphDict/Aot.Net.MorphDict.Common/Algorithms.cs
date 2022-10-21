namespace Aot.Net.MorphDict.Common
{
    public static class Algorithms
    {
        // TODO Maybe need some optimization
        public static IndexBasedReadOnlySpan<T> EqualRange<T, V>(this IReadOnlyList<T> list,
            int start, int end, V value, Func<T, V> selector)
            where V : IComparable<V>
        {
            if (end - start <= 0)
                return default;

            int newStart = -1;
            int i;
            for (i = start; i < end; i++)
            {
                var v = selector(list[i]);
                var c = v.CompareTo(value);
                if (c == 0 && newStart < 0)
                    newStart = i;
                if (c > 0)
                {
                    break;
                }
                i++;
            }
            return newStart < 0
                ? default
                : new IndexBasedReadOnlySpan<T>(list, newStart..i);
        }
    }
}
