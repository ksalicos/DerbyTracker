namespace DerbyTracker.Messaging.Events
{
    public enum MessageType
    {
        Error,
        Group,
        System
    }

    public class MessageBaseEvent : BaseEvent
    {
        public MessageType MessageType { get; set; }
        public string Message { get; set; }

        public static MessageBaseEvent Error(string message)
        { return new MessageBaseEvent { MessageType = MessageType.Error, Message = message }; }
    }
}
