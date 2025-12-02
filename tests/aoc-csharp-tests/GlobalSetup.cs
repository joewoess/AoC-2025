[assembly: System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]

namespace aoc_csharp_tests;

public class GlobalHooks
{
    [Before(TestSession)]
    public static void SetUp()
    {
        SolutionResultsDataSource.EnsureSolutionsLoaded();
    }
}
