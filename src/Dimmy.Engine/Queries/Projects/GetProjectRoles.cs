using System;
using System.Collections.Generic;
using Dimmy.Engine.Models;

namespace Dimmy.Engine.Queries.Projects
{
    public class GetProjectRoles:IQuery<IEnumerable<Service>>
    {
        public string ProjectName { get; set; }
        public Guid ProjectId { get; set; }
    }
}
