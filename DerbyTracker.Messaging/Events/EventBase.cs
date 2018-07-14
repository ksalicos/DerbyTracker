namespace DerbyTracker.Messaging.Events
{
    public abstract class BaseEvent : IEvent
    {
        public virtual string type => this.GetType().Name;
    }
}
