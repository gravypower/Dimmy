using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dimmy.Engine.Models.Yaml.DockerCompose;
using Dimmy.Engine.Pipelines;
using Dimmy.Engine.Pipelines.StartProject;
using Dimmy.Engine.Services;
using Dimmy.Engine.Services.Projects;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Model.Compose;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Dimmy.Engine.Commands.Docker
{
    public class StartProjectCommandHandler : ICommandHandler<StartProject>
    {
        private readonly Pipeline<Node<IStartProjectContext>, IStartProjectContext> _startProjectPipeline;

        public StartProjectCommandHandler(
            Pipeline<Node<IStartProjectContext>, IStartProjectContext> startProjectPipeline)
        {
            _startProjectPipeline = startProjectPipeline;
        }

        public void Handle(StartProject command)
        {
            _startProjectPipeline.Execute(new StartProjectContext
            {
                Command = command
            });
        }
    }

    
}