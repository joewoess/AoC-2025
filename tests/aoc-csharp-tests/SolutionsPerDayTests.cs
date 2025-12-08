namespace aoc_csharp_tests;

using aoc_csharp.helper;
using TUnit.Assertions;

[NotInParallel]
[Arguments(true)]
[Arguments(false)]
public class SolutionsPerDayTests(bool isDemo)
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
}
