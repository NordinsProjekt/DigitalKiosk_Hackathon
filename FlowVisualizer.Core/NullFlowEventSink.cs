namespace FlowVisualizer.Core;

public class NullFlowEventSink : IFlowEventSink
{
    public Task EmitAsync(FlowEvent flowEvent) => Task.CompletedTask;
}
