using System;
using System.Collections.Generic;
using DIMS.Engine.Models;

namespace DIMS.Engine.Queries.Projects
{
    public class GetProjectRoles:IQuery<IEnumerable<Role>>
    {
        public string ProjectName { get; set; }
        public Guid ProjectId { get; set; }
    }
}
