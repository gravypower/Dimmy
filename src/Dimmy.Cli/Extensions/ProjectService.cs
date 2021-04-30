using System;
using System.Linq;
using Dimmy.Engine.Models;
using Dimmy.Engine.Models.Yaml;
using Dimmy.Engine.Services.Projects;

namespace Dimmy.Cli.Extensions
{
    public static class ProjectService
    {
        public static (ProjectInstanceYaml ProjectInstance, ProjectYaml Project) ResolveProject(
            this IProjectService projectService, IGetProjectArg arg)
        {
            if (arg.ProjectId == string.Empty && string.IsNullOrEmpty(arg.WorkingPath))
            {
                return projectService.GetProject();
            }

            if (!string.IsNullOrEmpty(arg.WorkingPath))
            {
                return projectService.GetProject(arg.WorkingPath);
            }

            if(arg.ProjectId != string.Empty)
            {
                var project = projectService.RunningProjects().Single(p => p.Id == Guid.Parse(arg.ProjectId)); 
                return projectService.GetProject(project.WorkingPath);
            }

            throw new CouldNotResolveProject();
        }
        
        public static Project ResolveRunningProject(this IProjectService projectService, IGetProjectArg arg)
        {
            var project = projectService.ResolveProject(arg);
            var runningProject = projectService.RunningProjects().Single(p => p.Id == project.Project.Id);

            return runningProject;
        }
    }
    

    public class CouldNotResolveProject : Exception
    {
    }

    public interface IGetProjectArg
    {
        public string ProjectId { get; set; }
        
        public string WorkingPath { get; set; }
    }
}