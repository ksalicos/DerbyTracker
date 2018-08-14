using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.Commands.PenaltyBoxTimer;
using DerbyTracker.Common.Messaging.Events.PenaltyBoxTimer;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;
using System;
using System.Linq;

namespace DerbyTracker.Common.Messaging.CommandHandlers.PenaltyBoxTimer
{
    [Handles("ButtHitSeatCommand")]
    public class ButtHitSeatCommandHandler : CommandHandlerBase<ButtHitSeatCommand>
    {
        private readonly IBoutRunnerService _boutRunner;

        public ButtHitSeatCommandHandler(IBoutRunnerService boutRunner)
        {
            _boutRunner = boutRunner;
        }

        public override ICommandResponse Handle(ButtHitSeatCommand command)
        {
            var response = new CommandResponse();
            var state = _boutRunner.GetBoutState(command.BoutId);

            if (state.PenaltyBox.Any(x => x.Id == command.Chair.Id))
            {
                throw new ButtAlreadyInChairException(command.Chair.Id);
            }

            if (state.Phase == Entities.BoutPhase.Jam)
            {
                command.Chair.StopWatch.Start();
            }

            if (command.Chair.IsJammer)
            {
                var jammer = state.GetCurrentJammer(command.Chair.Team);
                if (jammer.HasValue)
                {
                    command.Chair.Number = jammer.Value;

                    command.Chair.SecondsOwed = state.Penalties.Any(x => x.Number == command.Chair.Number)
                        ? state.Penalties.Where(x => x.Number == command.Chair.Number).Sum(x => x.SecondsOwed)
                        : command.Chair.SecondsOwed;
                }

                var otherTeam = command.Chair.Team == "left" ? "right" : "left";
                var otherJammerInBox = state.PenaltyBox.SingleOrDefault(x => x.Team == otherTeam && x.IsJammer);
                if (otherJammerInBox != null)
                {
                    var timeRemaining = Math.Max(otherJammerInBox.SecondsOwed - (int)Math.Round(otherJammerInBox.StopWatch.Elapsed.TotalSeconds), 0);
                    if (timeRemaining < command.Chair.SecondsOwed)
                    {
                        otherJammerInBox.SecondsOwed -= timeRemaining;
                        command.Chair.SecondsOwed -= timeRemaining;
                    }
                    else
                    {
                        otherJammerInBox.SecondsOwed -= command.Chair.SecondsOwed;
                        command.Chair.SecondsOwed = 0;
                    }
                    response.AddEvent(new ChairUpdatedEvent(command.BoutId, otherJammerInBox), Audiences.Bout(command.BoutId));
                }
            }

            state.PenaltyBox.Add(command.Chair);
            response.AddEvent(new ChairUpdatedEvent(command.BoutId, command.Chair), Audiences.Bout(command.BoutId));
            return response;
        }
    }
}
