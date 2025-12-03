
using Spectre.Console;
using Spectre.Console.Cli;

namespace aoc_csharp.helper.spectre;

public class GlobalSettings : CommandSettings
{

    [CommandOption("--debug|-d")]
    public bool Debug { get; init; }

    [CommandOption("--quick|-q")]
    public bool Quick { get; init; }
}

public class BaseSettings : GlobalSettings
{
    [CommandArgument(0, "[days]")]
    public string[]? Days { get; set; }

    public override ValidationResult Validate()
    {
        if (Days != null)
        {
            foreach (var day in Days)
            {
                var isArgAnInt = int.TryParse(day, out int dayParsed);
                if (!isArgAnInt)
                {
                    return ValidationResult.Error($"Day '{day}' is not a valid day. Valid days are between 1 and {Config.MaxChallengeDays}.");
                }
                if (dayParsed < 1 || dayParsed > Config.MaxChallengeDays)
                {
                    return ValidationResult.Error($"Day '{day}' is out of range. Valid days are between 1 and {Config.MaxChallengeDays}.");
                }
            }
        }
        return ValidationResult.Success();
    }
}