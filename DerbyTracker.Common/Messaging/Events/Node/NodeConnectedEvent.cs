using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Events;

namespace DerbyTracker.Common.Messaging.Events.Node
{
    public class NodeConnectedEvent : BaseEvent
    {
        public override string type => "NODE_CONNECTED";
        public NodeConnection data;

        public NodeConnectedEvent(NodeConnection connection)
        {
            data = connection;
        }
    }
}
