using System;

namespace DerbyTracker.Common.Exceptions
{
    public class NoSuchPassException : Exception
    {
        public int Number { get; set; }

        public NoSuchPassException(int number) : base(
            $"Attempted to modify pass [{number}] but it has not been created.")
        {
            Number = number;
        }
    }
}
