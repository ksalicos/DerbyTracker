using DerbyTracker.Common.Messaging;
using DerbyTracker.Messaging.Callbacks;
using DerbyTracker.Messaging.Commands;
using Microsoft.AspNetCore.SignalR;

namespace DerbyTracker.Master.SignalR
{
    public class SignalRCallback : ICallback
    {
        private readonly IHubContext<WheelHub> _hubContext;
        public SignalRCallback(IHubContext<WheelHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void Callback(ICommandResponse response)
        {
            foreach (var e in response.Events)
            {
                IClientProxy target;
                switch (e.Audience)
                {
                    case Audiences.All:
                        target = _hubContext.Clients.All;
                        break;
                    case Audiences.Master:
                        target = _hubContext.Clients.Groups("Master");
                        break;
                    case Audiences.Nodes:
                        target = _hubContext.Clients.Groups("Nodes");
                        break;
                    default:
                        target = _hubContext.Clients.Client(e.Audience);
                        break;
                }
                target.SendAsync("dispatch", e.Event);
            }
        }
    }

    public class SignalRCallbackFactory : ICallbackFactory
    {
        private readonly IHubContext<WheelHub> _hubContext;
        public SignalRCallbackFactory(IHubContext<WheelHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public ICallback Get()
        {
            return new SignalRCallback(_hubContext);
        }
    }
}
