using DerbyTracker.Messaging.Events;
using System.Collections.Generic;

namespace DerbyTracker.Common.Messaging.Events.Node
{
    public class NodeRolesUpdatedEvent : BaseEvent
    {
        public override string type => "NODE_ROLES_UPDATED";
        public string NodeId { get; set; }
        public List<string> NewRoles { get; set; }

        public NodeRolesUpdatedEvent(string nodeId, List<string> newRoles)
        {
            NodeId = nodeId;
            NewRoles = newRoles;
        }
    }
}
