using System;

namespace DerbyTracker.Messaging.Commands
{
    public interface ICommand
    {
        Guid CommandId { get; }
        string Originator { get; }
    }
}
