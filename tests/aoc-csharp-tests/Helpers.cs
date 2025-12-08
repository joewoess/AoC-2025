using aoc_csharp.helper;

namespace aoc_csharp_tests;

public enum DataConfig { DemoData, RealData }
public static class ConfigExtensions
{
    public static void SetDataSource(DataConfig dataConfig) => Config.IsDemo = dataConfig == DataConfig.DemoData;
    public static DataConfig CurrentConfig => Config.IsDemo ? DataConfig.DemoData : DataConfig.RealData;
    public static Solutions? GetCurrentSolutions() => Config.IsDemo
            ? SolutionResultsDataSource.DemoSolutions
            : SolutionResultsDataSource.RealSolutions;
}