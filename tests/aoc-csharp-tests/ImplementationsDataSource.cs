using aoc_csharp.helper;
using TUnit.Core.DataSources;

namespace aoc_csharp_tests;

public static class ImplementationsDataSource
{
    public static Dictionary<int, List<IPuzzle>>? ImplementationsPerDay { get; private set; } = null;

    public static void EnsureLoaded()
    {
        if (ImplementationsPerDay is null)
        {
            ImplementationsPerDay = Puzzles.PuzzleImplementationDict()
                .Where(kvp => kvp.Value.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}