using System;
using System.Collections.Generic;
using System.Linq;

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
        public DateTime TimeOutStart { get; set; }

        public int Period { get; set; } = 1;
        public int JamNumber { get; set; } = 1;
        public List<Jam> Jams = new List<Jam> { new Jam(1, 1) };

        public TimeoutType TimeoutType { get; set; }
        public bool LoseOfficialReview { get; set; }

        public TeamState LeftTeamState { get; set; }
        public TeamState RightTeamState { get; set; }

        public List<Penalty> Penalties = new List<Penalty>();
        public List<Chair> PenaltyBox = new List<Chair>();

        public void CreateNextJam()
        {
            JamNumber++;
            Jams.Add(new Jam(Period, JamNumber));
        }

        public TeamState TeamState(string team)
        {
            switch (team)
            {
                case "left": return LeftTeamState;
                case "right": return RightTeamState;
                default: throw new Exception("Invalid team string");
            }
        }

        public int? GetCurrentJammer(string team)
        {
            var currentJam = team == "left" ? Jams.Last().Left : Jams.Last().Right;
            var jammer = currentJam.Passes.Any(x => x.StarPass) ? currentJam.Roster.SingleOrDefault(x => x.Position == Position.Pivot)
                : currentJam.Roster.SingleOrDefault(x => x.Position == Position.Jammer);
            return jammer?.Number;
        }
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
