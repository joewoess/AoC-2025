using Microsoft.VisualBasic;
using Spectre.Console;

namespace aoc_csharp.puzzles;

public sealed class Day07 : PuzzleBaseLines
{
    const char Start = 'S';
    const char Splitter = '^';
    const char Beam = '|';

    public override string? FirstPuzzle()
    {
        var grid = Data.AsJaggedCharGrid();
        var pointGrid = grid.AsPointDict();

        var numSplitted = 0L;

        var startPoint = pointGrid.First(pair => pair.Value == Start);
        List<Point> beams = [startPoint.Key];
        var endOfManifold = grid.Length;

        for (var beamIdx = 0; beamIdx < beams.Count; beamIdx++)
        {
            var nextStep = beams[beamIdx].StepInDirection(Direction.Down);
            if (nextStep.Y >= endOfManifold) continue;

            if (grid[nextStep.Y][nextStep.X] == Splitter)
            {
                numSplitted++;
                var left = nextStep.StepInDirection(Direction.Left);
                var right = nextStep.StepInDirection(Direction.Right);
                if (!beams.Contains(left)) beams.Add(left);
                if (!beams.Contains(right)) beams.Add(right);
            }
            else if (!beams.Contains(nextStep))
            {
                beams.Add(nextStep);
            }
        }

        beams.Remove(startPoint.Key);
        var gridWithBeams = grid.As2dArray();
        foreach (var beam in beams) gridWithBeams[beam.Y, beam.X] = Beam;

        Printer.DebugMsg("Final grid looks like:");
        Printer.DebugMsg(gridWithBeams.AsPrintable());

        Printer.DebugMsg($"Beam was split {numSplitted} times.");
        return numSplitted.ToString();
    }

    public override string? SecondPuzzle()
    {
        var grid = Data.AsJaggedCharGrid();
        var pointGrid = grid.AsPointDict();

        var startPoint = pointGrid.First(pair => pair.Value == Start);

        var gridWithBeams = grid.As2dArray().AsJaggedArray();
        var timelineCache = new Dictionary<Point, long>();
        var timelines = FollowTachyonRecursivly(startPoint.Key, gridWithBeams);
        Printer.DebugMsg("Final grid looks like:");
        Printer.DebugMsg(gridWithBeams.AsPrintable());

        Printer.DebugMsg($"There were {timelines} timelines.");
        return timelines.ToString();
    }
    private readonly Dictionary<Point, long> TimelineCache = [];
    private long FollowTachyonRecursivly(Point startingPoint, char[][] grid)
    {
        var nextStep = startingPoint.StepInDirection(Direction.Down);
        if (nextStep.Y > grid.Length - 1) return 1L;

        if (grid[nextStep.Y][nextStep.X] == Splitter)
        {
            var left = nextStep.StepInDirection(Direction.Left);
            var right = nextStep.StepInDirection(Direction.Right);
            long leftTimelines;
            if (TimelineCache.TryGetValue(left, out long leftCached)) leftTimelines = leftCached;
            else
            {
                TimelineCache[left] = FollowTachyonRecursivly(left, grid);
                leftTimelines = TimelineCache[left];
                grid[left.Y][left.X] = Beam;
            }
            long rightTimelines;
            if (TimelineCache.TryGetValue(right, out long rightCached)) rightTimelines = rightCached;
            else
            {
                TimelineCache[right] = FollowTachyonRecursivly(right, grid);
                rightTimelines = TimelineCache[right];
                grid[right.Y][right.X] = Beam;
            }
            return leftTimelines + rightTimelines;
        }
        if (TimelineCache.TryGetValue(nextStep, out long nextCached)) return nextCached;
        else
        {
            TimelineCache[nextStep] = FollowTachyonRecursivly(nextStep, grid);
            grid[nextStep.Y][nextStep.X] = Beam;
            return TimelineCache[nextStep];
        }
    }


    // var endOfManifold = grid.Length;

    // Dictionary<Point, int> beams = new() { { startPoint.Key, 1 } };
    // for (var beamIdx = 0; beamIdx < beams.Count; beamIdx++)
    // {
    //     var currentBeam = beams.ElementAt(beamIdx);
    //     var nextStep = currentBeam.Key.StepInDirection(Direction.Down);
    //     if (nextStep.Y >= endOfManifold) continue;

    //     if (grid[nextStep.Y][nextStep.X] == Splitter)
    //     {
    //         var left = nextStep.StepInDirection(Direction.Left);
    //         var right = nextStep.StepInDirection(Direction.Right);
    //         beams[left] = beams.TryGetValue(left, out int lVal) ? lVal + currentBeam.Value : 1;
    //         beams[right] = beams.TryGetValue(left, out int rVal) ? rVal + currentBeam.Value : 1;
    //     }
    //     else if (!beams.ContainsKey(nextStep))
    //     {
    //         beams[nextStep] = currentBeam.Value;
    //     }
    // }

    // beams.Remove(startPoint.Key);
    // var gridWithBeams = grid.As2dArray();
    // foreach (var (B, T) in beams) gridWithBeams[B.Y, B.X] = Beam;

    // Printer.DebugMsg("Final grid looks like:");
    // Printer.DebugMsg(gridWithBeams.AsPrintable());



    // var timelines = 0;
    // foreach (var leafBeam in beams.Where(b => b.Key.Y == endOfManifold - 1))
    // {
    //     timelines += leafBeam.Value;
    //     Printer.DebugMsg($"[{leafBeam.Key}]: {leafBeam.Value} timelines");
    // }
}