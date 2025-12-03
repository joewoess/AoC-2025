using Spectre.Console;

namespace aoc_csharp.puzzles;

public sealed class Day03 : PuzzleBaseLines
{
    public override string? FirstPuzzle()
    {
        var banks = Data;
        var sumJoltage = 0;

        foreach (var bank in banks)
        {
            var highestFirstDigit = bank.SkipLast(1).Max();
            var remainingDigits = bank.Skip(bank.IndexOf(highestFirstDigit) + 1);
            var highestSecondDigit = remainingDigits.Max();
            var joltage = int.Parse($"{highestFirstDigit}{highestSecondDigit}");
            sumJoltage += joltage;
            Printer.DebugMsg($"Joltage in {bank}: {joltage}");
        }

        Printer.DebugMsg($"Sum of maximum joltages: {sumJoltage}");
        return sumJoltage.ToString();
    }

    public override string? SecondPuzzle()
    {
        var banks = Data;
        var sumJoltage = 0L;
        const int NumBatteries = 12;

        foreach (var bank in banks)
        {
            var batteryList = new List<char>(NumBatteries);
            var indexOfLastEntry = -1;
            while (batteryList.Count != NumBatteries)
            {
                var (nextHighDigit, indexDigitAdded) = bank.WithIndex()
                    .Skip(indexOfLastEntry + 1)
                    .SkipLast(NumBatteries - 1 - batteryList.Count)
                    .MaxBy(pair => pair.item);
                batteryList.Add(nextHighDigit);
                indexOfLastEntry = indexDigitAdded;
            }
            var joltage = long.Parse(new string([.. batteryList]));
            sumJoltage += joltage;
            Printer.DebugMsg($"Joltage in {bank}: {joltage}");
        }

        Printer.DebugMsg($"Sum of maximum joltages: {sumJoltage}");
        return sumJoltage.ToString();
    }
}