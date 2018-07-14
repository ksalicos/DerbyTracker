using DerbyTracker.Common.Messaging.Commands.Node;
using DerbyTracker.Messaging.Dispatchers;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DerbyTracker.Master.SignalR
{
    public class WheelHub : Hub
    {
        private readonly IDispatcher _dispatcher;

        public WheelHub(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task Test()
        {
            await Clients.All.SendAsync("TestAck", "Data");
        }

        public async Task ConnectNode(string nodeId)
        {
            var command = new ConnectNodeCommand(nodeId, this.Context.ConnectionId);
            await _dispatcher.Dispatch(command);
        }
    }
}
