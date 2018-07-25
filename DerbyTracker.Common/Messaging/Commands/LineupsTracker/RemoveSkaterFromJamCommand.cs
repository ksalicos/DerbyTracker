﻿using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.LineupsTracker
{
    public class RemoveSkaterFromJamCommand : BaseJamCommand
    {
        public string Team { get; set; }
        public int Number { get; set; }

        public RemoveSkaterFromJamCommand(Guid boutId, string originator, int period, int jam, string team,
            int number) : base(boutId, originator, period, jam)
        {
            Team = team;
            Number = number;
        }
    }
}
