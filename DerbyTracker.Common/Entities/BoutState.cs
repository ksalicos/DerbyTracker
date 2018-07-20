using System;

namespace DerbyTracker.Common.Entities
{
    public class BoutState
    {
        public BoutState()
        {
            LeftTeamState = new TeamState();
            RightTeamState = new TeamState();
        }

        public Guid BoutId { get; set; }
        public BoutPhase Phase { get; set; } = BoutPhase.Pregame;

        //When a timeout is called, replace GameTimeElapsed with GameClock()
        //Then set Lastclock to now when it starts
        public TimeSpan GameClockElapsed { get; set; }
        public DateTime LastClockStart { get; set; }
        public bool ClockRunning { get; set; }
        public TimeSpan GameClock() => ClockRunning ? GameClockElapsed + (DateTime.Now - LastClockStart) : GameClockElapsed;

        public DateTime JamStart { get; set; }
        public TimeSpan JamClock() => DateTime.Now - JamStart;
        public DateTime LineupStart { get; set; } //separate in case of undo
        public TimeSpan LineupClock() => DateTime.Now - LineupStart;

        public int Period { get; set; } = 1;
        public int Jam { get; set; } = 1;
        //public int LeftPass { get; set; }
        //public int RightPass { get; set; }

        public TeamState LeftTeamState { get; set; }
        public TeamState RightTeamState { get; set; }
    }

    public enum BoutPhase
    {
        Pregame,
        Lineup,
        Jam,
        Timeout,
        Halftime,
        UnofficialFinal,
        Final
    }
}
