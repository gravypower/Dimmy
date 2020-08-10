using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using Dimmy.Cli.Commands;

namespace Dimmy.HelloWorld.Plugin.Commands
{
    public class HelloWorldCommandLineCommand : Command
    {
        public Command GetCommand()
        {
            var helloNameCommand = new Command("hello", "Greets you")
            {
                Handler = CommandHandler.Create((HelloWorldCommandLineArgument arg) =>
                {
                    Console.WriteLine($"Hello {arg.Name}");
                })
            };

            helloNameCommand.AddOption(new Option<string>("--name", "Greets you with you name"));

            return helloNameCommand;
        }

        public HelloWorldCommandLineCommand(string name, string? description = null) : base(name, description)
        {
        }
    }
}