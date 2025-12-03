namespace aoc_csharp.helper;

/// <summary>
///     Handles converting and printing Time in an concisive manner
/// </summary>
public static class TimeUtil
{
    public static string AsMsString(this TimeSpan timeSpan) => $"{timeSpan.TotalMilliseconds:F2}ms";
    public static string AsSecString(this TimeSpan timeSpan) => $"{timeSpan.TotalSeconds:F2}s";
    public static string AsMinString(this TimeSpan timeSpan) => $"{(int)timeSpan.TotalMinutes:##}m{timeSpan.Seconds:d2}";

    /// <summary>
    ///     Prefered way to print TimeSpans like those from a Stopwatch
    ///     Wrapper for nullable types. See <see cref="TimeUtil.Readable(TimeSpan)"/> for more details
    /// </summary>
    /// <param name="timeSpan">Nullable TimeSpan to convert</param>
    /// <returns>Human readable TimeSpan with the closest unit of measurement</returns>
    public static string Readable(this TimeSpan? timeSpan) => timeSpan switch
    {
        null => "0 ms",
        _ => Readable(timeSpan.Value)
    };

    /// <summary>
    ///     Prefered way to print TimeSpans like those from a Stopwatch
    /// </summary>
    /// <param name="timeSpan">TimeSpan to convert</param>
    /// <returns>Human readable TimeSpan with the closest unit of measurement</returns>
    public static string Readable(this TimeSpan timeSpan) => timeSpan switch
    {
        { TotalMilliseconds: < 1000 } => AsMsString(timeSpan),
        { TotalSeconds: < 60 } => AsSecString(timeSpan),
        { TotalMinutes: < 60 } => AsMinString(timeSpan),
        _ => timeSpan.ToString()
    };
}