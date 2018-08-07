using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.PenaltyBoxTimer
{
    public class UpdateChairCommand : BaseBoutCommand
    {
        public Chair Chair { get; set; }

        public UpdateChairCommand(Guid boutId, string originator, Chair chair) : base(boutId, originator)
        {
            Chair = chair;
        }
    }
}
