using System;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class StopArgument: ProjectSubCommandArgument
    {
        public Guid ProjectId { get; set; }
        public string WorkingPath { get; set; }
    }
}