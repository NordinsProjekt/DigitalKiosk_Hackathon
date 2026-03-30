namespace FlowVisualizer.Core;

public interface IFlowEventSink
{
    Task EmitAsync(FlowEvent flowEvent);
}
