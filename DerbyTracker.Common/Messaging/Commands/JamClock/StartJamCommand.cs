using DerbyTracker.Common.Messaging.CommandHandlers.Bout;
using System;

namespace DerbyTracker.Common.Messaging.Commands.JamClock
{
    public class StartJamCommand : BoutCommandBase
    {
        public StartJamCommand(Guid boutId, string originator) : base(boutId, originator)
        { }
    }
}
