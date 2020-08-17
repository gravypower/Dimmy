using System;
using System.CommandLine;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Dimmy.Engine.Services;
using SimpleInjector.Lifestyles;
using ICommand = Dimmy.Cli.Commands.ICommand;

namespace Dimmy.Cli.Application
{
    internal class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var container = Bootstrapper.Bootstrap();

            var baseDirectory = AppContext.BaseDirectory;
            
            var certPath = Path.Combine(baseDirectory, $"dimmy.pfx");
            if(!File.Exists(certPath))
            {
                var certificateService = container.GetInstance<ICertificateService>();
                var dimmyCert = certificateService.CreateCaCertificate("dimmy", "dimmy.local");
                using var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadWrite);
                store.Add(dimmyCert);
                
                await File.WriteAllBytesAsync(certPath, dimmyCert.Export(X509ContentType.Pfx));
            }
            
            await using (AsyncScopedLifestyle.BeginScope(container))
            {
                var rootCommand = new RootCommand();

                foreach (var commandLineCommand in container.GetAllInstances<ICommand>())
                    rootCommand.AddCommand(commandLineCommand.BuildCommand());

                return await rootCommand.InvokeAsync(args);
            }
        }
    }
}