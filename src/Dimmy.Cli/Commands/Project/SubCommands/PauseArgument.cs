using System;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class PauseArgument: ProjectCommandArgument
    {
        public Guid ProjectId { get; set; }
        public string WorkingPath { get; set; }
    }
}