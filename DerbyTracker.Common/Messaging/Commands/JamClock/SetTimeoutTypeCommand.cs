using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.JamClock
{
    public class SetTimeoutTypeCommand : BaseBoutCommand
    {
        public TimeoutType TimeoutType { get; set; }

        public SetTimeoutTypeCommand(Guid boutId, string originator, TimeoutType timeoutType) : base(boutId, originator)
        {
            TimeoutType = timeoutType;
        }
    }
}
