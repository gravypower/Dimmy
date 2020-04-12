using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using DIMS.Engine.Queries;
using DIMS.Engine.Queries.Projects;

namespace DIMS.CLI.RootCommands
{
    internal class Projects:Command
    {
        private readonly IQueryHandler<GetRunningProjects, IEnumerable<Engine.Models.Project>> _getRunningProjectsQueryHandler;
        
        public Projects(IQueryHandler<GetRunningProjects, IEnumerable<Engine.Models.Project>> getRunningProjectsQueryHandler) 
            : base("projects", "Projects")
        {
            _getRunningProjectsQueryHandler = getRunningProjectsQueryHandler;
         
            AddAlias("projects");
            AddListSubCommand();
        }

        private void AddListSubCommand()
        {
            var projectListCommand = new Command("ls")
            {
                Handler = CommandHandler.Create(() =>
                {
                    var getRunningProjects = new GetRunningProjects();
                    var runningProjects = _getRunningProjectsQueryHandler.Handle(getRunningProjects);
                    foreach (var runningProject in runningProjects)
                    {
                        Console.WriteLine($"\\-{runningProject.Name}");

                        foreach (var role in runningProject.Roles)
                        {
                            Console.WriteLine($"  |-{role.Name}");
                        }
                    }
                })
            };

            Add(projectListCommand);
        }
    }
}
