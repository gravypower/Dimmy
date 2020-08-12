using System;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class PauseArgument: ProjectSubCommandArgument
    {
        public Guid ProjectId { get; set; }
        public string WorkingPath { get; set; }
    }
}