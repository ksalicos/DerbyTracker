using System;

namespace DerbyTracker.Common.Exceptions
{
    public class BoutNotFoundException : Exception
    {
        public Guid BoutId { get; set; }

        public BoutNotFoundException(Guid boutId) : base($"The bout with ID {boutId} was not found.")
        { BoutId = boutId; }
    }
}
