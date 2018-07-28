using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.PenaltyTracker
{
    public class UpdatePenaltyCommand : BaseBoutCommand
    {
        public Penalty Penalty { get; }

        public UpdatePenaltyCommand(Guid boutId, string originator, Penalty penalty) : base(boutId, originator)
        {
            Penalty = penalty;
        }
    }
}
