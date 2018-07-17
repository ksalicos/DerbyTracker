using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.Bout
{
    public class RunBoutCommand : BaseBoutCommand
    {
        public RunBoutCommand(string nodeId, Guid boutId, string originator) : base(nodeId, boutId, originator)
        { }
    }
}
