using System;
using System.Collections.Generic;

namespace Dimmy.Engine.Models.Docker
{
    public class ImageInspect
    {
        public string Id { get; set; }

        public List<object> RepoTags { get; set; }

        public List<string> RepoDigests { get; set; }

        public string Parent { get; set; }

        public string Comment { get; set; }

        public DateTimeOffset Created { get; set; }

        public string Container { get; set; }

        public Conconfig ContainerConfig { get; set; }

        public string DockerVersion { get; set; }

        public string Author { get; set; }

        public Conconfig Config { get; set; }

        public string Architecture { get; set; }

        public string Os { get; set; }

        public string OsVersion { get; set; }

        public long Size { get; set; }

        public long VirtualSize { get; set; }

        public GraphDriver GraphDriver { get; set; }

        public RootFs RootFs { get; set; }

        public Metadata Metadata { get; set; }
    }

    public class Conconfig
    {
        public string Hostname { get; set; }

        public string Domainname { get; set; }
        
        public string User { get; set; }

        public bool AttachStdin { get; set; }
        
        public bool AttachStdout { get; set; }
        
        public bool AttachStderr { get; set; }
        
        public ExposedPorts ExposedPorts { get; set; }
        
        public bool Tty { get; set; }
        
        public bool OpenStdin { get; set; }
        
        public bool StdinOnce { get; set; }

        public List<string> Env { get; set; }
        
        public List<string> Cmd { get; set; }
        
        public string Image { get; set; }
        
        public object Volumes { get; set; }

        public string WorkingDir { get; set; }
        
        public List<string> Entrypoint { get; set; }

        public object OnBuild { get; set; }
        
        public object Labels { get; set; }

        public List<string> Shell { get; set; }
    }

    public class ExposedPorts
    {
        public The80Tcp The80Tcp { get; set; }
    }

    public class The80Tcp
    {
    }

    public class GraphDriver
    {
        public Data Data { get; set; }

        public string Name { get; set; }
    }

    public class Data
    {
        public string Dir { get; set; }
    }

    public class Metadata
    {
        public DateTimeOffset LastTagTime { get; set; }
    }

    public class RootFs
    {
        public string Type { get; set; }

        public List<string> Layers { get; set; }
    }
}