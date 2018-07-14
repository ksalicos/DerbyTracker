using DerbyTracker.Messaging.Commands;

namespace DerbyTracker.Common.Messaging.Commands.Node
{
    public class ConnectNodeCommand : CommandBase
    {
        public string ConnectionId { get; }

        public ConnectNodeCommand(string originator, string connectionId) : base(originator)
        {
            ConnectionId = connectionId;
        }
    }
}
