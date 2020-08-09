using System;
using System.Collections.Generic;
using Dimmy.Engine.Models;
using Dimmy.Engine.Models.Yaml;

namespace Dimmy.Engine.Services.Projects
{
    public interface IProjectService
    {
        IList<Project> RunningProjects();
        Project GetProjectById(Guid projectId);
        (ProjectInstanceYaml ProjectInstance, ProjectYaml Project) GetProject(string projectInstancePath = "");
    }
}