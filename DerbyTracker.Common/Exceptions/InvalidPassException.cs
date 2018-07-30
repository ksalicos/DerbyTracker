using System;

namespace DerbyTracker.Common.Exceptions
{
    public class InvalidPassException : Exception
    {
        public InvalidPassException(string message) : base(message)
        {
        }
    }
}
