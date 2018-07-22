using DerbyTracker.Messaging.Callbacks;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Events;
using DerbyTracker.Messaging.Exceptions;
using DerbyTracker.Messaging.Handlers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DerbyTracker.Messaging.Dispatchers
{
    /// <summary>
    /// When a command is received, execute the handler immediately.
    /// </summary>
    public class ImmediateDispatcher : IDispatcher
    {
        protected readonly Dictionary<string, ICommandHandler> Handlers = new Dictionary<string, ICommandHandler>();
        private readonly ICallbackFactory _callbackFactory;

        public ImmediateDispatcher(ICallbackFactory callbackFactory)
        {
            _callbackFactory = callbackFactory;
        }

        public void RegisterHandler(ICommandHandler handler)
        {
            var att = (HandlesAttribute)Attribute.GetCustomAttribute(handler.GetType(), typeof(HandlesAttribute));

            if (Handlers.ContainsKey(att.CommandType))
            {
                throw new Exception($"Duplicate Type Registration: {att.CommandType}");
            }

            Handlers[att.CommandType] = handler;
        }

#pragma warning disable 1998
        public async Task Dispatch(ICommand command)
#pragma warning restore 1998
        {
            var callback = _callbackFactory.Get();
            var key = command.GetType().Name;
            if (!Handlers.ContainsKey(key))
            { throw new UnknownCommandTypeException(command); }

            try
            {
                var response = Handlers[key].Handle(command);
                callback.Callback(response);
            }
            catch (Exception ex)
            {
                var response = new CommandResponse();
                response.AddEvent(MessageBaseEvent.Error(ex.Message), command.Originator);
                //TODO: Better error handling
                callback.Callback(response);
            }
        }

    }
}