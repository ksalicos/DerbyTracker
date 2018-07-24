using System;
using System.Collections.Generic;

namespace DerbyTracker.Common.Entities
{
    public class BoutState
    {
        public BoutState(BoutData boutData)
        {
            BoutId = boutData.BoutId;
            LeftTeamState = new TeamState(boutData.RuleSet);
            RightTeamState = new TeamState(boutData.RuleSet);
        }

        public Guid BoutId { get; set; }
        public BoutPhase Phase { get; set; } = BoutPhase.Pregame;

        public StopWatch GameClock = new StopWatch();

        public DateTime JamStart { get; set; }
        public TimeSpan JamClock() => DateTime.Now - JamStart;
        public DateTime LineupStart { get; set; } //separate in case of undo
        public TimeSpan LineupClock() => DateTime.Now - LineupStart;

        public int Period { get; set; } = 1;
        public int JamNumber { get; set; } = 1;
        public List<Jam> Jams = new List<Jam> { new Jam(1, 1) };

        public void CreateNextJam()
        {
            JamNumber++;
            Jams.Add(new Jam(Period, JamNumber));
        }

        public TeamState LeftTeamState { get; set; }
        public TeamState RightTeamState { get; set; }


        public TimeoutType TimeoutType { get; set; }
        public bool LoseOfficialReview { get; set; }
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

    public enum TimeoutType
    {
        Official,
        LeftTeam,
        RightTeam,
        LeftReview,
        RightReview
    }
}
