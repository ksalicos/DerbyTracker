using DerbyTracker.Common.Messaging.Commands.Base;
using System;

namespace DerbyTracker.Common.Messaging.Commands.JamClock
{
    public class SetLoseOfficialReviewCommand : BaseBoutCommand
    {
        public bool LoseOfficialReview { get; set; }

        public SetLoseOfficialReviewCommand(Guid boutId, string originator, bool loseOfficialReview) : base(boutId, originator)
        { LoseOfficialReview = loseOfficialReview; }
    }
}
