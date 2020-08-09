using System;

namespace Dimmy.Engine.Commands.Docker
{
    public class DockerComposeGenerationFailed : Exception
    {
        public DockerComposeGenerationFailed(string error) : base(error)
        {
        }
    }
}