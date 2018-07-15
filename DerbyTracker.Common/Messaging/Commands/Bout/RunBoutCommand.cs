using System;

namespace DerbyTracker.Common.Messaging.Commands.Bout
{
    public class RunBoutCommand : BoutCommand
    {
        public RunBoutCommand(Guid boutId, string originator) : base(boutId, originator)
        { }
    }
}
