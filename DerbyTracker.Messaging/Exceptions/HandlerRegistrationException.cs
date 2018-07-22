using System;

namespace DerbyTracker.Messaging.Exceptions
{
    public class HandlerRegistrationException : Exception
    {
        public string HandlerName { get; set; }

        public HandlerRegistrationException(string handler, string message) : base(message)
        { HandlerName = handler; }
    }
}
