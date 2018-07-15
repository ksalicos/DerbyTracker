using DerbyTracker.Common.Messaging.Commands.Node;
using DerbyTracker.Messaging.Dispatchers;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DerbyTracker.Master.SignalR
{
    public partial class WheelHub : Hub
    {
        private readonly IDispatcher _dispatcher;

        public WheelHub(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task Test()
        {
            await Clients.Caller.SendAsync("TestAck", "Data");
        }

        public async Task ConnectNode(string nodeId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Nodes");
            var command = new ConnectNodeCommand(nodeId, this.Context.ConnectionId);
            await _dispatcher.Dispatch(command);
        }

        public async Task ConnectMaster()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Master");
        }
    }
}
