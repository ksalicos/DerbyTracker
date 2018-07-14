using DerbyTracker.Messaging.Events;
using System.Collections.Generic;

namespace DerbyTracker.Common.Messaging.Events.Node
{
    public class NodeConnectedEvent : BaseEvent
    {
        public override string type => "NODE_CONNECTED";
        public string NodeId { get; set; }
        public int ConnectionNumber { get; set; }
        public IEnumerable<string> Roles { get; set; }

        public NodeConnectedEvent(string nodeId, int connectionNumber, IEnumerable<string> roles)
        {
            NodeId = nodeId;
            ConnectionNumber = connectionNumber;
            Roles = roles;
        }
    }
}
