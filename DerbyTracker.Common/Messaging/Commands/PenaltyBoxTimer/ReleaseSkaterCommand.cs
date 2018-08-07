using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.PenaltyBoxTimer
{
    public class ReleaseSkaterCommand : BaseBoutCommand
    {
        public Guid ChairId { get; set; }

        public ReleaseSkaterCommand(Guid boutId, string originator, Guid chairId) : base(boutId, originator)
        {
            ChairId = chairId;
        }
    }
}
