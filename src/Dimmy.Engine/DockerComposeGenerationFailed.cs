using System;

namespace Dimmy.Engine
{
    public class DockerComposeGenerationFailed : Exception
    {
        public DockerComposeGenerationFailed(string error) : base(error)
        {
        }
    }
}