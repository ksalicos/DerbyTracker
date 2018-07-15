using System;

namespace DerbyTracker.Common.Exceptions
{
    public class BoutAlreadyRunningException : Exception
    {
        public Guid BoutId { get; set; }

        public BoutAlreadyRunningException(Guid boutId) : base($"Bout with id:[{boutId}] is already running.")
        {
            BoutId = boutId;
        }
    }
}
