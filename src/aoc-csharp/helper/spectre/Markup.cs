using Spectre.Console;

namespace aoc_csharp.helper.spectre;

public static class MarkupExtensions
{
    public static Markup AsMarkup(this string str, string markupStyle = "")
    {
        return new Markup($"[{markupStyle}]{str}[/]");
    }
}