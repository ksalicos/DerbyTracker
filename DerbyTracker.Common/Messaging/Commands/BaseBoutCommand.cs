using DerbyTracker.Messaging.Commands;
using System;

namespace DerbyTracker.Common.Messaging.Commands
{
    public abstract class BaseBoutCommand : CommandBase
    {
        public Guid BoutId { get; set; }
        public DateTime TimeStamp { get; set; }

        protected BaseBoutCommand(Guid boutId, string originator) : base(originator)
        {
            BoutId = boutId;
        }
    }
}
