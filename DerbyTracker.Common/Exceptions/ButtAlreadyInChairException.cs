using System;

namespace DerbyTracker.Common.Exceptions
{
    public class ButtAlreadyInChairException : Exception
    {
        public Guid SitId { get; set; }

        public ButtAlreadyInChairException(Guid id) : base($"Attempted to re-add sit: [{id}]")
        {
            SitId = id;
        }
    }
}
