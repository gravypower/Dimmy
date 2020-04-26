using System;
using System.Collections.Generic;
using Dimmy.Engine.Models;
using Dimmy.Engine.Models.Yaml;

namespace Dimmy.Engine.Services
{
    public interface IProjectService
    {
        IEnumerable<Project> RunningProjects();
        Project GetProjectById(Guid projectId);
        (ProjectInstanceYaml ProjectInstance, ProjectYaml Project) GetProject(string projectInstancePath = "");
    }
}