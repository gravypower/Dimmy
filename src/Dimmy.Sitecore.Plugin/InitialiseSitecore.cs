using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using Dimmy.Cli.Commands.Project;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Project;
using Dimmy.Engine.Models;

namespace Dimmy.Sitecore.Plugin
{
    public class InitialiseSitecore: InitialiseSubCommand
    {
        private readonly IEnumerable<ITopology> _topologies;

        public InitialiseSitecore(
            IEnumerable<ITopology> topologies,
            ICommandHandler<InitialiseProject> initialiseProjectCommandHandler) : base(initialiseProjectCommandHandler)
        {
            _topologies = topologies;
        }

        protected override string Name => "sitecore";
        protected override string Description => "Initialise a Sitecore project.";
        
        protected override void HydrateCommand(Command command)
        {
            command.AddOption(new Option("--license-path"));

            command.AddOption(new Option("--topology-name"));

            command.Handler = CommandHandler.Create<string, string, string, string, string, string>(DoInitialise);
        }

        public void DoInitialise(string name, string sourceCodeFolder, string projectPath, string dockerComposeTemplate,  string licensePath, string topologyName)
        {

            var composeTemplate = new DockerComposeTemplate();

            if (!string.IsNullOrEmpty(dockerComposeTemplate) && string.IsNullOrEmpty(topologyName))
            {
                composeTemplate.Contents = File.ReadAllText(dockerComposeTemplate);
                composeTemplate.FileName = Path.GetFileName(dockerComposeTemplate);
            }
            else
            {
                throw new MultipleDockerComposeTemplatesPassed();
            }

            if (!string.IsNullOrEmpty(topologyName) && string.IsNullOrEmpty(dockerComposeTemplate))
            {
                var topology = _topologies.Single(t => t.Name == topologyName);
                composeTemplate.Contents = dockerComposeTemplate;
                composeTemplate.FileName = topology.DockerComposeTemplateName;
            }
            else
            {
                throw new MultipleDockerComposeTemplatesPassed();
            }

            var initialiseProject = new InitialiseProject()
            {
                ProjectLocation = projectPath,
                SourceCodeLocation = sourceCodeFolder,
                DockerComposeTemplate = composeTemplate
            };

            InitialiseProjectCommandHandler.Handle(initialiseProject);

        }
    }

    public class MultipleDockerComposeTemplatesPassed : Exception
    {
    }
}
