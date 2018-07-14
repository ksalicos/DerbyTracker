using DerbyTracker.Messaging.Events;
using System.Collections.Generic;

namespace DerbyTracker.Messaging.Commands
{
    public interface ICommandResponse
    {
        IEnumerable<EventEnvelope> Events { get; }
    }
}