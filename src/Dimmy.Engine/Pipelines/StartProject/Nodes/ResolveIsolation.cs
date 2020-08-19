using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Dimmy.Engine.Pipelines.StartProject.Nodes
{
    public class ResolveIsolation:Node<IStartProjectContext>
    {
        public override void DoExecute(IStartProjectContext input)
        {
            if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return;


            using var key = Registry.CurrentUser.OpenSubKey(@"Software\\Microsoft\\Windows NT\\CurrentVersion");
            if (key == null) return;
            
            var releaseId = (string)key.GetValue("ReleaseId");
            
            
            
        }
    }
}