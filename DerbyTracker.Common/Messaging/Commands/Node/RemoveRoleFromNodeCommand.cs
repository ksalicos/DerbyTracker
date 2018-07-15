using DerbyTracker.Messaging.Commands;

namespace DerbyTracker.Common.Messaging.Commands.Node
{
    public class RemoveRoleFromNodeCommand : CommandBase
    {
        public string NodeId { get; set; }
        public string Role { get; set; }

        public RemoveRoleFromNodeCommand(string nodeId, string role, string originator) : base(originator)
        {
            NodeId = nodeId;
            Role = role;
        }
    }
}
