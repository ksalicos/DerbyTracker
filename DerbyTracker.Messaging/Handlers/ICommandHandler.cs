using DerbyTracker.Messaging.Commands;

namespace DerbyTracker.Messaging.Handlers
{
    public interface ICommandHandler
    {
        ICommandResponse Handle(ICommand command);
    }
}
