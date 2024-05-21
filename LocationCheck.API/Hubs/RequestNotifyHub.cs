using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace LocationCheck.API.Hubs
{
    [AllowAnonymous]
    public class RequestNotifyHub : Hub
    {
        public async Task BroadcastLog(string message)
        {            
            await Clients.All.SendAsync("broadcastLog", message);    
        }
    }
}
