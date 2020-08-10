using System;

namespace Dimmy.Cli.Commands.Project
{
    public interface IProjectSubCommand: ICommand
    {
        Type ArgumentType { get; }
    }
}