using System.Threading.Tasks;

namespace DIMS.Engine.Commands
{
    public interface ICommandHandler<in TCommand>
        where TCommand : ICommand
    {
        Task Handle(TCommand command);
    }
}