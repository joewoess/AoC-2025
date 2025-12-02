using System.Text;

namespace aoc_csharp.helper.algorithms;

/// <summary>
/// Implements a basic algorithm for Conways Look-And-Say sequence
/// </summary>
public static class LookAndSay
{
    private const int MaxExcerptLength = 100;

    /// <summary>
    /// One iteration of the Look and Feel sequence 
    /// </summary>
    /// <param name="text">Last step of the sequence</param>
    /// <returns></returns>
    public static string LookAndSayIteration(ReadOnlySpan<char> text)
    {
        if (text.Length == 0)
            return string.Empty;
        var numChar = 1;
        var lastChar = text[0];
        var result = new StringBuilder();
        for (int i = 1; i < text.Length; i++)
        {
            if (text[i] != lastChar)
            {
                result.Append(numChar);
                result.Append(lastChar);
                numChar = 0;
                lastChar = text[i];
            }
            numChar++;
        }
        result.Append(numChar);
        result.Append(lastChar);
        return result.ToString();
    }

    /// <summary>
    /// Iterate N times through the Look and Say sequence
    /// </summary>
    /// <param name="seed">Initial seed text for the sequence</param>
    /// <param name="times">Amount of iterations</param>
    /// <returns></returns>
    public static string LookAndSayNTimes(ReadOnlySpan<char> seed, int times)
    {
        if (seed.Length == 0)
            return string.Empty;
        string permutatedText = seed.ToString();
        times.DoTimes(() => permutatedText = LookAndSayIteration(permutatedText));

        Printer.DebugMsg($"From {seed} after {times} iterations we get:");
        Printer.DebugPrintExcerpt(permutatedText, maxCount: MaxExcerptLength, separator: "");
        return permutatedText.ToString();
    }
}