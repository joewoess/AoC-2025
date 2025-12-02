namespace aoc_csharp.helper;

using System.Numerics;

public static class Util
{
    /** Initializes a list with count amount default values */
    public static List<T> InitializeListWithDefault<T>(int count, Func<T> defaultFactory) => Range(1, count).Select(_ => defaultFactory.Invoke()).ToList();

    /** Returns an enumerable of ints between from and to counting down if needed */
    public static IEnumerable<int> Range(int from, int to)
    {
        return from > to
            ? Enumerable.Range(to, from - to + 1).Reverse()
            : Enumerable.Range(from, to - from + 1);
    }

    /** Returns an enumerable of pairs between the elements of a collection */
    public static IEnumerable<(T From, T To)> PairWithNextDeprecated<T>(this IEnumerable<T> collection) => collection.Zip(collection.Skip(1), (a, b) => (a, b));

    /** Returns an enumerable of pairs between the elements of a collection. Uses yield returns and a single enumeration */
    public static IEnumerable<(T From, T To)> PairWithNext<T>(this IEnumerable<T> collection)
    {
        using var enumerator = collection.GetEnumerator();
        if (enumerator.MoveNext().Not()) yield break;
        var previous = enumerator.Current;
        while (enumerator.MoveNext())
        {
            yield return (previous, enumerator.Current);
            previous = enumerator.Current;
        }
    }

    /** Executes the action count times */
    public static void DoTimes(this int count, Action action)
    {
        foreach (var _ in Range(1, count))
        {
            action.Invoke();
        }
    }

    /** If a string has two value split by a separator, this parses it into a pair of 2 strings */
    public static (TFirst First, TSecond Second) SplitAndMapToPair<TFirst, TSecond>(this string str, Func<string, TFirst> mapFirst, Func<string, TSecond> mapSecond,
        string separator = ",")
    {
        var parts = str.Split(separator);
        switch (parts.Length)
        {
            case < 2:
                throw new ArgumentException($"String {str} did not have two elements when split with {separator}");
            case 2:
                return (mapFirst(parts[0]), mapSecond(parts[1]));
            case > 2:
                Printer.DebugMsg($"Expected 2 parts in string '{str}' but got {parts.Length}");
                return (mapFirst(parts[0]), mapSecond(parts[1]));
        }
    }

    public static (T First, T Second) SplitAndMapToPair<T>(this string str, Func<string, T> mapBoth, string separator = ",") => str.SplitAndMapToPair(mapBoth, mapBoth, separator);
    public static (string First, string Second) ToStrPair(this string str, string separator = ",") => str.SplitAndMapToPair(s => s, separator);

    public static (TTo First, TTo Second) MapPairWith<TFrom, TTo>(this (TFrom First, TFrom Second) pair, Func<TFrom, TTo> func) =>
        (func.Invoke(pair.First), func.Invoke(pair.Second));

    /** Extension function to check if a number is between two other numbers */
    public static bool Between<T>(this T num, T min, T max, bool inclEnd = false) where T : INumber<T> => (num >= min) && (inclEnd ? (num <= max) : (num < max));

    /** Extension function to get the string representation of a list */
    public static string ToListString<T>(this IEnumerable<T>? list, string? separator = ",", string? prefix = "[", string? postfix = "]")
        => list != null
            ? $"{prefix}{string.Join(separator, list)}{postfix}"
            : $"{prefix}{postfix}";

    /** Just checks if a generic value is not null. Useful for linq */
    public static bool IsNotNull<T>(this T? value) where T : class => value != null;

    /** Just checks if a string is not null or whitespace. Useful for linq */
    public static bool HasContent(this string? value) => !string.IsNullOrWhiteSpace(value);

    /** Just negates a boolean. Useful for linq */
    public static bool Not(this bool value) => !value;

    /** Where filter with inverted predicate */
    public static IEnumerable<TSource> WhereNot<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        return source.Where(predicate.Invert());
    }

    /** Inverts a predicate */
    public static Func<TSource, bool> Invert<TSource>(this Func<TSource, bool> original)
    {
        return x => !original(x);
    }

    /** Enumerate with index used in foreach **/
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
       => self.Select((item, index) => (item, index));

    /** Sliding window over a string **/
    public static IEnumerable<string> Window(this string self, int windowSize)
    {
        if (self.Length >= windowSize)
        {
            for (int idx = 0; idx < self.Length - windowSize; idx++)
            {
                yield return self[idx..(idx + windowSize)];
            }
        }
    }
}