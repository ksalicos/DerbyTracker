namespace DerbyTracker.Messaging.Events
{
    /// <summary>
    /// Container to link Events to their destinations.
    /// </summary>
    public class EventEnvelope
    {
        public string Audience { get; set; }
        public IEvent Event { get; set; }
    }
}
