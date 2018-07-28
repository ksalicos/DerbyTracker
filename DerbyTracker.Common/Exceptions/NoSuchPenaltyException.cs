using System;

namespace DerbyTracker.Common.Exceptions
{
    public class NoSuchPenaltyException : Exception
    {
        public Guid Id { get; set; }

        public NoSuchPenaltyException(Guid id) : base($"Attempted to access penalty with id [{id}] but none exist.")
        {
            Id = id;
        }
    }
}
