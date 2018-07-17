using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.JamClock
{
    public class EnterLineupPhaseCommand : BaseBoutCommand
    {
        public EnterLineupPhaseCommand(Guid boutId, string originator) : base(boutId, originator)
        { }
    }
}
