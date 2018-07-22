using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.JamClock
{
    public class StopTimeoutCommand : BaseBoutCommand
    {
        public StopTimeoutCommand(Guid boutId, string originator) : base(boutId, originator)
        { }
    }
}
