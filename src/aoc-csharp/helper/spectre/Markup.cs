using Spectre.Console;
using Spectre.Console.Rendering;

namespace aoc_csharp.helper.spectre;

public static class MarkupExtensions
{
    public readonly static Style DebugStyle = Style.Parse("grey");
    public readonly static Style HeaderStyle = Style.Parse("bold blue");
    public readonly static Style InfoStyle = Style.Parse("lightgreen");

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

    public static Panel PanelWithStyle(Style style, Renderable content) => new(content)
    {
        BorderStyle = style,
        Expand = true,
        Border = BoxBorder.Rounded
    };
}