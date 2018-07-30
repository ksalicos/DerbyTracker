using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.Events.ScoreKeeper;
using DerbyTracker.Messaging.Commands;
using System;

namespace DerbyTracker.Common.Messaging
{
    public class UpdateJamResponse : CommandResponse
    {
        public UpdateJamResponse(Guid boutId, Jam jam)
        {
            AddEvent(new JamUpdatedEvent(boutId, jam), Audiences.Bout(boutId));
        }
    }
}
