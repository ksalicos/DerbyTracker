using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.LineupsTracker
{
    public class SetSkaterPositionCommand : BaseJamCommand
    {
        public string Team { get; set; }
        public int Number { get; set; }
        public Position Position { get; set; }

        public SetSkaterPositionCommand(Guid boutId, string originator, int period, int jam,
            string team, int number, Position position) : base(boutId, originator, period, jam)
        {
            Team = team;
            Number = number;
            Position = position;
        }
    }
}
