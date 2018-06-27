using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DerbyTracker.Master
{
    public class WheelHub : Hub
    {
        public async Task Test()
        {
            await Clients.All.SendAsync("TestAck", "Data");
        }
    }
}
