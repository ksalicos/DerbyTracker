using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.PenaltyTracker
{
    public class CreatePenaltyCommand : BaseJamCommand
    {
        public string Team { get; set; }

        public CreatePenaltyCommand(Guid boutId, string originator, int period, int jam, string team) : base(boutId, originator, period, jam)
        { Team = team; }
    }
}
