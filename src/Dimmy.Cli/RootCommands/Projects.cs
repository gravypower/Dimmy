using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using Dimmy.Cli.Extensions;
using Dimmy.Engine.Queries;
using Dimmy.Engine.Queries.Projects;

namespace Dimmy.Cli.RootCommands
{
    internal class Projects:ICommandLineCommand
    {
        private readonly IQueryHandler<GetRunningProjects, IEnumerable<Engine.Models.Project>> _getRunningProjectsQueryHandler;
        
        public Projects(IQueryHandler<GetRunningProjects, IEnumerable<Engine.Models.Project>> getRunningProjectsQueryHandler)
        {
            _getRunningProjectsQueryHandler = getRunningProjectsQueryHandler;
        }

        private Command AddListSubCommand()
        {
            var projectListCommand = new Command("ls", "Lists running projects")
            {
                Handler = CommandHandler.Create(async () =>
                {
                    var getRunningProjects = new GetRunningProjects();
                    var runningProjects = _getRunningProjectsQueryHandler.Handle(getRunningProjects);
                    foreach (var runningProject in await runningProjects)
                    {
                        runningProject.PrettyPrint();
                    }
                })
            };

            return projectListCommand;
        }

        public Command GetCommand()
        {
            var projectsCommand = new Command("projects");

            projectsCommand.AddCommand(AddListSubCommand());
            projectsCommand.AddAlias("projects");

            return projectsCommand;
        }
    }
}
