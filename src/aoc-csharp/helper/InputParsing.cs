using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using Spectre.Console.Rendering;

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

    /** convenience wrapper for SplitAndMapToRecord with included mapper for a pair */
    public static TRecord SplitAndMapToRecord<TValue, TRecord>(this string str, Func<string, TValue> mapper,
                                                               Func<(TValue First, TValue Second), TRecord> recordMapper, string separator = ",")
        => recordMapper(str.SplitAndMapToPair(mapper, separator));

    /** splits a string by seperator and delivers the value for a factory method of the record type */
    public static TRecord SplitAndMapToRecord<TRecord>(this string str, Func<string[], TRecord> recordMapper, string separator = ",")
        => recordMapper(str.Split(separator));

    /** convenience wrapper for SplitAndMapToPair that splits a string into a pair mapped values with identical type */
    public static (T First, T Second) SplitAndMapToPair<T>(this string str, Func<string, T> mapBoth, string separator = ",") => str.SplitAndMapToPair(mapBoth, mapBoth, separator);

    /** convenience wrapper for SplitAndMapToPair that splits a string into a pair of strings */
    public static (string First, string Second) ToStrPair(this string str, string separator = ",") => str.SplitAndMapToPair(s => s, separator);

    /** Remapping a pair of type TFrom into a pair of Type TTo */
    public static (TTo First, TTo Second) MapPairWith<TFrom, TTo>(this (TFrom First, TFrom Second) pair, Func<TFrom, TTo> func) =>
        (func.Invoke(pair.First), func.Invoke(pair.Second));

    /** Splits a string array (of input lines) by a given separatorLine */
    public static IEnumerable<string[]> SplitSections(this ICollection<string> inputLines, string? separatorLine = null)
    {
        using var enumerator = inputLines.GetEnumerator();
        Func<string?, bool> IsSeparatorLine = separatorLine == null
                                                ? string.IsNullOrWhiteSpace
                                                : enumerator.Current.Equals;

        var section = new List<string>();
        while (enumerator.MoveNext())
        {
            if (IsSeparatorLine(enumerator.Current))
            {
                yield return section.ToArray();
                section.Clear();
            }
            else
                section.Add(enumerator.Current);
        }

        yield return section.ToArray();
        section.Clear();
    }

    /** Takes sections and maps them into a header section and then all the other sections into their own mapping */
    public static (TFirst[] First, IReadOnlyList<TOthers[]> Others) MapSectionsFirstAndOthers<TFirst, TOthers>(this IEnumerable<string[]> sections,
        Func<string, TFirst> firstMapper, Func<string, TOthers> othersMapper)
    {
        return (
            First: sections.FirstOrDefault()?.Select(firstMapper)?.ToArray() ?? [],
            Others: sections.Skip(1).Select(section => section.Select(othersMapper).ToArray()).ToImmutableList()
        );
    }

    /** Takes sections and maps them into a header section and a content section */
    public static (TFirst[] First, TSecond[] Second) MapSectionsToPair<TFirst, TSecond>(this IEnumerable<string[]> sections,
        Func<string, TFirst> firstMapper, Func<string, TSecond> secondMapper)
    {
        var firstTwo = sections.Take(2).ToArray();
        if (firstTwo.Length != 2)
        {
            throw new InvalidInputFormatParsingException($"There wasn't exactly two sections");
        }
        return (
            First: [.. firstTwo[0].Select(firstMapper)],
            Second: [.. firstTwo[1].Select(secondMapper)]
        );
    }
}