namespace aoc_csharp.helper;

/// <summary>
/// Provides application-wide configuration constants and settings for data paths, runtime options, and message
/// templates.
/// </summary>
/// <remarks>This class contains static fields and constants used throughout the application to control behavior,
/// specify file locations, and define standard messages. The values are intended to be accessed directly and are not
/// intended to be modified at runtime unless explicitly marked as non-const. This class is not intended to be
/// instantiated.</remarks>
public static class Config
{
    // Paths

    public const string InputPathReal = "../../data/real/";
    public const string InputPathDemo = "../../data/demo/";
    private const string OutputHeaderPath = "../../HEADER";

    // Settings

    public static bool IsDebug = false;
    public static bool IsDemo = false;
    public static bool ShowLast = false;
    public static bool ShowFirst = true;
    public static bool ShowSecond = true;
    public static bool IsInRelease = false;
    public static bool OnlyTestCode = false;
    public static bool RunBenchmarks = false;
    public static bool SkipLongRunning = false;

    public static readonly bool TryAndUseConsoleWidth = true;
    public static readonly bool PrintAfterLastImpl = false;

    // Messages
    public static readonly string[] GreetingMessageLines = File.ReadAllLines(OutputHeaderPath);

    public const string NoSolutionMessage = "NO IMPL";
    public const string NoDataMessage = "NO DATA";
    public const string NoResultMessage = "NO RESULT";
    public const string SkippedMessage = "SKIPPED";

    // Constants

    public const int ResultColumnPadding = 30;
    public const int InfoColumnPadding = 15;
    public const int MaxChallengeDays = 25;
    public const char LineArtChar = '-';
    public const string ImplementationNamespace = "aoc_csharp.puzzles";
    public const string BenchmarkNamespace = "aoc_csharp.benchmarks";
    public const string DataFileNamingConvention = "day{0:D2}.txt";
    public const string DayMessageConvention = "Day {0:D2}";
}
