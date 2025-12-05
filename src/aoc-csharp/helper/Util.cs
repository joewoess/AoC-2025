namespace aoc_csharp.helper;

using System.Numerics;

public static class Util
{
    /** Returns an enumerable of ints between from and to counting down if needed */
    public static IEnumerable<int> Range(int from, int to)
    {
        return from > to
            ? Enumerable.Range(to, from - to + 1).Reverse()
            : Enumerable.Range(from, to - from + 1);
    }

    /// <summary>
    ///     Generates a range of whole numbers between start and end point, optionally with inclusive end.
    ///     If the end point is lower than start point, it will return steps downward towards the start point.
    /// </summary>
    /// <typeparam name="T">Generic Number type to allow for types like long and BigInteger></typeparam>
    /// <param name="from">Starting point</param>
    /// <param name="to">End point, per default exclusive</param>
    /// <param name="inclEnd">optional parameter to make the end point inclusive</param>
    /// <returns>Enumerable of generic Number type between start and end point</returns>
    /// <exception cref="IsNotAWholeNumberException">In case the start and end point were not integral numbers</exception>
    public static IEnumerable<T> IntegerRange<T>(T from, T to, bool inclEnd = false) where T : INumber<T>
    {
        if (!(T.IsInteger(from) && T.IsInteger(to)))
            throw new IsNotAWholeNumberException();

        T current = from;
        T step(T current) => from > to
                                ? current - T.One
                                : current + T.One;
        Func<T, bool> check = (from > to, inclEnd) switch
        {
            (false, false) => current => current < to,
            (false, true) => current => current <= to,
            (true, false) => current => current > to,
            (true, true) => current => current >= to,
        };
        while (check(current))
        {
            yield return current;
            current = step(current);
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


    /** Inverts a predicate */
    public static Func<TSource, bool> Invert<TSource>(this Func<TSource, bool> original)
    {
        return x => !original(x);
    }
}