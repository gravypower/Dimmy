using System;

namespace DIMS.Engine.Models
{
    public class ProjectYaml
    {
        public string Name { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
