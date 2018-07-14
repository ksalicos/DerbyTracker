using DerbyTracker.Messaging.Commands;

namespace DerbyTracker.Messaging.Handlers
{
    public abstract class CommandHandlerBase<T> : ICommandHandler where T : class, ICommand
    {
        public ICommandResponse Handle(ICommand command)
        {
            return Handle(command as T);
        }

        public abstract ICommandResponse Handle(T command);
    }
}
