using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.PenaltyBoxTimer
{
    public class ButtHitSeatCommand : BaseBoutCommand
    {
        public Chair Chair { get; }

        public ButtHitSeatCommand(Guid boutId, string originator, Chair chair) : base(boutId, originator)
        {
            Chair = chair;
        }
    }
}
