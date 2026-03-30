using Microsoft.AspNetCore.SignalR;

namespace FlowVisualizer.Core.Hub;

public class FlowHub : Microsoft.AspNetCore.SignalR.Hub
{
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "dashboard");
        await base.OnConnectedAsync();
    }
}
