namespace aoc_csharp.helper.spectre;

/// <summary>
///     This is what actually will be run if a command will complete successfully
///     It is basically akin to the Program.cs
/// </summary>
public static class SpectreFlowRunner
{
    /// <summary>
    ///     Starts the live results table and with it the evaluation of the requested puzzles
    /// </summary>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static int Run(BaseSettings settings)
    {
        // Days have already been validated in BaseSettings.Validate()
        var explicitDaysRequested = settings.Days?
            .Select(int.Parse)
            .ToList() ?? [];

        // Print relevant config information in debug
        var configStateMessage =
            $"IsDemo: {Config.IsDemo} | IsDebug: {Config.IsDebug} | ShowLast: {Config.ShowLast} |"
            + $" ExplicitDays: [{string.Join(",", explicitDaysRequested)}] |"
            + $" SkipLongRunning: {Config.SkipLongRunning}";

        // Greeting message
        Printer.PrintGreeting();
        Printer.DebugMsg(configStateMessage);

        // Table of results for all implemented puzzles
        Printer.DebugMsg($"Found {Puzzles.ImplementedPuzzles().Count} implementations");

        Printer.StartLiveResultsTable(explicitDaysRequested);
        return 0;
    }
}