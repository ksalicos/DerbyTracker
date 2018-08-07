using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.PenaltyBoxTimer
{
    public class CancelSitCommand : BaseBoutCommand
    {
        public Guid ChairId { get; set; }

        public CancelSitCommand(Guid boutId, string originator, Guid chairId) : base(boutId, originator)
        {
            ChairId = chairId;
        }
    }
}
