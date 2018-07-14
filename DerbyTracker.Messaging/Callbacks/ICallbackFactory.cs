namespace DerbyTracker.Messaging.Callbacks
{
    public interface ICallbackFactory
    {
        ICallback Get();
    }
}
