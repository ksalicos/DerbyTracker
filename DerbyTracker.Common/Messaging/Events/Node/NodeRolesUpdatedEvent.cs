using System.Collections.Generic;

namespace DerbyTracker.Common.Messaging.Events.Node
{
    public class NodeRolesUpdatedEvent : BaseNodeEvent
    {
        public override string Type => "NODE_ROLES_UPDATED";
        public List<string> NewRoles { get; set; }

        public NodeRolesUpdatedEvent(string nodeId, List<string> newRoles) : base(nodeId)
        {
            NewRoles = newRoles;
        }
    }
}
