using System.Text;
using aoc_csharp.helper.spectre;
using Spectre.Console;

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
    // Some stylings used throughout the printer
    private readonly static Style DebugStyle = Style.Parse("grey");
    private static readonly Panel HeaderPanel = new(
        Align.Center(
            new Rows(
                $"{Config.ApplicationReadableName} for {Config.CurrentChallengeYear}".AsMarkup("bold blue"),
                new Grid()
                    .AddColumn()
                    .AddColumn()
                    .AddRow("Challenge at:".AsMarkup("lightgreen"), new Markup($"AdventOfCode [link]https://adventofcode.com/{Config.CurrentChallengeYear}/[/]"))
                    .AddRow("Author:".AsMarkup("lightgreen"), new Markup($"{Config.AuthorName} ([link]{Config.AuthorGithubRepo}[/])"))
    )))
    {
        Border = BoxBorder.Rounded,
        Expand = true,
        BorderStyle = new Style(Color.Blue)
    };

    // Live display context and related fields for updating logs during live table updates
    private static LiveDisplayContext? liveContext = null;
    private static StringBuilder? logsDuringLive = null;
    private static List<Panel>? logRendererPerDay = null;
    private static Table? liveTable = null;
    private static int? currentDay = null;

    /// <summary>
    ///     Prints the beginning header of console output
    /// </summary>
    public static void PrintGreeting()
    {
        AnsiConsole.Write(HeaderPanel);
    }

    /// <summary>
    ///     Create a header for the results table
    /// </summary>
    public static Table CreateTableWithHeader()
    {
        return new Table()
            .Expand()
            .Border(TableBorder.Rounded)
            .Alignment(Justify.Center)
            .AddColumn(TableCol("Day", "bold blue", Config.InfoColumnRatio))
            .AddColumn(TableCol("Type", "bold grey", Config.InfoColumnRatio))
            .AddColumn(TableCol("First puzzle", "bold blue", Config.ResultColumnRatio))
            .AddColumn(TableCol("Time 1st", "bold grey", Config.TimingColumnRatio))
            .AddColumn(TableCol("Second puzzle", "bold blue", Config.ResultColumnRatio))
            .AddColumn(TableCol("Time 2nd", "bold grey", Config.TimingColumnRatio))
            .ShowRowSeparators()
            ;
    }

    /// <summary>
    ///     Prints a message only if the static variable <see cref="Config.IsDebug"/> is set
    /// </summary>
    /// <param name="message">Message to print</param>
    public static void DebugMsg(string message, bool addNewLine = true)
    {
        if (!Config.IsDebug) return;
        PrintOrPostInPanel(message, addNewLine);
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

        if (prefix != null) DebugMsg(prefix, false);
        var actuallyTaken = collection.Take(maxCount).ToList();
        var andMore = actuallyTaken.Count == maxCount ? $"... and {collection.Count() - actuallyTaken.Count} more" : string.Empty;
        DebugMsg($"{string.Join(separator, actuallyTaken)} {andMore}");
    }

    /// <summary>
    ///     Starts a live updating results table for puzzle implementations
    /// </summary>
    /// <param name="explicitDaysRequested">Explicitley requested days if any</param>
    public static void StartLiveResultsTable(List<int> explicitDaysRequested)
    {
        // Determine days to process
        var implementationDict = Puzzles.PuzzleImplementationDict();
        var daysToProcess = implementationDict.CalculateDaysToProcess(explicitDaysRequested);

        // Start stopwatch
        var sw = System.Diagnostics.Stopwatch.StartNew();
        AnsiConsole.Write(Align.Right("Starting execution...".AsMarkup("lightgreen")));

        // Prepare live table and log containers
        liveTable = CreateTableWithHeader();
        logsDuringLive = new StringBuilder();
        logRendererPerDay = [];
        var layout = new Rows([.. logRendererPerDay, liveTable]);

        // Start live context
        AnsiConsole.Live(layout)
            .Start(ctx =>
            {
                liveContext = ctx;
                foreach (var day in daysToProcess)
                {
                    currentDay = day;
                    logRendererPerDay.Add(CreateLiveLogPanel());
                    logsDuringLive.Clear();
                    var implementationsOfDay = implementationDict[day];
                    if (implementationsOfDay.Count == 0)
                    {
                        AddPlaceholderTableRow(day);
                        continue;
                    }
                    foreach (var impl in implementationsOfDay)
                    {
                        var startTime = sw.Elapsed;
                        string firstPuzzle = impl.ResolveFirstPuzzle();
                        var midTime = sw.Elapsed;
                        string secondPuzzle = impl.ResolveSecondPuzzle();
                        var endTime = sw.Elapsed;

                        liveTable.AddRow(
                            string.Format(Config.DayMessageConvention, day).AsMarkup(),
                            impl.TypeName.AsMarkup("grey"),
                            firstPuzzle.AsMarkup(),
                            (midTime - startTime).Readable().AsMarkup("grey"),
                            secondPuzzle.AsMarkup(),
                            (endTime - midTime).Readable().AsMarkup("grey")
                        );
                        ctx.Refresh();
                    }
                }
                logsDuringLive = null;
                logRendererPerDay = null;
                liveTable = null;
                currentDay = null;
                liveContext = null;
            });

        // Finished execution
        sw.Stop();
        AnsiConsole.Write(Align.Right($"Finished in {sw.Elapsed.Readable()}".AsMarkup("lightgreen")));
    }

    private static void AddPlaceholderTableRow(int day)
    {
        liveTable?.AddRow(
            string.Format(Config.DayMessageConvention, day).AsMarkup(),
            Config.NoSolutionMessage.AsMarkup("grey"),
            Config.NoSolutionMessage.AsMarkup(),
            TimeUtil.Readable(null).AsMarkup("grey"),
            Config.NoSolutionMessage.AsMarkup(),
            TimeUtil.Readable(null).AsMarkup("grey")
        );
    }

    private static List<int> CalculateDaysToProcess(this Dictionary<int, List<IPuzzle>> implementationDict, List<int> explicitDaysRequested)
    {
        var lastDay = implementationDict
            .Last(entry => entry.Value.Count > 0 && entry.Value.First() != Puzzles.NoImplementation)
            .Key;

        var daysToProcess = (explicitDaysRequested, Config.ShowLast, Config.PrintAfterLastImpl) switch
        {
            ({ Count: > 0 }, _, _) => explicitDaysRequested,
            (_, true, _) => [lastDay],
            (_, _, false) => [.. implementationDict.Keys.Where(day => day <= lastDay)],
            _ => [.. implementationDict.Keys]
        };
        return daysToProcess;
    }



    private static TableColumn TableCol(string header, string markup, int width) =>
        new TableColumn(header.AsMarkup(markup)).RightAligned().Width(width);

    private static void PrintOrPostInPanel(string message, bool addNewLine = true)
    {
        if (liveContext == null || logsDuringLive == null || logRendererPerDay == null || liveTable == null)
        {
            // Not in live context, just print directly
            AnsiConsole.Write(new Text(addNewLine ? message + "\n" : message, DebugStyle));
        }
        else
        {
            // In live context, append to live log panel
            logsDuringLive.AppendLine(message);
            logRendererPerDay[^1] = CreateLiveLogPanel();
            liveContext.UpdateTarget(new Rows(
                [
                    ..logRendererPerDay,
                    liveTable
                ]
            ));
            liveContext.Refresh();
        }
    }

    private static Panel CreateLiveLogPanel()
    {
        if (liveContext == null || logsDuringLive == null || logRendererPerDay == null || liveTable == null || currentDay == null)
            throw new InvalidLiveDisplayOperationException();
        return new Panel(new Text(logsDuringLive.ToString(), DebugStyle))
        {
            Expand = true,
            Header = new PanelHeader($"Debug for day{currentDay}", Justify.Center),
            Border = BoxBorder.Rounded,
            BorderStyle = DebugStyle
        };
    }
}