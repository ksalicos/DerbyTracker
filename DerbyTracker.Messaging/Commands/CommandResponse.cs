using DerbyTracker.Messaging.Events;
using System.Collections.Generic;

namespace DerbyTracker.Messaging.Commands
{
    public class CommandResponse : ICommandResponse
    {
        private readonly List<EventEnvelope> _events = new List<EventEnvelope>();
        public IEnumerable<EventEnvelope> Events => _events;

        public void AddEvent(IEvent @event, string audience)
        {
            _events.Add(new EventEnvelope { Event = @event, Audience = audience });
        }
    }
}
