using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.JamClock
{
    public class StartTimeoutCommand : BaseBoutCommand
    {
        public StartTimeoutCommand(Guid boutId, string originator) : base(boutId, originator)
        { }
    }
}
