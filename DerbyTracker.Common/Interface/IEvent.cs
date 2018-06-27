using System;

namespace DerbyTracker.Common.Interface
{
    public interface IEvent
    {
        string Type { get; }
        string Sender { get; }
        Guid BoutId { get; set; }
        DateTime Timestamp { get; }
    }
}
