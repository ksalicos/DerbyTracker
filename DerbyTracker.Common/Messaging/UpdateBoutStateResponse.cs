using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.Events.Bout;
using DerbyTracker.Messaging.Commands;

namespace DerbyTracker.Common.Messaging
{
    public class UpdateBoutStateResponse : CommandResponse
    {
        public UpdateBoutStateResponse(BoutState state)
        {
            AddEvent(new BoutStateUpdatedEvent(state), Audiences.Bout(state.BoutId));
        }
    }
}
