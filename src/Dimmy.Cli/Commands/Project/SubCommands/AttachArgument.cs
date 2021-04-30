using Dimmy.Cli.Extensions;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class AttachArgument:ProjectCommandArgument, IGetProjectArg
    {
        public string ProjectId { get; set; }
        
        public string WorkingPath { get; set; }
        
        public string Role { get; set; }
        public bool NoExit { get; set; }
    }
}