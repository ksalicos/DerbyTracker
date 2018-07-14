namespace DerbyTracker.Messaging.Events
{
    public interface IEvent
    {
        // ReSharper disable once InconsistentNaming
        string type { get; }
    }
}
