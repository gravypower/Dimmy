namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class StartArgument:ProjectSubCommandArgument
    {
        public string WorkingPath { get; set; }
        public bool GenerateOnly { get; set; }
    }
}