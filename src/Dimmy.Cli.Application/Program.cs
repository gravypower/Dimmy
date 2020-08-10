
using System.CommandLine;
using System.Threading.Tasks;
using SimpleInjector.Lifestyles;
using ICommand = Dimmy.Cli.Commands.ICommand;

namespace Dimmy.Cli.Application
{
    internal class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var container = Bootstrapper.Bootstrap();

            await using (AsyncScopedLifestyle.BeginScope(container))
            {
                var rootCommand = new RootCommand();

                foreach (var commandLineCommand in container.GetAllInstances<ICommand>())
                    rootCommand.AddCommand(commandLineCommand.GetCommand());

                return await rootCommand.InvokeAsync(args);
            }
        }
    }
}