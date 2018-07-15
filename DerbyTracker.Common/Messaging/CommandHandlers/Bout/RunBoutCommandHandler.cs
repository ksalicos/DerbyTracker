using DerbyTracker.Common.Exceptions;
using DerbyTracker.Common.Messaging.Commands.Bout;
using DerbyTracker.Common.Messaging.Events.Bout;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;

namespace DerbyTracker.Common.Messaging.CommandHandlers.Bout
{
    [Handles("RunBoutCommand")]
    public class RunBoutCommandHandler : CommandHandlerBase<RunBoutCommand>
    {
        private readonly IBoutDataService _boutDataService;
        private readonly IBoutRunnerService _boutRunnerService;

        public RunBoutCommandHandler(IBoutRunnerService boutRunnerService, IBoutDataService boutDataService)
        {
            _boutRunnerService = boutRunnerService;
            _boutDataService = boutDataService;
        }

        public override ICommandResponse Handle(RunBoutCommand command)
        {
            var response = new CommandResponse();
            var bout = _boutDataService.Load(command.BoutId);
            if (_boutRunnerService.IsRunning(bout.BoutId))
            { throw new BoutAlreadyRunningException(bout.BoutId); }

            _boutRunnerService.StartBout(bout);
            response.AddEvent(new BoutRunningEvent(bout.BoutId, bout.Name), Audiences.All);

            return response;
        }
    }
}
