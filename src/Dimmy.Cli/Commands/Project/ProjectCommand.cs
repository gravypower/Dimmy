﻿using System.Collections.Generic;
using System.CommandLine;

namespace Dimmy.Cli.Commands.Project
{
    internal class ProjectCommand : ICommandLineCommand
    {
        private readonly IEnumerable<IProjectSubCommand> _projectSubCommands;

        public ProjectCommand(IEnumerable<IProjectSubCommand> projectSubCommands)
        {
            _projectSubCommands = projectSubCommands;
        }

        public Command GetCommand()
        {
            var command = new Command("project");

            foreach (var projectSubCommand in _projectSubCommands) command.AddCommand(projectSubCommand.GetCommand());

            return command;
        }
    }
}