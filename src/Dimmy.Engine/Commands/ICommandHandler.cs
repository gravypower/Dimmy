using System.Threading.Tasks;

namespace Dimmy.Engine.Commands
{
    public interface ICommandHandler<in TCommand>
        where TCommand : ICommand
    {
        Task Handle(TCommand command);
    }
}