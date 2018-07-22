using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.JamClock
{
    public class StartPeriodCommand : BaseBoutCommand
    {
        public StartPeriodCommand(Guid boutId, string originator) : base(boutId, originator)
        { }
    }
}
