using BenchmarkDotNet.Attributes;

namespace aoc_csharp.benchmarks;

[Config(typeof(BenchmarkConfig))]
public class PairWithNextBenchmarks
{
    private static readonly List<string> TestData =
        Util.InitializeListWithDefault(10_000, () => Random.Shared.Next().ToString());

    [Benchmark(Baseline = true)]
    public void UseOldPairWithNext()
    {
        TestData.PairWithNextDeprecated().ToList();
    }

    [Benchmark]
    public void UseNewPairWithNext()
    {
        TestData.PairWithNext().ToList();
    }
}