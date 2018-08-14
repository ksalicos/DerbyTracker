using System;

namespace DerbyTracker.Common.Entities
{
    public class Penalty
    {
        public Penalty(string team, int period, int jam, TimeSpan timeStamp, int secondsOwed, int number = -1, string code = null)
        {
            Id = Guid.NewGuid();
            Team = team;
            Period = period;
            JamNumber = jam;
            GameClock = timeStamp;
            SecondsOwed = secondsOwed;
            Number = number;
            PenaltyCode = code;
        }

        public Guid Id { get; set; }
        public string Team { get; set; }
        public int Number { get; set; } = -1;
        public string PenaltyCode { get; set; }
        public int Period { get; set; }
        public int JamNumber { get; set; }
        public TimeSpan GameClock { get; set; }
        public int SecondsOwed { get; set; }
    }
}
