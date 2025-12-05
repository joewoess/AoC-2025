namespace aoc_csharp.helper;

public static class CollectionUtil
{
    /** Initializes a list with count amount default values */
    public static List<T> InitializeListWithDefault<T>(int count, Func<T> defaultFactory) => Util.Range(1, count).Select(_ => defaultFactory.Invoke()).ToList();

    /** Where filter with inverted predicate */
    public static IEnumerable<TSource> WhereNot<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        return source.Where(predicate.Invert());
    }

    /** Enumerate with index used in foreach **/
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
       => self.Select((item, index) => (item, index));

    /** Sliding window over a string **/
    public static IEnumerable<IList<T>> Window<T>(this IList<T> self, int windowSize)
    {
        if (self.Count >= windowSize)
        {
            for (int idx = 0; idx < self.Count - windowSize; idx++)
            {
                yield return self.Skip(idx).Take(windowSize).ToList();
            }
        }
    }
}