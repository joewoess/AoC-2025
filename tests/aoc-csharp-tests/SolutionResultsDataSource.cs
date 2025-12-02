using aoc_csharp.helper;

namespace aoc_csharp_tests;

public static class SolutionResultsDataSource
{
    public static Solutions? DemoSolutions { get; private set; } = null;
    public static Solutions? RealSolutions { get; private set; } = null;

    public static void EnsureSolutionsLoaded()
    {
        if (DemoSolutions is null || RealSolutions is null)
        {
            var before = Config.IsDemo;
            Config.IsDemo = false;
            RealSolutions = SolutionResults.LoadExpected();
            Config.IsDemo = true;
            DemoSolutions = SolutionResults.LoadExpected();
            Config.IsDemo = before;
        }
    }
}
