using System;
using System.Collections.Generic;
using System.Linq;
using Ductus.FluentDocker.Model.Compose;

namespace Dimmy.Engine.Services
{
    public class DockerComposeParser:IDockerComposeParser
    {
        public DockerComposeFileConfig ParseDockerComposeString(string dockerComposeString)
        {
            var deserializer = new YamlDotNet.Serialization.Deserializer();
            var dict = deserializer.Deserialize<Dictionary<object, object>>(dockerComposeString);
            var dockerCompose = new DockerComposeFileConfig {Version = (string) dict["version"]};

            foreach (var (key, value) in (Dictionary<object,object>) dict["services"])
            {
                var composeServiceDefinition = new ComposeServiceDefinition {Name = (string) key};

                var s = (Dictionary<object, object>) value;

                ResolveIsolation(s, composeServiceDefinition);
                ResolveImage(s, composeServiceDefinition);
                ResolvePorts(s, composeServiceDefinition);
                ResolveHealthCheck(s, composeServiceDefinition);
                ResolveVolumes(s, composeServiceDefinition);
                ResolveDependsOn(s, composeServiceDefinition);
                ResolveLabels(s, composeServiceDefinition);
                
                dockerCompose.ServiceDefinitions.Add(composeServiceDefinition);
            }

            return dockerCompose;
        }
        
        private static void ResolveLabels(Dictionary<object, object> s, ComposeServiceDefinition composeServiceDefinition)
        {
            if(!s.ContainsKey("labels"))
                return;
            
            switch (s["labels"])
            {
                case Dictionary<object, object> labelsDictionary:
                {
                    foreach (var (key, value) in labelsDictionary)
                    {
                        composeServiceDefinition.Labels.Add((string) key, (string) value);
                    }

                    break;
                }
                case List<object> labelsList:
                {
                    foreach (var label in labelsList)
                    {
                        var l = ((string) label).Split("=");
                        composeServiceDefinition.Labels.Add(l[0], l[1]);
                    }

                    break;
                }
            }
        }

        private static void ResolveDependsOn(IReadOnlyDictionary<object, object> s, ComposeServiceDefinition composeServiceDefinition)
        {
            if(!s.ContainsKey("depends_on"))
                return;
            
            var dependsOn = (Dictionary<object, object>) s["depends_on"];
            foreach (var dependency in dependsOn)
            {
                composeServiceDefinition.DependsOn.Add((string) dependency.Key);
            }
        }

        private static void ResolveVolumes(Dictionary<object, object> s, ComposeServiceDefinition composeServiceDefinition)
        {
            if(!s.ContainsKey("volumes"))
                return;
            
            foreach (var volume in (List<object>) s["volumes"])
            {
                switch (volume)
                {
                    case string vSting:
                    {
                        var shortServiceVolumeDefinition = new ShortServiceVolumeDefinition
                        {
                            Entry = vSting
                        };
                        composeServiceDefinition.Volumes.Add(shortServiceVolumeDefinition);
                        break;
                    }
                    case Dictionary<object, object> vDictionary:
                    {
                        var longServiceVolumeDefinition = new LongServiceVolumeDefinition();

                        var bindType = (string) vDictionary["type"];
                        Enum.TryParse(bindType, true, out VolumeType b);

                        longServiceVolumeDefinition.Type = b;
                        longServiceVolumeDefinition.Source = (string) vDictionary["source"];
                        longServiceVolumeDefinition.Target = (string) vDictionary["target"];

                        composeServiceDefinition.Volumes.Add(longServiceVolumeDefinition);
                        break;
                    }
                }
            }
        }

        private static void ResolveHealthCheck(IReadOnlyDictionary<object, object> s, ComposeServiceDefinition composeServiceDefinition)
        {
            if(!s.ContainsKey("healthcheck"))
                return;
            
            var healthCheck = (Dictionary<object, object>) s["healthcheck"];
            var tests = (IList<object>) healthCheck["test"];

            composeServiceDefinition.HealthCheck = new HealthCheckDefinition
            {
                Test = tests.Select(t => (string) t).ToList()
            };
        }

        private static void ResolvePorts(IReadOnlyDictionary<object, object> s, ComposeServiceDefinition composeServiceDefinition)
        {
            if(!s.ContainsKey("ports"))
                return;
            
            switch (s["ports"])
            {
                case Dictionary<object, object> portsDictionary:
                {
                    foreach (var port in portsDictionary)
                    {
                        var portsShortDefinition = new PortsShortDefinition();
                        composeServiceDefinition.Ports.Add(portsShortDefinition);
                    }

                    break;
                }
                case List<object> portsList:
                {
                    foreach (var port in portsList)
                    {
                        var portsShortDefinition = new PortsShortDefinition
                        {
                            Entry = (string) port
                        };
                        composeServiceDefinition.Ports.Add(portsShortDefinition);
                    }

                    break;
                }
            }
        }

        private static void ResolveImage(Dictionary<object, object> s, ComposeServiceDefinition composeServiceDefinition)
        {
            if(!s.ContainsKey("image"))
                return;
            
            composeServiceDefinition.Image = (string) s["image"];
        }

        private static void ResolveIsolation(Dictionary<object, object> s, ComposeServiceDefinition composeServiceDefinition)
        {
            if(!s.ContainsKey("isolation"))
                return;
            
            var isolation = (string) s["isolation"];
            Enum.TryParse(isolation, out ContainerIsolationType i);
            composeServiceDefinition.Isolation = i;
        }
    }
}