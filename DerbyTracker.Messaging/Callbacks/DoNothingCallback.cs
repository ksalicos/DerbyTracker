using DerbyTracker.Messaging.Commands;

namespace DerbyTracker.Messaging.Callbacks
{
    /// <summary>
    /// Provided for testing purposes
    /// </summary>
    public class DoNothingCallback : ICallback
    {
        public void Callback(ICommandResponse result) { }
    }

    public class DoNothingCallbackFactory : ICallbackFactory
    {
        public ICallback Get() => new DoNothingCallback();
    }
}