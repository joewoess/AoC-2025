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

    /** Maps the pairs of values of self product (list X list) with a mapper function **/
    public static IEnumerable<TOut> MapSelfCrossProduct<T, TOut>(this IList<T> values, Func<T, T, TOut> mapper,
        bool bothDirections = false, bool ignoreSelfMap = false)
    {
        for (var i = 0; i < values.Count - 1; i++)
        {
            for (var j = i + (ignoreSelfMap ? 1 : 0); j < values.Count; j++)
            {
                yield return mapper(values[i], values[j]);
                if (bothDirections) yield return mapper(values[j], values[i]);
            }
        }
    }

    /** Maps the pairs of values of two lists (listA X listB) with a mapper function **/
    public static IEnumerable<TOut> MapCrossProduct<TA, TB, TOut>(this IList<TA> listA, IList<TB> listB, Func<TA, TB, TOut> mapper,
        bool bothDirections = false, Func<TB, TA, TOut>? reverseMapper = null)
    {
        for (var i = 0; i < listA.Count; i++)
        {
            for (var j = 0; j < listB.Count; j++)
            {
                yield return mapper(listA[i], listB[j]);
                if (bothDirections && reverseMapper != null) yield return reverseMapper(listB[j], listA[i]);
            }
        }
    }

    /** wrapper for MapCrossProduct if lists have the same data type **/
    public static IEnumerable<TOut> MapCrossProduct<T, TOut>(this IList<T> listA, IList<T> listB, Func<T, T, TOut> mapper, bool bothDirections = false)
         => MapCrossProduct(listA, listB, mapper, bothDirections, mapper);
}