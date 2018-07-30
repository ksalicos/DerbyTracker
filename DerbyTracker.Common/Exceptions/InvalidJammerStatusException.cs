using System;

namespace DerbyTracker.Common.Exceptions
{
    public class InvalidJammerStatusException : Exception
    {
        public InvalidJammerStatusException(string message) : base(message)
        { }
    }
}
