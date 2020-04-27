using System.CommandLine;
using System.Threading.Tasks;
using Dimmy.Cli.Commands;
using SimpleInjector.Lifestyles;

namespace Dimmy.Cli.Application
{
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var container = Bootstrapper.Bootstrap();

            using (AsyncScopedLifestyle.BeginScope(container))
            {
                var rootCommand = new RootCommand();

                foreach (var commandLineCommand in container.GetAllInstances<ICommandLineCommand>())
                {
                    rootCommand.AddCommand(commandLineCommand.GetCommand());
                }

                return await rootCommand.InvokeAsync(args);
            }
        }
    }
}