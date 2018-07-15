using DerbyTracker.Common.Messaging.CommandHandlers.Bout;
using DerbyTracker.Common.Messaging.CommandHandlers.Node;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Dispatchers;

namespace DerbyTracker.Common.Messaging.CommandHandlers
{
    //TODO: This will get unweildy, and should be reworked to use DI/Reflection
    public class HandlerRegistrar
    {
        private readonly INodeService _nodeService;
        private readonly IBoutDataService _boutDataService;
        private readonly IBoutRunnerService _boutRunnerService;

        public HandlerRegistrar(INodeService nodeService, IBoutDataService boutDataService, IBoutRunnerService boutRunnerService)
        {
            _nodeService = nodeService;
            _boutDataService = boutDataService;
            _boutRunnerService = boutRunnerService;
        }

        public void RegisterHandlers(ImmediateDispatcher dispatcher)
        {
            dispatcher.RegisterHandler(new ConnectNodeCommandHandler(_nodeService));
            dispatcher.RegisterHandler(new RunBoutCommandHandler(_boutRunnerService, _boutDataService));
            dispatcher.RegisterHandler(new AssignRoleToNodeCommandHandler(_nodeService));
            dispatcher.RegisterHandler(new RemoveRoleFromNodeCommandHandler(_nodeService));
        }
    }
}
