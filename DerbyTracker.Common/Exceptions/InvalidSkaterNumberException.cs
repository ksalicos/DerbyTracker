using System;

namespace DerbyTracker.Common.Exceptions
{
    public class InvalidSkaterNumberException : Exception
    {
        public string Team { get; set; }
        public int Number { get; set; }

        public InvalidSkaterNumberException(string team, int number) : base(
            $"Attempted to access skater with number {number} from the {team} team")
        {
            Team = team;
            Number = number;
        }
    }
}
