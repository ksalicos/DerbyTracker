using System;

namespace DerbyTracker.Common.Exceptions
{
    public class NoTimeoutsRemainingException : Exception
    {
        public NoTimeoutsRemainingException() : base("A timeout command was called for a team that had none.")
        { }
    }
}
