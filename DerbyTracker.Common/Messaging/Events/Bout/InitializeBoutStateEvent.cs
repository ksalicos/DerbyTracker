using DerbyTracker.Common.Entities;

namespace DerbyTracker.Common.Messaging.Events.Bout
{
    public class InitializeBoutStateEvent : BaseBoutEvent
    {
        public override string Type => "INITIALIZE_BOUT_STATE";
        public BoutState BoutState { get; set; }

        public InitializeBoutStateEvent(BoutState boutState) : base(boutState.BoutId)
        {
            BoutState = boutState;
        }
    }
}
