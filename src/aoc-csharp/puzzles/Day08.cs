using System.Collections.Immutable;
using System.Diagnostics;
using System.Numerics;
using Microsoft.VisualBasic;
using Spectre.Console;

namespace aoc_csharp.puzzles;

public sealed class Day08 : PuzzleBaseLines
{
    private record class Connection(Vector3 From, Vector3 To, double Distance);

    public override string? FirstPuzzle()
    {
        var numConnections = Config.IsDemo ? 10 : 1000;
        const int numTopCircuits = 3;

        var coords = Data.Select(line => line.Split(',').Select(int.Parse).ToArray())
                        .Select(coords => new Vector3(coords[0], coords[1], coords[2]))
                        .ToImmutableArray();

        var connections = coords.MapSelfCrossProduct((a, b) => new Connection(a, b, (b - a).Length()), ignoreSelfMap: true).ToList();
        var connectionsToMake = connections.OrderBy(c => c.Distance).Take(numConnections).ToList();
        var circuits = coords.Select(c => new List<Vector3>() { c }).ToList();

        foreach (var connection in connectionsToMake)
        {
            var from = circuits.Find(c => c.Contains(connection.From));
            var to = circuits.Find(c => c.Contains(connection.To));
            if (from == null || to == null) throw new ImplementationException();
            if (Equals(from, to)) continue;

            var idxNew = Math.Min(circuits.IndexOf(from), circuits.IndexOf(to));
            circuits.Remove(from);
            circuits.Remove(to);
            circuits.Insert(idxNew, [.. from.Union(to)]);
            if (circuits.Count < 10) Printer.DebugPrintExcerpt(circuits.Select(c => c.Count), "Circuit counts towards the end: ");
        }

        var topCircuits = circuits.OrderByDescending(c => c.Count).Take(numTopCircuits).ToArray();
        Printer.DebugPrintExcerpt(topCircuits.Select(c => c.Count), "Top 3 sizes");

        var result = topCircuits.Aggregate(1, (a, b) => a * b.Count);

        Printer.DebugMsg($"Top 3 sizes multiply to: {result}");
        return result.ToString();
    }

    public override string? SecondPuzzle()
    {
        var coords = Data.Select(line => line.Split(',').Select(int.Parse).ToArray())
                        .Select(coords => new Vector3(coords[0], coords[1], coords[2]))
                        .ToImmutableArray();

        var connections = coords.MapSelfCrossProduct((a, b) => new Connection(a, b, (b - a).Length()), ignoreSelfMap: true).ToList();
        var connectionsToMake = connections.OrderBy(c => c.Distance).ToList();
        var circuits = coords.Select(c => new List<Vector3>() { c }).ToList();

        Connection? mvpConnection = null;
        foreach (var connection in connectionsToMake)
        {
            var from = circuits.Find(c => c.Contains(connection.From));
            var to = circuits.Find(c => c.Contains(connection.To));
            if (from == null || to == null) throw new ImplementationException();
            if (from == to) continue;

            var idxNew = Math.Min(circuits.IndexOf(from), circuits.IndexOf(to));
            circuits.Remove(from);
            circuits.Remove(to);
            circuits.Insert(idxNew, [.. from.Union(to)]);
            if (circuits.Count < 10) Printer.DebugPrintExcerpt(circuits.Select(c => c.Count), "Circuit counts towards the end: ");
            if (circuits.Count == 1)
            {
                mvpConnection = connection;
                break;
            }
        }

        // < 170629060 ?? 
        if (mvpConnection == null) throw new ImplementationException();
        Printer.DebugMsg($"Final connection: {mvpConnection}");
        var result = mvpConnection.From.X * mvpConnection.To.X;

        Printer.DebugMsg($"Final connection had this X * X result: {result}");
        return result.ToString();
    }
}