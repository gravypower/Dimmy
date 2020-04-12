using System;
using System.Collections.Generic;

namespace DIMS.Engine.Models
{
    public class Project
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public IList<Role> Roles { get; set; } = new List<Role>();
    }
}
