using DerbyTracker.Messaging.Commands;

namespace DerbyTracker.Messaging.Callbacks
{
    public interface ICallback
    {
        void Callback(ICommandResponse response);
    }
}
