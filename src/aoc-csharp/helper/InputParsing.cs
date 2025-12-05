namespace aoc_csharp.helper;

/// <summary>
///     Helper functions for parsing input text / lines into desireable format
/// </summary>
public static class InputParsing
{
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
}