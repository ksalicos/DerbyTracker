using DerbyTracker.Common.Entities;

namespace DerbyTracker.Common.Messaging.Events.Bout
{
    public class BoutStateUpdatedEvent : BaseBoutEvent
    {
        public override string Type => "INITIALIZE_BOUT_STATE";
        public BoutState BoutState { get; set; }

        public BoutStateUpdatedEvent(BoutState boutState) : base(boutState.BoutId)
        {
            BoutState = boutState;
        }
    }
}
