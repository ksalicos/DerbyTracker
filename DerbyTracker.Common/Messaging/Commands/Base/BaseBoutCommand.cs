using DerbyTracker.Messaging.Commands;
using System;

namespace DerbyTracker.Common.Messaging.Commands.Base
{
    public abstract class BaseBoutCommand : CommandBase
    {
        public Guid BoutId { get; set; }

        protected BaseBoutCommand(Guid boutId, string originator) : base(originator)
        {
            BoutId = boutId;
        }
    }
}
