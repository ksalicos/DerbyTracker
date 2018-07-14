using DerbyTracker.Messaging.Commands;
using System;

namespace DerbyTracker.Messaging.Callbacks
{
    /// <summary>
    /// Provided for testing/debugging purposes
    /// </summary>
    public class DelegateCallback : ICallback
    {
        private readonly Action<ICommandResponse> _callback;

        public DelegateCallback(Action<ICommandResponse> callback)
        {
            _callback = callback;
        }

        public void Callback(ICommandResponse response)
        {
            _callback(response);
        }
    }

    public class DelegateCallbackFactory : ICallbackFactory
    {
        public DelegateCallbackFactory()
        { _action = (ICommandResponse icr) => { }; }

        public DelegateCallbackFactory(Action<ICommandResponse> action)
        { _action = action; }

        private readonly Action<ICommandResponse> _action;

        public ICallback Get() => new DelegateCallback(_action);
    }
}
