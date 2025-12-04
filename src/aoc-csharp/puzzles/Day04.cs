using Spectre.Console;

namespace aoc_csharp.puzzles;

public sealed class Day04 : PuzzleBaseLines
{
    public override string? FirstPuzzle()
    {
        const int maxNeighborRollsExcl = 4;
        var grid = Data.Select(line => line.ToCharArray()).ToArray();
        var countAccessibleRolls = 0;

        Printer.DebugMsg(Grids.AsPrintable(grid));

        var gridAsDict = grid.AsPointDict(validPointCheck: c => c != '.');
        foreach (var roll in gridAsDict)
        {
            var neighbors = roll.Key.GetNeighborPoints().Count(gridAsDict.Keys.Contains);
            Printer.DebugMsg($"Available roll at: {roll.Key}");
            if (neighbors < maxNeighborRollsExcl) countAccessibleRolls++;
        }

        Printer.DebugMsg($"Num available Rolls: {countAccessibleRolls}");
        return countAccessibleRolls.ToString();
    }

    public override string? SecondPuzzle()
    {
        const int maxNeighborRollsExcl = 4;
        var grid = Data.Select(line => line.ToCharArray()).ToArray();
        var countRemovedRolls = 0;

        var gridAsDict = grid.AsPointDict(validPointCheck: c => c != '.');
        var toBeRemoved = new List<Point>();
        do
        {
            toBeRemoved.ForEach(p => gridAsDict.Remove(p));
            countRemovedRolls += toBeRemoved.Count;
            toBeRemoved.Clear();
            foreach (var roll in gridAsDict)
            {
                var neighbors = roll.Key.GetNeighborPoints().Count(gridAsDict.Keys.Contains);
                if (neighbors < maxNeighborRollsExcl) toBeRemoved.Add(roll.Key);
            }
        } while (toBeRemoved.Count > 0);

        Printer.DebugMsg($"Num removed rolls: {countRemovedRolls}");
        return countRemovedRolls.ToString();
    }
}