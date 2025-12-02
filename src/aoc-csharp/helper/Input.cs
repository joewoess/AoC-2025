namespace aoc_csharp.helper;

/// <summary>
///     Provides extension methods for retrieving input data associated with a puzzle implementation.
/// </summary>
/// <remarks>These methods are intended to simplify access to input files for puzzle types implementing the
/// IPuzzle interface. The input file is determined based on the puzzle's type name and the current configuration (demo
/// or real mode).</remarks>
public static class Input
{
    private static string DataDirectory => Config.IsDemo ? Config.InputPathDemo : Config.InputPathReal;
    private static string DataFileName(IPuzzle puzzle) => $"{puzzle.TypeName.ToLower()}.txt";
    private static string DataFilePath(IPuzzle puzzle) => Path.Combine(DataDirectory, DataFileName(puzzle));

    /// <summary>
    ///     Attempts to retrieve the input data associated with the specified puzzle implementation.
    /// </summary>
    /// <remarks>The method checks for the existence of an input data file associated with the provided puzzle
    /// implementation. If the file exists, its contents are returned; otherwise, the result indicates failure. Callers
    /// should check the <c>Success</c> property of the returned result to determine if input data was found.</remarks>
    /// <param name="impl">The puzzle implementation for which to retrieve the input data. Cannot be null.</param>
    /// <returns>A <see cref="SuccessResult{string}"/> indicating whether the input data was found. If successful, the result
    /// contains the input data as a string; otherwise, the result indicates failure and contains null.</returns>
    public static SuccessResult<string> GetInput(this IPuzzle impl)
    {
        return File.Exists(DataFilePath(impl))
            ? new SuccessResult<string>(true, File.ReadAllText(DataFilePath(impl)))
            : new SuccessResult<string>(false, null);
    }

    /// <summary>
    ///     Retrieves the input lines for the specified puzzle implementation from its associated data file.
    /// </summary>
    /// <remarks>The method checks for the existence of the data file associated with the given puzzle
    /// implementation. If the file does not exist, the returned result will indicate failure and contain a null
    /// value.</remarks>
    /// <param name="impl">The puzzle implementation for which to retrieve input lines. Cannot be null.</param>
    /// <returns>A <see cref="SuccessResult{T}"/> containing an array of input lines if the data file exists; otherwise, a result
    /// indicating failure with a null value.</returns>
    public static SuccessResult<string[]> GetInputLines(this IPuzzle impl)
    {
        return File.Exists(DataFilePath(impl))
            ? new SuccessResult<string[]>(true, File.ReadAllLines(DataFilePath(impl)))
            : new SuccessResult<string[]>(false, null);
    }
}