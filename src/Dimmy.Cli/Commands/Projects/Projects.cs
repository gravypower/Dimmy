using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using Dimmy.Cli.Extensions;
using Dimmy.Engine.Queries;
using Dimmy.Engine.Queries.Projects;

namespace Dimmy.Cli.Commands.Projects
{
    public class Projects : Command<ProjectsArgument>
    {
        private readonly IQueryHandler<GetRunningProjects, IList<Engine.Models.Project>>
            _getRunningProjectsQueryHandler;

        public Projects(IQueryHandler<GetRunningProjects, IList<Engine.Models.Project>> getRunningProjectsQueryHandler)
        {
            _getRunningProjectsQueryHandler = getRunningProjectsQueryHandler;
        }

        public override Command BuildCommand()
        {
            var projectsCommand = new Command("projects");

            projectsCommand.AddCommand(AddListSubCommand());
            projectsCommand.AddAlias("projects");

            return projectsCommand;
        }

        private Command AddListSubCommand()
        {
            var projectListCommand = new Command("ls", "Lists running projects")
            {
                Handler = CommandHandler.Create(ListRunningProjcts)
            };

            return projectListCommand;
        }

        private void ListRunningProjcts()
        {
            var getRunningProjects = new GetRunningProjects();
            var runningProjects = _getRunningProjectsQueryHandler.Handle(getRunningProjects);
            foreach (var runningProject in runningProjects) runningProject.PrettyPrint();
        }
    }
}