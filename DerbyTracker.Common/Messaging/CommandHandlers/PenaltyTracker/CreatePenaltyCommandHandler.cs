﻿using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Messaging.Commands.PenaltyTracker;
using DerbyTracker.Common.Messaging.Events.PenaltyTracker;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;

namespace DerbyTracker.Common.Messaging.CommandHandlers.PenaltyTracker
{
    [Handles("CreatePenaltyCommand")]
    public class CreatePenaltyCommandHandler : CommandHandlerBase<CreatePenaltyCommand>
    {
        private readonly IBoutRunnerService _boutRunner;

        public CreatePenaltyCommandHandler(IBoutRunnerService boutRunner)
        {
            _boutRunner = boutRunner;
        }

        public override ICommandResponse Handle(CreatePenaltyCommand command)
        {
            var state = _boutRunner.GetBoutState(command.BoutId);

            var penalty = new Penalty(command.Team, state.Period, state.JamNumber, state.GameClock.Elapsed);
            state.Penalties.Add(penalty);
            var response = new CommandResponse();
            response.AddEvent(new PenaltyUpdatedEvent(command.BoutId, penalty), Audiences.Bout(command.BoutId));
            return response;
        }
    }
}