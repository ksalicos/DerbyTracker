using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.Events.PenaltyBoxTimer;
using DerbyTracker.Messaging.Commands;
using System;

namespace DerbyTracker.Common.Messaging
{
    public class UpdatePenaltySeatResponse : CommandResponse
    {
        public UpdatePenaltySeatResponse(Guid boutId, Chair chair)
        {
            AddEvent(new ChairUpdatedEvent(boutId, chair), Audiences.Bout(boutId));
        }
    }
}
