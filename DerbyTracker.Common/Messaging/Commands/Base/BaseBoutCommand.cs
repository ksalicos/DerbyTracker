using DerbyTracker.Messaging.Commands;
using System;

namespace DerbyTracker.Common.Messaging.Commands.Base
{
    public abstract class BaseBoutCommand : CommandBase
    {
        public string NodeId { get; set; }
        public Guid BoutId { get; set; }

        protected BaseBoutCommand(string nodeId, Guid boutId, string originator) : base(originator)
        {
            NodeId = nodeId;
            BoutId = boutId;
        }
    }
}
