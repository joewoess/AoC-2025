namespace aoc_csharp.puzzles;

public sealed class Day06 : PuzzleBaseLines
{
    private readonly Func<string, Func<long, long, long>> operationMapper = (op) => op switch
    {
        "*" => (val1, val2) => val1 * val2,
        "+" => (val1, val2) => val1 + val2,
        _ => throw new ArgumentException($"Op '{op}' was not a valid op"),
    };

    public override string? FirstPuzzle()
    {
        var grid = Data
                    .SkipLast(1)
                    .Select(line => line.Split(" ").WhereNot(string.IsNullOrWhiteSpace).Select(long.Parse).ToArray())
                    .ToArray();
        var operations = Data[^1].Split(" ").WhereNot(string.IsNullOrWhiteSpace).Select(operationMapper).ToArray();

        var sumResults = 0L;

        Printer.DebugMsg(grid.AsPrintable(separator: " "));

        var perLine = grid.Transpose(0);
        foreach (var (line, index) in perLine.WithIndex())
        {
            var lineValue = line.Aggregate(operations[index]);
            Printer.DebugMsg($"Eval [{index}]: {lineValue}");
            sumResults += lineValue;
        }

        Printer.DebugMsg($"Results sum up to: {sumResults}");
        return sumResults.ToString();
    }

    public override string? SecondPuzzle()
    {
        var grid = Data;

        var sumResults = 0L;
        var maxLineLength = grid.Max(line => line.Length);

        Func<long, long, long> lastOp = (v1, v2) => 0;
        List<long> currentValues = [];
        for (int colIdx = 0; colIdx < maxLineLength; colIdx++)
        {
            var verticalNumber = new string([.. Data[0..^1].Select(line => line[colIdx])]);
            var op = Data[^1][colIdx].ToString();

            if (!string.IsNullOrWhiteSpace(op)) lastOp = operationMapper(op); // new operation
            if (string.IsNullOrWhiteSpace(verticalNumber)) // spacing between columns
            {
                var colResult = currentValues.Count == 0 ? 0 : currentValues.Aggregate(lastOp);
                Printer.DebugMsg($"Eval: {colResult}");
                sumResults += colResult;

                currentValues.Clear();
                continue;
            }
            var num = long.Parse(verticalNumber);
            currentValues.Add(num);
            Printer.DebugMsg($"Col[{colIdx,2}]: {num,4}");
        }
        // since after last col there is no more filler spacing, add last
        var lastResult = currentValues.Aggregate(lastOp);
        Printer.DebugMsg($"Eval: {lastResult}");
        sumResults += lastResult;

        Printer.DebugMsg($"Results sum up to: {sumResults}");
        return sumResults.ToString();
    }
}