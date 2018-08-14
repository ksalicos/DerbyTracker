using DerbyTracker.Common.Entities;
using System;

namespace DerbyTracker.Common.Services.Mocks
{
    /// <summary>
    /// This is to be used to set up tests, and assumes the bout data returned from the 
    /// Guid.Empty bout in MockBoutFileService.
    /// </summary>
    public class BoutStateBuilder
    {
        private readonly BoutState _state;

        public BoutStateBuilder(BoutState state)
        {
            _state = state;
        }

        public void SetPhase(BoutPhase phase)
        {
            _state.Phase = phase;
        }

        public Chair AddSkaterToBox(string team = "left", bool isJammer = false,
            int number = -1, int chairNumber = 1, int secondsOwed = 30, int secondsServed = 0)
        {
            var chair = new Chair
            {
                Id = Guid.NewGuid(),
                SecondsOwed = secondsOwed,
                Team = team,
                Number = number,
                ChairNumber = chairNumber,
                IsJammer = isJammer,
            };
            chair.StopWatch.Nudge(TimeSpan.FromSeconds(secondsServed));
            _state.PenaltyBox.Add(chair);
            return chair;
        }

        public JamParticipant AddSkaterToJam(string team = "left", int jamIndex = 0,
            int number = 8, Position position = Position.Blocker)
        {
            var participant = new JamParticipant
            {
                Number = number,
                Position = position
            };
            if (team == "left")
            { _state.Jams[jamIndex].Left.Roster.Add(participant); }
            else
            { _state.Jams[jamIndex].Right.Roster.Add(participant); }
            return participant;
        }

        public Penalty AddPenalty(string team = "left", int period = 1, int jam = 1, TimeSpan? timeStamp = null, int secondsOwed = 30, int number = -1)
        {
            var penalty = new Penalty(team, period, jam, timeStamp ?? TimeSpan.FromSeconds(0), secondsOwed, number);
            _state.Penalties.Add(penalty);
            return penalty;
        }

    }
}
