using aoc_csharp.helper;

namespace aoc_csharp_tests;

public class ImplementationsDataSource
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

    /// <summary>
    /// Provides test days dynamically based on implemented puzzles.
    /// Only returns days that have implementations.
    /// </summary>
    public static IEnumerable<int> GetDaysWithImplementations()
    {
        EnsureLoaded();
        return ImplementationsPerDay?.Keys.Order() ?? Enumerable.Empty<int>();
    }
}