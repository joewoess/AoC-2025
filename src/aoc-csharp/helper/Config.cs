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

    // Messages
    public static readonly string[] GreetingMessageLines =
    [
        $"AdventOfCode Runner for {CurrentChallengeYear}",
        $"Challenge at: https://adventofcode.com/{CurrentChallengeYear}/",
        "Author: Johannes Wöß (https://github.com/joewoess)"
    ];

    public const string NoSolutionMessage = "NO IMPL";
    public const string NoDataMessage = "NO DATA";
    public const string NoResultMessage = "NO RESULT";
    public const string SkippedMessage = "SKIPPED";

    // Constants

    public const int ResultColumnPadding = 30;
    public const int InfoColumnPadding = 15;
    public const int MaxChallengeDays = CurrentChallengeYear < 2025 ? 25 : 12; // Starting 2025, only 12 days of challenges are available
    public const int CurrentChallengeYear = 2025;
    public const char LineArtChar = '-';
    public const string ImplementationNamespace = "aoc_csharp.puzzles";
    public const string DataFileNamingConvention = "day{0:D2}.txt";
    public const string DayMessageConvention = "Day {0:D2}";
}
