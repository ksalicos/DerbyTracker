using DerbyTracker.Common.Messaging.CommandHandlers.Node;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Dispatchers;

namespace DerbyTracker.Common.Messaging.CommandHandlers
{
    //TODO: This will get unweildy, and should be reworked to use DI/Reflection
    public class HandlerRegistrar
    {
        private readonly INodeService _nodeService;
        public HandlerRegistrar(INodeService nodeService)
        {
            _nodeService = nodeService;
        }

        public void RegisterHandlers(ImmediateDispatcher dispatcher)
        {
            dispatcher.RegisterHandler(new ConnectNodeCommandHandler(_nodeService));
        }
    }
}
