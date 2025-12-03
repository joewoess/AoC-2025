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

    public const string InputPathData = "./";
    public const string InputPathReal = $"{InputPathData}real/";
    public const string InputPathDemo = $"{InputPathData}demo/";
    public const string InputPathSolutionsMd = $"{InputPathData}solutions.md";

    // Settings

#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static bool IsDebug = false;
    public static bool IsDemo = false;
    public static bool ShowLast = false;
    public static bool ShowFirst = true;
    public static bool ShowSecond = true;
    public static bool SkipLongRunning = false;
#pragma warning restore CA2211 // Non-constant fields should not be visible

    public static readonly bool TryAndUseConsoleWidth = true;
    public static readonly bool PrintAfterLastImpl = false;

    public const string NoSolutionMessage = "NO IMPL";
    public const string NoDataMessage = "NO DATA";
    public const string NoResultMessage = "NO RESULT";
    public const string SkippedMessage = "SKIPPED";

    // Constants

    public const int ResultColumnRatio = 30;
    public const int InfoColumnRatio = 5;
    public const int TimingColumnRatio = 10;
    public const int MaxChallengeDays = CurrentChallengeYear < 2025 ? 25 : 12; // Starting 2025, only 12 days of challenges are available
    public const int CurrentChallengeYear = 2025;
    public const string ImplementationNamespace = "aoc_csharp.puzzles";
    public const string DataFileNamingConvention = "day{0:D2}.txt";
    public const string DayMessageConvention = "Day {0:D2}";

    // Application Constants

    public const string ApplicationReadableName = "AdventOfCode C# Runner:";
    public const string ApplicationVersion = "1.0.0";
    public const string AuthorName = "Johannes Wöß";
    public const string AuthorGithubRepo = "https://github.com/joewoess/";

}
