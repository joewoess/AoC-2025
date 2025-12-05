using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Spectre.Console;

namespace aoc_csharp.puzzles;

public sealed class Day05 : PuzzleBaseLines
{
    public override string? FirstPuzzle()
    {
        var freshRanges = new List<(BigInteger From, BigInteger To)>();
        int lineIdx = 0;
        for (; lineIdx < Data.Length; lineIdx++)
        {
            if (string.IsNullOrWhiteSpace(Data[lineIdx]))
            {
                lineIdx++;
                break;
            }
            freshRanges.Add(Data[lineIdx].SplitAndMapToPair(BigInteger.Parse, "-"));
        }

        var ingredientList = Data.Skip(lineIdx).Select(BigInteger.Parse).ToList();

        var freshIngredients = 0;

        Printer.DebugPrintExcerpt(freshRanges, "Fresh ingredient ranges: ");
        Printer.DebugPrintExcerpt(ingredientList, "Ingredients to check: ");

        foreach (var ingredient in ingredientList)
        {
            if (freshRanges.Any(range => ingredient.Between(range.From, range.To, inclEnd: true))) freshIngredients++;
        }

        Printer.DebugMsg($"Num fresh ingredients: {freshIngredients}");
        return freshIngredients.ToString();
    }

    private record class Interval(BigInteger From, BigInteger To);

    public override string? SecondPuzzle()
    {
        var freshRanges = new List<Interval>();
        for (var lineIdx = 0; lineIdx < Data.Length; lineIdx++)
        {
            if (string.IsNullOrWhiteSpace(Data[lineIdx]))
            {
                lineIdx++;
                break;
            }
            var (From, To) = Data[lineIdx].SplitAndMapToPair(BigInteger.Parse, "-");
            freshRanges.Add(new Interval(From, To));
        }

        var freshRangesSorted = freshRanges.OrderBy(r => r.From).ToList();
        for (var minIdx = 0; minIdx < freshRangesSorted.Count; minIdx++)
        {
            for (var checkIdx = minIdx + 1; checkIdx < freshRangesSorted.Count; checkIdx++)
            {
                if (freshRangesSorted[minIdx].To >= freshRangesSorted[checkIdx].From)
                {
                    // range is wholly encompassed, remove it
                    if (freshRangesSorted[minIdx].To >= freshRangesSorted[checkIdx].To)
                    {
                        freshRangesSorted.RemoveAt(checkIdx--);
                        continue;
                    }
                    // range min can be set to new minimum
                    else
                    {
                        freshRangesSorted[checkIdx] = freshRangesSorted[checkIdx] with
                        {
                            From = freshRangesSorted[minIdx].To + 1
                        };
                    }
                }
                // since sorted no need to check the rest
                else { break; }
            }
        }
        Printer.DebugPrintExcerpt(freshRangesSorted, "Sorted ranges without duplicates: ", separator: "\n");

        var freshIngredients = freshRangesSorted
                            .Select(range => range.To - range.From + 1) // +1 since inclusive borders
                            .Aggregate((one, two) => one + two); // aggregate since .Sum() is not valid for BigInteger

        Printer.DebugMsg($"Num fresh ingredients: {freshIngredients}");
        return freshIngredients.ToString();
    }
}