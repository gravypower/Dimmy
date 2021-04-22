using System.Collections.Generic;
using Dimmy.Engine.Models.Yaml;

namespace Dimmy.Engine.Pipelines.InitialiseProject
{
    public interface IInitialiseProjectContext
    {
        string Name { get; set; }
        string SourceCodePath { get; set; }
        string WorkingPath { get; set; }
        public string DockerComposeFilePath { get; set; }
        public string EnvironmentTemplateFilePath { get; set; }
        IDictionary<string, string> PublicVariables { get; set; }
        IDictionary<string, string> PrivateVariables { get; set; }
        IDictionary<string, string> MetaData { get; set; }
        
        ProjectYaml ProjectYaml { get; set; }
    }
}