using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.Node
{
    public class AddNodeToBoutCommand : BaseBoutCommand
    {
        public string NodeId { get; set; }

        public AddNodeToBoutCommand(string nodeId, Guid boutId, string originator) : base(boutId, originator)
        { NodeId = nodeId; }
    }
}
