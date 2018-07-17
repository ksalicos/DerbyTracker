using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.JamClock
{
    public class StopJamCommand : BaseBoutCommand
    {
        public StopJamCommand(Guid boutId, string originator) : base(boutId, originator)
        { }
    }
}
