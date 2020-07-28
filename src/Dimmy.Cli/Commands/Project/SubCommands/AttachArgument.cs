using System;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class AttachArgument
    {
        public Guid ProjectId { get; set; }
        public string Role { get; set; }
        public bool NoExit { get; set; }
    }
}