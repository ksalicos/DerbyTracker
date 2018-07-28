using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.Events.Base;

namespace DerbyTracker.Common.Messaging.Events.Bout
{
    public class InitializeBoutEvent : BaseBoutEvent
    {
        public override string Type => "INITIALIZE_BOUT_STATE";
        public BoutState BoutState { get; set; }
        public BoutData BoutData { get; set; }

        public InitializeBoutEvent(BoutData boutData, BoutState boutState) : base(boutState.BoutId)
        {
            BoutData = boutData;
            BoutState = boutState;
        }
    }
}
