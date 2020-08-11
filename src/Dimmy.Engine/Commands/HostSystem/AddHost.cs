using System.Net;

namespace Dimmy.Engine.Commands.HostSystem
{
    public class AddHost : ICommand
    {
        public IPAddress Address { get; set; }
        public string Hostname { get; set; }
        public string Comment { get; set; }
    }
}