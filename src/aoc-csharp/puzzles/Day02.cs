namespace aoc_csharp.puzzles;

public sealed class Day02 : PuzzleBaseText
{
    public override string? FirstPuzzle()
    {
        var ranges = Data
            .Split(",")
            .Select(x => x.SplitAndMapToPair(long.Parse, separator: "-"))
            .ToList();

        Printer.DebugPrintExcerpt(ranges, "Ranges: ");

        var sumInvalidIds = 0L;

        foreach (var range in ranges)
        {
            for (var id = range.First; id <= range.Second; id++)
            {
                var idStr = id.ToString();
                var idStrLen = idStr.Length;
                if (idStrLen % 2 == 0 && idStr.Substring(0, idStrLen / 2).Equals(idStr.Substring(idStrLen / 2)))
                {
                    sumInvalidIds += id;
                    Printer.DebugMsg($"Invalid id: {idStr}");
                    continue;
                }
            }
        }

        Printer.DebugMsg($"Sum of invalid ids: {sumInvalidIds}");
        return sumInvalidIds.ToString();
    }

    public override string? SecondPuzzle()
    {
        var ranges = Data
            .Split(",")
            .Select(x => x.SplitAndMapToPair(long.Parse, separator: "-"))
            .ToList();

        Printer.DebugPrintExcerpt(ranges, "Ranges: ");

        var sumInvalidIds = 0L;

        foreach (var range in ranges)
        {
            for (var id = range.First; id <= range.Second; id++)
            {
                var idStr = id.ToString();
                var idStrLen = idStr.Length;
                for (var seqLen = 1; seqLen <= idStrLen / 2; seqLen++)
                {
                    if (idStrLen % seqLen != 0) continue;
                    var sequences = idStr.Chunk(seqLen).Select(seq => new string(seq)).ToList();

                    if (sequences.All(s => s.Equals(sequences[0]))) // all sequences are the same
                    {
                        sumInvalidIds += id;
                        Printer.DebugMsg($"Invalid id: {idStr}");
                        break;
                    }
                }
            }
        }

        Printer.DebugMsg($"Sum of invalid ids: {sumInvalidIds}");
        return sumInvalidIds.ToString();
    }
}