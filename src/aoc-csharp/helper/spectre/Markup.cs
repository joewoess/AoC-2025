using Spectre.Console;

namespace aoc_csharp.helper.spectre;

public static class MarkupExtensions
{
    public static Markup AsMarkup(this string str, string markupStyle)
    {
        return new Markup($"[{markupStyle}]{str}[/]");
    }
    public static Markup AsMarkup(this string str, Style markupStyle)
    {
        return new Markup(str, markupStyle);
    }
    public static Markup AsMarkup(this string str)
    {
        return new Markup(str);
    }
}