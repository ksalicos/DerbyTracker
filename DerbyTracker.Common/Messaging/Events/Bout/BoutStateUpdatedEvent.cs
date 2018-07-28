using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.Events.Base;

namespace DerbyTracker.Common.Messaging.Events.Bout
{
    public class BoutStateUpdatedEvent : BaseBoutEvent
    {
        public override string Type => "UPDATE_BOUT_STATE";
        public BoutState BoutState { get; set; }

        public BoutStateUpdatedEvent(BoutState boutState) : base(boutState.BoutId)
        {
            BoutState = boutState;
        }
    }
}
