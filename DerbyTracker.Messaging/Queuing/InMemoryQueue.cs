using DerbyTracker.Messaging.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerbyTracker.Messaging.Queuing
{
    /// <summary>
    /// Quick and dirty message queue.
    /// </summary>
    public class InMemoryQueue : ICommandEnqueuer, ICommandQueue
    {
        private readonly List<ICommand> _list = new List<ICommand>();

        public async Task Enqueue(ICommand command)
        {
            await Task.Run(() =>
            {
                lock (_list)
                {
                    _list.Add(command);
                }
            });
        }

        public async Task<ICommand> Pop()
        {
            ICommand retVal = null;
            await Task.Run(() =>
            {
                lock (_list)
                {
                    if (!_list.Any()) return;
                    retVal = _list.First();
                    _list.RemoveAt(0);
                }
            });
            return retVal;
        }
    }
}
