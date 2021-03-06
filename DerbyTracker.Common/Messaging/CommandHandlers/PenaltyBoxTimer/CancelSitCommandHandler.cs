﻿using DerbyTracker.Common.Messaging.Commands.PenaltyBoxTimer;
using DerbyTracker.Common.Messaging.Events.PenaltyBoxTimer;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;
using System.Linq;

namespace DerbyTracker.Common.Messaging.CommandHandlers.PenaltyBoxTimer
{
    [Handles("CancelSitCommand")]
    public class CancelSitCommandHandler : CommandHandlerBase<CancelSitCommand>
    {
        private readonly IBoutRunnerService _boutRunner;

        public CancelSitCommandHandler(IBoutRunnerService boutRunner)
        {
            _boutRunner = boutRunner;
        }

        public override ICommandResponse Handle(CancelSitCommand command)
        {
            var state = _boutRunner.GetBoutState(command.BoutId);
            var sit = state.PenaltyBox.SingleOrDefault(x => x.Id == command.ChairId);
            var response = new CommandResponse();
            if (sit != null)
            {
                state.PenaltyBox.Remove(sit);
                response.AddEvent(new ChairRemovedEvent(command.BoutId, command.ChairId), Audiences.Bout(command.BoutId));
            }
            return response;
        }
    }
}
