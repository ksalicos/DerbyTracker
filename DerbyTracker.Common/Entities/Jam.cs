using System.Collections.Generic;

namespace DerbyTracker.Common.Entities
{
    public class Jam
    {
        public int Period { get; set; }
        public int JamNumber { get; set; }

        public List<JamParticipant> LeftRoster = new List<JamParticipant>();
        public List<JamParticipant> RightRoster = new List<JamParticipant>();

        public Jam(int period, int jamNumber)
        {
            Period = period;
            JamNumber = jamNumber;
        }
    }

    public class JamParticipant
    {
        public int Number { get; set; }
        public Position Position { get; set; }
    }

    public enum Position
    {
        Blocker, Jammer, Pivot
    }
}
