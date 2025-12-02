using System.Reflection;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Jobs;

namespace aoc_csharp.helper;

public class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        AddJob(Job.InProcessDontLogOutput); // Use this to test for benchmarking results
        //AddJob(Job.Dry); // Use this to test if benchmarks are valid
        AddLogger(NullLogger.Instance);
        AddColumnProvider(DefaultColumnProviders.Instance);
        AddDiagnoser(MemoryDiagnoser.Default);
        WithUnionRule(ConfigUnionRule.AlwaysUseLocal);
        WithOption(ConfigOptions.LogBuildOutput, false);
        WithOption(ConfigOptions.DisableLogFile, true);
        WithOption(ConfigOptions.DisableOptimizationsValidator, true);
        WithOption(ConfigOptions.KeepBenchmarkFiles, false);
        WithOption(ConfigOptions.GenerateMSBuildBinLog, false);
        WithArtifactsPath("../../dump/benchmarks/");
    }

    /** Gets all Benchmark classes */
    public static IEnumerable<Type> GetBenchmarks()
    {
        return Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false })
            .Where(t => string.Equals(t.Namespace, Config.BenchmarkNamespace, StringComparison.OrdinalIgnoreCase))
            .WhereNot(t => t.Name.Contains('<'))
            .OrderBy(t => t.Name);
    }

    /** Check if assembly is in release more or else benchmarks wont run */
    public static bool IsInReleaseConfiguration()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var attributes = assembly.GetCustomAttributes(false);
        return attributes.Any(a => a is AssemblyConfigurationAttribute { Configuration: "Release" });
    }

    /** Prints the result of the benchmark runner */
    public static void PrintBenchmarkSummary(BenchmarkDotNet.Reports.Summary summary)
    {
        MarkdownExporter.Console.ExportToLog(summary, ConsoleLogger.Default);
    }
}