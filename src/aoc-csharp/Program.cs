global using aoc_csharp.helper;

// Parse cmd line args
Config.IsDemo = args.Contains("--demo");
Config.IsDebug = args.Contains("--debug");
Config.ShowLast = args.Contains("--last");
Config.ShowFirst = !args.Contains("--second") || args.Contains("--first");
Config.ShowSecond = !args.Contains("--first") || args.Contains("--second");
Config.SkipLongRunning = args.Contains("--quick");

// Determine explicitly requested days
var explicitDaysRequested = args
    .Where(arg => arg.All(char.IsDigit))
    .Select(int.Parse)
    .Where(day => day.Between(1, Config.MaxChallengeDays, true))
    .ToList();

// Prepare estimated longest message for console width calculation
var longestMessage =
    $"IsDemo: {Config.IsDemo} | IsDebug: {Config.IsDebug} | ShowLast: {Config.ShowLast} | ShowFirst: {Config.ShowFirst} | ShowSecond: {Config.ShowSecond} |"
    + $" ExplicitDays: [{string.Join(",", explicitDaysRequested)}] |"
    + $" SkipLongRunning: {Config.SkipLongRunning}";

// Adjust console width if needed
Printer.ConsoleWidth = longestMessage.Length;
if (Config.TryAndUseConsoleWidth) Printer.TryUpdateConsoleWidth(); // this only works when running in a real console

// Greeting message
Printer.PrintGreeting();
Printer.DebugMsg(longestMessage);

// Table of results for all implemented puzzles
Printer.DebugMsg($"Found {Puzzles.ImplementedPuzzles().Count} implementations");
Printer.PrintSeparator(onlyDuringDebug: true);
Printer.PrintResultHeader();

// Run the actual program
if (explicitDaysRequested.Count > 0) explicitDaysRequested.ForEach(Printer.PrintSolutionMessage);
if (Config.ShowLast) Printer.PrintLastSolutionMessage();
if (explicitDaysRequested.Count == 0 && !Config.ShowLast) Printer.PrintAllSolutionMessages();