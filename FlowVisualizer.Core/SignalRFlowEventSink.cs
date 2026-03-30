using System.Collections.Concurrent;
using FlowVisualizer.Core.Hub;
using Microsoft.AspNetCore.SignalR;

namespace FlowVisualizer.Core;

public class SignalRFlowEventSink(IHubContext<FlowHub> hubContext) : IFlowEventSink
{
    private readonly ConcurrentQueue<FlowEvent> _buffer = new();
    private const int MaxBufferSize = 1000;

    public IReadOnlyCollection<FlowEvent> RecentEvents => _buffer.ToArray();

    public async Task EmitAsync(FlowEvent flowEvent)
    {
        _buffer.Enqueue(flowEvent);
        while (_buffer.Count > MaxBufferSize)
            _buffer.TryDequeue(out _);

        await hubContext.Clients.Group("dashboard").SendAsync("ReceiveFlowEvent", flowEvent);
    }
}
