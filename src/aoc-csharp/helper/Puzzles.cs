using System.Reflection;

namespace aoc_csharp.helper;

public static class Puzzles
{
    /** Gets all puzzles in the csharp.puzzles folder by naming convention 'DayXX.cs' */
    public static List<IPuzzle> ImplementedPuzzles()
    {
        return GetImplementedTypesFromNamespace()
            .Select(GetPuzzle)
            .Where(Util.IsNotNull)
            .Cast<IPuzzle>()
            .ToList();
    }

    /** Static placeholder if there is no implementation */
    public static NoImplPuzzle NoImplementation { get; } = new();

    /** Instance the type found by reflection */
    private static IPuzzle? GetPuzzle(Type targetType) => (IPuzzle?)Activator.CreateInstance(targetType);

    /** Gets all implementations in the csharp.puzzles folder that use the IPuzzles interface */
    private static IEnumerable<Type> GetImplementedTypesFromNamespace()
    {
        return Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false })
            .Where(t => string.Equals(t.Namespace, Config.ImplementationNamespace, StringComparison.OrdinalIgnoreCase))
            .Where(t => !t.Name.Contains('<') && t.Name.StartsWith("Day"))
            .OrderBy(t => t.Name);
    }

    /** Group the puzzles by day */
    public static Dictionary<int, List<IPuzzle>> PuzzleImplementationDict()
    {
        return Util.Range(1, Config.MaxChallengeDays)
            .ToDictionary(
                day => day,
                day => ImplementedPuzzles()
                    .Where(puzzle => ParseDay(puzzle.TypeName) == day)
                    .ToList());
    }

    /** Parse the day integer from a typename */
    private static int? ParseDay(string input) => int.TryParse(string.Join("", input.Where(char.IsDigit)), out var day) ? day : null;
}