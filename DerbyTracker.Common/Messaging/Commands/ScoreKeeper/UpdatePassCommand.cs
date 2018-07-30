using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.ScoreKeeper
{
    public class UpdatePassCommand : BaseJamCommand
    {
        public string Team { get; set; }
        public Pass Pass { get; set; }

        public UpdatePassCommand(Guid boutId, string originator, int period, int jam, string team, Pass pass) : base(
            boutId, originator, period, jam)
        {
            Team = team;
            Pass = pass;
        }
    }
}
