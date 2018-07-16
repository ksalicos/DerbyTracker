using DerbyTracker.Messaging.Events;
using System;

namespace DerbyTracker.Common.Messaging.Events.Node
{
    public abstract class BaseNodeEvent : BaseEvent
    {
        public string NodeId { get; set; }
        public DateTime ServerTime { get; set; }

        protected BaseNodeEvent(string nodeId)
        {
            NodeId = nodeId;
            ServerTime = DateTime.Now;
        }
    }
}
