using Ductus.FluentDocker.Model.Compose;

namespace Dimmy.Engine.Services
{
    public interface IDockerComposeParser
    {
        DockerComposeFileConfig ParseDockerComposeString(string dockerComposeString);
    }
}