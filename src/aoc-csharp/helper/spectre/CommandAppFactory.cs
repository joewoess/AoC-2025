
using System.Data;
using Spectre.Console;
using Spectre.Console.Cli;

namespace aoc_csharp.helper.spectre;

/// <summary>
///  Factory class for creating and configuring instances of <see cref="CommandApp"/> tailored for the Advent of Code application.
/// </summary>
public static class CommandAppFactory
{
    /// <summary>
    ///   Creates and configures a new instance of <see cref="CommandApp"/> for the Advent of Code application.
    /// </summary>
    /// <returns></returns>
    public static CommandApp Create()
    {
        var app = new CommandApp();
        app.Configure(config =>
        {
            // Set application metadata
            config.SetBaseSettings();

            // Define command branches and their respective commands
            config.AddBranch("demo", demo =>
            {
                demo.AddCommand<RunDemoCommand>("days").WithAlias("s");
                demo.SetDefaultCommand<RunDemoCommand>();
                demo.AddBranch("last",
                    last => SetupChildCommand<RunDemoLastCommand>(last, "days", "s")
                ).WithAlias("l");
            }).WithAlias("d");
            config.AddBranch("real", real =>
            {
                real.AddCommand<RunRealCommand>("days").WithAlias("s");
                real.SetDefaultCommand<RunRealCommand>();
                real.AddBranch("last",
                    last => SetupChildCommand<RunRealLastCommand>(last, "days", "s")
                ).WithAlias("l");
            }).WithAlias("r");
            config.AddCommand<RunDemoCommand>("days");
        });
        app.SetDefaultCommand<RunRealCommand>();
        return app;
    }

    private static IConfigurator SetBaseSettings(this IConfigurator config)
    {
        config.SetApplicationName(Config.ApplicationReadableName);
        config.SetApplicationVersion(Config.ApplicationVersion);
        config.CaseSensitivity(CaseSensitivity.None);
        config.SetExceptionHandler((ex, resolver) =>
        {
            AnsiConsole.WriteException(ex, ExceptionFormats.NoStackTrace);
        });
        return config;
    }

    private static IConfigurator<BaseSettings> SetupChildCommand<T>(IConfigurator<BaseSettings> config, string commandName, string alias)
        where T : Command<BaseSettings>
    {
        config.AddCommand<T>(commandName).WithAlias(alias);
        config.SetDefaultCommand<T>();
        return config;
    }
}