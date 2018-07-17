using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.JamClock
{
    public class ExitPregameCommand : BaseBoutCommand
    {
        public ExitPregameCommand(string nodeId, Guid boutId, string originator) : base(nodeId, boutId, originator)
        { }
    }
}
