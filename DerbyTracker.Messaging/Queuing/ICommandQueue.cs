using DerbyTracker.Messaging.Commands;
using System.Threading.Tasks;

namespace DerbyTracker.Messaging.Queuing
{
    public interface ICommandEnqueuer
    {
        Task Enqueue(ICommand command);
    }

    public interface ICommandQueue
    {
        Task<ICommand> Pop();
    }
}
