﻿using System;
using System.Collections.Generic;
using Dimmy.Engine.Models;

namespace Dimmy.Engine.Services
{
    public interface IProjectService
    {
        IEnumerable<Project> RunningProjects();
        Project GetProjectById(Guid projectId);
        (ProjectInstanceYaml ProjectInstance, ProjectYaml Project) GetProject(string projectInstancePath = "");
    }
}