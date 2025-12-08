namespace aoc_csharp_tests;

using aoc_csharp.helper;
using TUnit.Assertions;

// Cant run in parallel for inputs since the choice of which data is selected is a static variable 
// but can be parallel per day
[NotInParallel]
[Arguments(DataConfig.DemoData)]
[Arguments(DataConfig.RealData)]
public class SolutionsPerDayTests(DataConfig dataSource)
{
    [Before(Test)]
    public void Setup()
    {
        ConfigExtensions.SetDataSource(dataSource);
    }

    [Test]
    [MethodDataSource<ImplementationsDataSource>(nameof(ImplementationsDataSource.GetDaysWithImplementations))]
    public async Task Check_Solution_For_Day(int day)
    {
        var solutionsForConfig = ConfigExtensions.GetCurrentSolutions();
        Assert.NotNull(solutionsForConfig);

        var puzzleDict = Puzzles.PuzzleImplementationDict();

        // Skip if this day doesn't have both implementations and solutions
        if (!puzzleDict.ContainsKey(day) || !solutionsForConfig.ContainsKey(day) || puzzleDict[day].Count == 0)
        {
            return;
        }

        var puzzlesOfDay = puzzleDict[day];
        var expectedSolution = solutionsForConfig[day];

        foreach (var puzzle in puzzlesOfDay)
        {
            await Assert.That(puzzle.FirstResult).IsNull().Or.IsEqualTo(expectedSolution.Part1);
            await Assert.That(puzzle.SecondResult).IsNull().Or.IsEqualTo(expectedSolution.Part2);
        }
    }
}
