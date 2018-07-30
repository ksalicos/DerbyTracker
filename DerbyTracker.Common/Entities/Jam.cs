using DerbyTracker.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DerbyTracker.Common.Entities
{
    public class Jam
    {
        public int Period { get; set; }
        public int JamNumber { get; set; }

        public TeamJam Left = new TeamJam();
        public TeamJam Right = new TeamJam();

        public TeamJam Team(string team)
        {
            switch (team)
            {
                case "left":
                    return Left;
                case "right":
                    return Right;
                default:
                    throw new Exception("Invalid Team String Passed");
            }
        }

        public Jam(int period, int jamNumber)
        {
            Period = period;
            JamNumber = jamNumber;
            Left.AddPass();
            Right.AddPass();
        }
    }

    public class TeamJam
    {
        public JammerStatus JammerStatus { get; set; }
        public List<Pass> Passes = new List<Pass>();
        public List<JamParticipant> Roster = new List<JamParticipant>();

        public void AddPass(int number = -1)
        {
            if (Passes.Any(x => x.Number == number))
            {
                throw new InvalidPassException($"Pass Already Exists: {number}");
            }
            if (number == -1)
            {
                number = Passes.Any() ? Passes.Max(x => x.Number) + 1 : 0;
            }

            Passes.Add(new Pass { Number = number });
        }
    }

    public class Pass
    {
        public int Number { get; set; }
        public int Score { get; set; }
        public bool StarPass { get; set; }
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

    public enum JammerStatus
    {
        NoInitialPass,
        Lead,
        NotLead,
        LostLead,
        CantGainLead
    }
}
