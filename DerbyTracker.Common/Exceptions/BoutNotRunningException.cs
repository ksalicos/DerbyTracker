using System;

namespace DerbyTracker.Common.Exceptions
{
    public class BoutNotRunningException : Exception
    {
        public Guid BoutId { get; set; }

        public BoutNotRunningException(Guid boutId) : base($"Bout with id:[{boutId}] is not running.")
        { BoutId = boutId; }
    }
}
