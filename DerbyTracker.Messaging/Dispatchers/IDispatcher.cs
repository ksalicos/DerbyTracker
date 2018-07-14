using DerbyTracker.Messaging.Commands;
using System.Threading.Tasks;

namespace DerbyTracker.Messaging.Dispatchers
{
    public interface IDispatcher
    {
        Task Dispatch(ICommand command);
    }
}
