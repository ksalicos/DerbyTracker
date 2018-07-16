namespace DerbyTracker.Messaging.Events
{
    public abstract class BaseEvent : IEvent
    {
        public virtual string Type => this.GetType().Name;
    }
}
