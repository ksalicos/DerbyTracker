using System;

namespace DerbyTracker.Common.Entities
{
    public class BoutState
    {
        public enum BoutPhase
        {
            Pregame,
            Lineup,
            Jam,
            Timeout,
            Halftime,
        }

        public BoutState()
        {
            LeftTeamState = new TeamState();
            RightTeamState = new TeamState();
        }

        public TimeSpan GameTimeElapsed { get; set; }
        public DateTime LastClock { get; set; }
        public bool ClockRunning { get; set; }
        public TimeSpan GameClock => ClockRunning ? GameTimeElapsed + (DateTime.Now - LastClock) : GameTimeElapsed;

        public DateTime JamStart { get; set; }
        public TimeSpan JamClock => DateTime.Now - JamStart;

        public TeamState LeftTeamState { get; set; }
        public TeamState RightTeamState { get; set; }
    }
}
