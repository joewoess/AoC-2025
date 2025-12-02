namespace aoc_csharp_tests;

using aoc_csharp.helper;
using TUnit.Assertions;

[Arguments(true)]
[Arguments(false)]
[NotInParallel]
public class SolutionResultTests(bool isDemo)
{
    private Solutions? SolutionsForConfig => isDemo
        ? SolutionResultsDataSource.DemoSolutions
        : SolutionResultsDataSource.RealSolutions;

    [Before(Test)]
    public void SetupDemo()
    {
        Config.IsDemo = isDemo;
    }

    [Test]
    public async Task CouldParseSolutionsData()
    {
        await Assert.That(File.Exists(Config.InputPathSolutionsMd)).IsTrue();
        Assert.NotNull(SolutionsForConfig);
        await Assert.That(SolutionsForConfig).IsNotEmpty();
    }

    [Test, DependsOn(nameof(CouldParseSolutionsData))]
    public async Task All_Puzzles_Should_Have_Valid_Solutions()
    {
        Assert.NotNull(SolutionsForConfig); // here to satisfy the compiler
        var puzzleDict = Puzzles.PuzzleImplementationDict();

        await Assert.That(puzzleDict).IsNotEmpty();

        Printer.DebugMsg($"Found implementations for {puzzleDict.Count} days.");
        Printer.DebugMsg($"Found solutions for {SolutionsForConfig.Count} days.");

        var dictWithBoth = puzzleDict
            .Where(kvp => SolutionsForConfig.ContainsKey(kvp.Key) && kvp.Value.Count > 0)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        await Assert.That(dictWithBoth).IsNotEmpty();

        foreach (var (day, puzzlesOfDay) in dictWithBoth)
        {
            foreach (var puzzle in puzzlesOfDay)
            {
                Printer.DebugMsg($"Testing Day {day} with {puzzle.TypeName}...");
                await Assert.That(puzzle.FirstResult).IsNull().Or.IsEqualTo(SolutionsForConfig[day].Part1);
                await Assert.That(puzzle.SecondResult).IsNull().Or.IsEqualTo(SolutionsForConfig[day].Part2);
                Printer.DebugMsg($"...passed for Day {day} with {puzzle.TypeName}.");
            }
        }

        Printer.DebugMsg("All tests passed!");
    }
}
