namespace aoc_csharp.helper;

/// <summary>
/// Provides static methods for formatting and printing structured output, such as headers, solution results, and debug
/// information, to the console.
/// </summary>
/// <remarks>The Printer class is designed to support console-based applications that display tabular results,
/// such as puzzle solutions or reports. It includes methods for printing formatted tables, headers, and debug messages,
/// and allows customization of console width and output formatting. All members are static and thread safety is not
/// guaranteed for concurrent modifications of shared configuration properties.</remarks>
public static class Printer
{
    /// <summary>
    ///     Column headers for the results table 
    /// </summary>
    private static IEnumerable<(string Message, int Padding)> ColumnHeaders { get; } =
    [
        ("Day", Config.InfoColumnPadding),
        ("Type", Config.InfoColumnPadding),
        ("1st", Config.ResultColumnPadding),
        ("2nd", Config.ResultColumnPadding)
    ];

    /// <summary>
    ///     The estimated width of the console window use for printing the separator line
    /// </summary>
    public static int ConsoleWidth { get; set; } = 105;

    /// <summary>
    ///     Try and update the console width. This can fail if the console isn't a real console, e.g. when running emulated
    /// </summary>
    /// <returns>True if successfully updated, otherwise false</returns>
    public static bool TryUpdateConsoleWidth()
    {
        try
        {
            ConsoleWidth = Console.WindowWidth;
            return true;
        }
        catch
        {
            DebugMsg($"Could not set console width. Defaulting to {ConsoleWidth} (Covers early debug logs)");
        }

        return false;
    }

    /// <summary>
    ///     Prints the beginning header of console output
    /// </summary>
    public static void PrintGreeting()
    {
        PrintSeparator();
        var lines = Config.GreetingMessageLines;
        var longestLine = lines.Max(line => line.Length);
        var (padLeft, padRight) = ((ConsoleWidth - longestLine) / 2, (ConsoleWidth - 3 + longestLine) / 2);

        Console.WriteLine(string.Join(Environment.NewLine, lines.Select(line => $"|{new string(' ', padLeft)}{line.PadRight(padRight)}|")));
        PrintSeparator();
    }

    /// <summary>
    ///     Prints the result of a days puzzle in the result table
    /// </summary>
    /// <param name="day">Integer number corresponding to a days number (usually 1-25/1-12)</param>
    public static void PrintSolutionMessage(int day)
    {
        var implementationsOfDay = Puzzles.PuzzleImplementationDict()[day];
        foreach (var impl in implementationsOfDay)
        {
            var firstPuzzle = Config.ShowFirst && !(Config.SkipLongRunning && impl.FirstIsLongRunning()) ? impl.FirstResult : Config.SkippedMessage;
            var secondPuzzle = Config.ShowSecond && !(Config.SkipLongRunning && impl.SecondIsLongRunning()) ? impl.SecondResult : Config.SkippedMessage;

            var columnsToPrint = new (string Message, int Padding)[]
            {
                (string.Format(Config.DayMessageConvention, day), Config.InfoColumnPadding),
                (impl.TypeName, Config.InfoColumnPadding),
                (firstPuzzle, Config.ResultColumnPadding),
                (secondPuzzle, Config.ResultColumnPadding)
            }.Select(pair => pair.Message.PadLeft(pair.Padding));

            Console.WriteLine($"| {string.Join(" | ", columnsToPrint)} |");
        }
    }

    /// <summary>
    ///     Prints the result of all days in the result table
    /// </summary>
    public static void PrintAllSolutionMessages()
    {
        if (!Config.PrintAfterLastImpl)
        {
            var lastDay = Puzzles.PuzzleImplementationDict()
                .Last(entry => !(entry.Value.Count > 0 && entry.Value.First() == Puzzles.NoImplementation))
                .Key;
            Puzzles.PuzzleImplementationDict().Keys
                .Where(day => day <= lastDay)
                .ToList()
                .ForEach(PrintSolutionMessage);
        }
        else
        {
            Puzzles.PuzzleImplementationDict().Keys
                .ToList()
                .ForEach(PrintSolutionMessage);
        }
    }

    /// <summary>
    ///     Prints the result of the last day with an implementation in the result table
    /// </summary>
    public static void PrintLastSolutionMessage()
    {
        var lastDay = Puzzles.PuzzleImplementationDict()
            .Last(entry => entry.Value.Count > 0 && entry.Value.FirstOrDefault() != Puzzles.NoImplementation)
            .Key;
        PrintSolutionMessage(lastDay);
    }

    /// <summary>
    ///     Prints a header for the results table
    /// </summary>
    public static void PrintResultHeader()
    {
        var columnsHeadersToPrint = ColumnHeaders.Select(header => header.Message.PadLeft(header.Padding));
        Console.WriteLine($"| {string.Join(" | ", columnsHeadersToPrint)} |");
    }


    /// <summary>
    ///     Prints a separator line spanning the console width
    /// </summary>
    /// <param name="onlyDuringDebug">Optional param defaulting to false, will make this a debug only print if true</param>
    public static void PrintSeparator(bool onlyDuringDebug = false)
    {
        if (!onlyDuringDebug || Config.IsDebug) Console.WriteLine(new string(Config.LineArtChar, ConsoleWidth));
    }

    /// <summary>
    ///     Prints a message only if the static variable <see cref="Config.IsDebug"/> is set
    /// </summary>
    /// <param name="message">Message to print</param>
    public static void DebugMsg(string message)
    {
        if (Config.IsDebug) Console.WriteLine(message);
    }

    /// <summary>
    ///     Print a collection only up to a certain amount
    /// </summary>
    /// <typeparam name="T">Generic type param for elements in the collection</typeparam>
    /// <param name="collection">Base collection containing the data</param>
    /// <param name="prefix">string to print before the collection items</param>
    /// <param name="maxCount">Maximum number of items to print</param>
    /// <param name="separator">string seperator between items</param>
    public static void DebugPrintExcerpt<T>(IEnumerable<T> collection, string? prefix = null, int maxCount = 10, string separator = ", ")
    {
        if (!Config.IsDebug) return;

        if (prefix != null) Console.Write(prefix);
        var actuallyTaken = collection.Take(maxCount).ToList();
        var andMore = actuallyTaken.Count == maxCount ? $"... and {collection.Count() - actuallyTaken.Count} more" : string.Empty;
        Console.WriteLine($"{string.Join(separator, actuallyTaken)} {andMore}");
    }
}