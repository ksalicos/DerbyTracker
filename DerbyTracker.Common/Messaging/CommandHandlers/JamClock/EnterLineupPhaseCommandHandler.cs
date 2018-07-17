﻿using DerbyTracker.Common.Entities;
using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.Commands.JamClock;
using DerbyTracker.Common.Messaging.Events.Bout;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;

namespace DerbyTracker.Common.Messaging.CommandHandlers.JamClock
{
    [Handles("EnterLineupPhaseCommand")]
    public class EnterLineupPhaseCommandHandler : CommandHandlerBase<EnterLineupPhaseCommand>
    {
        private readonly IBoutDataService _boutDataService;
        private readonly IBoutRunnerService _boutRunnerService;

        public EnterLineupPhaseCommandHandler(IBoutRunnerService boutRunnerService, IBoutDataService boutDataService)
        {
            _boutRunnerService = boutRunnerService;
            _boutDataService = boutDataService;
        }

        public override ICommandResponse Handle(EnterLineupPhaseCommand command)
        {
            var response = new CommandResponse();

            //Bout must be running
            var bout = _boutDataService.Load(command.BoutId);
            if (!_boutRunnerService.IsRunning(bout.BoutId))
            { throw new BoutNotRunningException(bout.BoutId); }

            //Game Must Be in pregame
            var state = _boutRunnerService.GetBoutState(command.BoutId);
            if (state.Phase != BoutPhase.Pregame && state.Phase != BoutPhase.Halftime)
            { return response; }

            state.Phase = BoutPhase.Lineup;
            response.AddEvent(new BoutPhaseChangedEvent(BoutPhase.Lineup, command.BoutId), Audiences.All);

            return response;
        }
    }
}