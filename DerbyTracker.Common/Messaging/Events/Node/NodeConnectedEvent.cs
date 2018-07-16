using DerbyTracker.Common.Services;

namespace DerbyTracker.Common.Messaging.Events.Node
{
    public class NodeConnectedEvent : BaseNodeEvent
    {
        public override string Type => "NODE_CONNECTED";
        public NodeConnection Data;

        public NodeConnectedEvent(NodeConnection connection) : base(connection.NodeId)
        {
            Data = connection;
        }
    }
}
