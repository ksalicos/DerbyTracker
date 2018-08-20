using DerbyTracker.Messaging.Events;
using System;

namespace DerbyTracker.Common.Messaging.Events.Node
{
    public class NodeJoinedBoutEvent : BaseEvent
    {
        public override string Type => "NODE_JOINED_BOUT";
        public string NodeId { get; set; }
        public Guid BoutId { get; set; }

        public NodeJoinedBoutEvent(string nodeId, Guid boutId)
        {
            NodeId = nodeId;
            BoutId = boutId;
        }
    }
}
