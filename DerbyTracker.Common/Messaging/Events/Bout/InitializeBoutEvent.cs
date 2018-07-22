using DerbyTracker.Common.Entities;

namespace DerbyTracker.Common.Messaging.Events.Bout
{
    public class InitializeBoutEvent : BaseBoutEvent
    {
        public override string Type => "INITIALIZE_BOUT_STATE";
        public BoutState BoutState { get; set; }
        public RuleSet Rules { get; set; }

        public InitializeBoutEvent(BoutState boutState, RuleSet rules) : base(boutState.BoutId)
        {
            Rules = rules;
            BoutState = boutState;
        }
    }
}
