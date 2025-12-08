namespace aoc_csharp_tests;

using aoc_csharp.helper;
using TUnit.Assertions;

[NotInParallel]
[Arguments(true)]
[Arguments(false)]
public class SolutionResultTests(bool isDemo)
{
    private Solutions? SolutionsForConfig => isDemo
        ? SolutionResultsDataSource.DemoSolutions
        : SolutionResultsDataSource.RealSolutions;

    private readonly Dictionary<int, List<IPuzzle>>? ImplementationDict
        = ImplementationsDataSource.ImplementationsPerDay;

    [Before(Test)]
    public void SetupDemo()
    {
        Config.IsDemo = isDemo;
        Console.WriteLine();
    }

    [Test]
    public async Task CouldParseSolutionsData()
    {
        await Assert.That(File.Exists(Config.InputPathSolutionsMd)).IsTrue();
        Assert.NotNull(SolutionsForConfig);
        await Assert.That(SolutionsForConfig).IsNotEmpty();
    }
    [Test]
    public async Task CouldParsePuzzleImplementations()
    {
        Assert.NotNull(ImplementationDict);
        await Assert.That(ImplementationDict).IsNotEmpty();
    }

    [Test, DependsOn(nameof(CouldParseSolutionsData)), DependsOn(nameof(CouldParsePuzzleImplementations))]
    public async Task All_Puzzles_Should_Have_Valid_Solutions()
    {
        Assert.NotNull(SolutionsForConfig); // here to satisfy the compiler
        var puzzleDict = Puzzles.PuzzleImplementationDict();

        await Assert.That(puzzleDict).IsNotEmpty();

        Console.WriteLine($"Found implementations for {puzzleDict.Count(d => d.Value.Count > 0)} out of {puzzleDict.Count} days.");
        Console.WriteLine($"Found solutions for {SolutionsForConfig.Count} out of {puzzleDict.Count} days.");
        Console.WriteLine();

        var dictWithBoth = puzzleDict
            .Where(kvp => SolutionsForConfig.ContainsKey(kvp.Key) && kvp.Value.Count > 0)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        await Assert.That(dictWithBoth).IsNotEmpty();

        foreach (var (day, puzzlesOfDay) in dictWithBoth)
        {
            foreach (var puzzle in puzzlesOfDay)
            {
                Console.Write($"Testing Day {day} with implementation {puzzle.TypeName}: ");
                await Assert.That(puzzle.FirstResult).IsNull().Or.IsEqualTo(SolutionsForConfig[day].Part1);
                await Assert.That(puzzle.SecondResult).IsNull().Or.IsEqualTo(SolutionsForConfig[day].Part2);
                Console.WriteLine($"[PASSED]");
            }
        }
    }
}
