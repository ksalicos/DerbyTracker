using DerbyTracker.Messaging.Commands;
using System;

namespace DerbyTracker.Common.Messaging.CommandHandlers.Bout
{
    public abstract class BoutCommandBase : CommandBase
    {
        public Guid BoutId { get; set; }

        protected BoutCommandBase(Guid boutId, string originator) : base(originator)
        {
            BoutId = boutId;
        }
    }
}
