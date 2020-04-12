using System;
using System.Collections.Generic;
using DIMS.Engine.Models;

namespace DIMS.Engine.Services
{
    public interface IProjectService
    {
        IEnumerable<Project> RunningProjects();
        Project GetProjectById(Guid projectId);
        Guid GetContextProjectId();
    }
}