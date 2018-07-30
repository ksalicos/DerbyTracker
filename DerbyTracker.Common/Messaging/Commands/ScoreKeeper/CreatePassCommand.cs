using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.ScoreKeeper
{
    public class CreatePassCommand : BaseJamCommand
    {
        public string Team { get; set; }
        public int Number { get; set; }

        public CreatePassCommand(Guid boutId, string originator, int period, int jam, string team,
            int number = -1) : base(boutId, originator, period, jam)
        {
            Team = team;
            Number = number;
        }
    }
}
