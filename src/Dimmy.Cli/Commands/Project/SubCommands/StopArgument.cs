using System;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class StopArgument
    {
        public Guid ProjectId { get; set; }
        public string WorkingPath { get; set; }
    }
}