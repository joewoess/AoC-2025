namespace aoc_csharp.puzzles;

public sealed class Day01 : PuzzleBaseLines
{

    public override string? FirstPuzzle()
    {
        var turns = Data
            .Select(line => int.Parse(line.Substring(1)) * (line[0] == 'R' ? -1 : 1))
            .ToList();

        var dialStartingValue = 50;
        Printer.DebugMsg($"Starting at: {dialStartingValue}");

        var timesDialAtZero = 0;
        var currentDial = dialStartingValue;

        foreach (var turn in turns)
        {
            currentDial += turn;
            currentDial = currentDial % 100;
            Printer.DebugMsg($"Turn: {turn}, New Value: {currentDial}");
            if (currentDial == 0)
            {
                timesDialAtZero++;
            }
        }

        Printer.DebugMsg($"Secret Password: {timesDialAtZero}");
        return timesDialAtZero.ToString();
    }

    public override string? SecondPuzzle()
    {
        var turns = Data
            .Select(line => int.Parse(line.Substring(1)) * (line[0] == 'R' ? -1 : 1))
            .ToList();

        var dialStartingValue = 50;
        Printer.DebugMsg($"Starting at: {dialStartingValue}");

        var timesDialAtZero = 0;
        var currentDial = dialStartingValue;
        foreach (var turn in turns)
        {
            var beforeTurning = currentDial;
            Util.DoTimes(
                Math.Abs(turn),
                () =>
                {
                    currentDial += turn < 0 ? -1 : 1;
                    currentDial = currentDial % 100;
                    if (currentDial == 0)
                    {
                        timesDialAtZero++;
                    }
                });
            Printer.DebugMsg($"Turn: {turn}, Dial: {beforeTurning}->{currentDial}, timesAt0: {timesDialAtZero}");
        }

        Printer.DebugMsg($"Secret Password: {timesDialAtZero}");
        return timesDialAtZero.ToString();
    }
}