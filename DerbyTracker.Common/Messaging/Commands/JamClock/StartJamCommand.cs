﻿using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.JamClock
{
    public class StartJamCommand : BaseBoutCommand
    {
        public StartJamCommand(Guid boutId, string originator) : base(boutId, originator)
        { }
    }
}
