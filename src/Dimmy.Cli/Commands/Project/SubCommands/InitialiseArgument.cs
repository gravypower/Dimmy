namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class InitialiseArgument : ProjectCommandArgument
    {
        public string Name { get; set; }
        
        public string SourceCodePath { get; set; }
        
        public string WorkingPath { get; set; }
        
        public string DockerComposeTemplate { get; set; }
        
        public string EnvironmentTemplate { get; set; }
    }
}