namespace Dimmy.Cli.Commands.Project
{
    public abstract class ProjectSubCommand<TProjectSubCommandArgument> : Command<TProjectSubCommandArgument>, IProjectSubCommand 
        where TProjectSubCommandArgument : CommandArgument
    {
    }
}