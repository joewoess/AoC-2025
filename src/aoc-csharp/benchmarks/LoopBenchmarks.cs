using BenchmarkDotNet.Attributes;

namespace aoc_csharp.benchmarks;

[Config(typeof(BenchmarkConfig))]
public class LoopBenchmarks
{
    private static readonly List<string> TestData = Util.InitializeListWithDefault(10_000, () => Random.Shared.Next().ToString());
    private const int LoopCount = 100_000;

    [Benchmark(Baseline = true)]
    public void ForLoop()
    {
        var idx = 0;
        for (int i = 0; i < LoopCount; i++)
        {
            idx++;
        }
        if (idx != LoopCount) throw new Exception();
    }

    [Benchmark]
    public void ForEachLoop()
    {
        var idx = 0;
        foreach (var _ in Enumerable.Range(0, LoopCount))
        {
            idx++;
        }
        if (idx != LoopCount) throw new Exception();
    }

    [Benchmark]
    public void DoTimesLoop()
    {
        var idx = 0;
        LoopCount.DoTimes(() => idx++);
        if (idx != LoopCount) throw new Exception();
    }

    [Benchmark]
    public void WhileLoop()
    {
        var idx = 0;
        var i = 0;
        while (i < LoopCount)
        {
            idx++;
            i++;
        }
        if (idx != LoopCount) throw new Exception();
    }
}