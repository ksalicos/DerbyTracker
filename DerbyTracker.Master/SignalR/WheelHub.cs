using DerbyTracker.Common.Messaging.Commands.Node;
using DerbyTracker.Common.Services;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Dispatchers;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DerbyTracker.Master.SignalR
{
    public partial class WheelHub : Hub
    {
        private readonly IDispatcher _dispatcher;
        private readonly INodeService _nodeService;

        public WheelHub(IDispatcher dispatcher, INodeService nodeService)
        {
            _dispatcher = dispatcher;
            this._nodeService = nodeService;
        }

        private async Task Dispatch(ICommand command)
        {
            await _dispatcher.Dispatch(command);
        }

        private async Task Dispatch(string nodeId, string role, ICommand command)
        {
            if (_nodeService.ValidateNode(nodeId, role, Context.ConnectionId))
            { await _dispatcher.Dispatch(command); }
            else
            { await Clients.Caller.SendAsync("Error", "Error creating command"); }
        }

        public async Task Test()
        {
            await Clients.Caller.SendAsync("TestAck", "Data");
        }

        public async Task ConnectNode(string nodeId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Nodes");
            var command = new ConnectNodeCommand(nodeId, this.Context.ConnectionId);
            await Dispatch(command);
        }

        public async Task ConnectMaster()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Master");
        }
    }
}
