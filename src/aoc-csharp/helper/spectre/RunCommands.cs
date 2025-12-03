using Spectre.Console.Cli;

namespace aoc_csharp.helper.spectre;

public class BasicRunCommand : Command<BaseSettings>
{
    public override int Execute(CommandContext context, BaseSettings settings, CancellationToken cancellationToken)
    {
        Config.IsDebug = settings.Debug;
        Config.SkipLongRunning = settings.Quick;

        return SpectreFlowRunner.Run(settings);
    }
}

public class RunRealCommand : BasicRunCommand
{
    public override int Execute(CommandContext context, BaseSettings settings, CancellationToken cancellationToken)
    {
        Config.IsDemo = false;
        return base.Execute(context, settings, cancellationToken);
    }
}

public class RunDemoCommand : BasicRunCommand
{
    public override int Execute(CommandContext context, BaseSettings settings, CancellationToken cancellationToken)
    {
        Config.IsDemo = true;
        return base.Execute(context, settings, cancellationToken);
    }
}

public class RunRealLastCommand : RunRealCommand
{
    public override int Execute(CommandContext context, BaseSettings settings, CancellationToken cancellationToken)
    {
        Config.ShowLast = true;
        return base.Execute(context, settings, cancellationToken);
    }
}

public class RunDemoLastCommand : RunDemoCommand
{
    public override int Execute(CommandContext context, BaseSettings settings, CancellationToken cancellationToken)
    {
        Config.ShowLast = true;
        return base.Execute(context, settings, cancellationToken);
    }
}