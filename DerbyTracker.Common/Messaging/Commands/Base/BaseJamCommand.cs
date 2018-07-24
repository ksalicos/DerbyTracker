using System;

namespace DerbyTracker.Common.Messaging.Commands.Base
{
    public abstract class BaseJamCommand : BaseBoutCommand
    {
        public int Period { get; set; }
        public int Jam { get; set; }

        protected BaseJamCommand(Guid boutId, string originator, int period, int jam) : base(boutId, originator)
        {
            Period = period;
            Jam = jam;
        }
    }
}
