using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Queuing;
using System.Threading.Tasks;

namespace DerbyTracker.Messaging.Dispatchers
{
    public class EnquingDispatcher : IDispatcher
    {
        private readonly ICommandEnqueuer _queue;
        public EnquingDispatcher(ICommandEnqueuer queue)
        {
            _queue = queue;
        }

        public async Task Dispatch(ICommand command)
        {
            await _queue.Enqueue(command);
        }
    }
}
