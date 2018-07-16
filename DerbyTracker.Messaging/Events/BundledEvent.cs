using System.Collections.Generic;

namespace DerbyTracker.Messaging.Events
{
    public class BundledEvents : IEvent
    {
        public string Type => "Bundled";
        public List<IEvent> Events = new List<IEvent>();
    }
}
