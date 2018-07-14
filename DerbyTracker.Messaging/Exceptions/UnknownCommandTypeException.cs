using DerbyTracker.Messaging.Commands;
using System;

namespace DerbyTracker.Messaging.Exceptions
{
    public class UnknownCommandTypeException : Exception
    {
        public ICommand Command;
        public UnknownCommandTypeException(ICommand command)
            : base($"Unknown Command Id: {command.CommandId}")
        {
            this.Command = command;
        }
    }
}