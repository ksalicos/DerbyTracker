using System;

namespace DerbyTracker.Common.Messaging.Commands.JamClock
{
    public class ExitPregameCommand : BaseBoutCommand
    {
        public ExitPregameCommand(Guid boutId, string originator) : base(boutId, originator)
        { }
    }
}
