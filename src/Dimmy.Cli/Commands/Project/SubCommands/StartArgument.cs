namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class StartArgument:ProjectCommandArgument
    {
        public string WorkingPath { get; set; }
        public bool GenerateOnly { get; set; }
    }
}