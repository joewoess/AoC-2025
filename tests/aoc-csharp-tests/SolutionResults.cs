namespace aoc_csharp_tests;

using aoc_csharp.helper;

public class Solutions : Dictionary<int, (string? Part1, string? Part2)>;

public static class SolutionResults
{
    // Parses a markdown table in solutions.md and returns a map Day -> (First, Second).
    // Chooses the demo or real column based on Config.IsDemo.

    private static bool ContainsIgnoreCase(this string content, string part) =>
        content.Contains(part, StringComparison.InvariantCultureIgnoreCase);

    private static string[] SplitPipe(string line) =>
        line.Split('|').Select(s => s.Trim()).ToArray();

    /// <summary>
    ///     Loads the expected solutions from the solutions markdown file. This considers the Config.IsDemo setting to
    ///     determine whether to load demo or real solutions.
    /// </summary>
    /// <returns>Dictionary that maps a day number to a pair of results</returns>
    public static Solutions LoadExpected()
    {
        try
        {
            var lines = File.Exists(Config.InputPathSolutionsMd)
                ? File.ReadAllLines(Config.InputPathSolutionsMd)
                : [];
            if (lines.Length == 0) return [];

            // Find the header line
            int headerIdx = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                var l = lines[i];
                if (!l.Contains('|') || !l.ContainsIgnoreCase("Day")) continue;

                headerIdx = i;
                break;
            }

            // could not find header
            if (headerIdx == -1) return [];

            // Determine which columns to look for
            var headerCols = SplitPipe(lines[headerIdx]);
            var configToLookFor = Config.IsDemo ? "Demo" : "Real";

            int dayIdx = Array.FindIndex(headerCols, c => c?.ContainsIgnoreCase("Day") == true);
            int part1 = Array.FindIndex(headerCols, c => c?.ContainsIgnoreCase(configToLookFor) == true && c?.ContainsIgnoreCase("part1") == true);
            int part2 = Array.FindIndex(headerCols, c => c?.ContainsIgnoreCase(configToLookFor) == true && c?.ContainsIgnoreCase("part2") == true);

            if (dayIdx < 0 || part1 < 0 || part2 < 0) return [];

            // skip separator line (|---|---|)
            int dataStart = headerIdx + 2;

            // Start parsing data
            var result = new Solutions();
            for (int i = dataStart; i < lines.Length; i++)
            {
                if (!lines[i].Contains('|')) break; // end of table

                var cols = SplitPipe(lines[i]);

                // ignore short rows or separator rows
                var day = int.Parse(cols[dayIdx].Trim());
                var first = cols[part1].Trim();
                var second = cols[part2].Trim();

                // remove markdown inline code ticks
                if (first.StartsWith("`") && first.EndsWith("`")) first = first.Trim('`').Trim();
                if (second.StartsWith("`") && second.EndsWith("`")) second = second.Trim('`').Trim();

                if (first == "part1") first = null;
                if (second == "part2") second = null;

                if (first is null && second is null) continue;

                result[day] = (first, second);
            }
            return result;
        }
        catch
        {
            return [];
        }
    }
}