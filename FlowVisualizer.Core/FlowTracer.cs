using System.Diagnostics;

namespace FlowVisualizer.Core;

public class FlowTracer(IFlowEventSink sink)
{
    public async Task<T> TraceAsync<T>(
        string sourceClass, string sourceMethod,
        string targetClass, string targetMethod,
        string layerName,
        Func<Task<T>> action,
        object? input = null, string? payloadType = null)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var result = await action();
            sw.Stop();
            await sink.EmitAsync(new FlowEvent
            {
                SourceClass = sourceClass,
                SourceMethod = sourceMethod,
                TargetClass = targetClass,
                TargetMethod = targetMethod,
                LayerName = layerName,
                DurationMs = sw.Elapsed.TotalMilliseconds,
                CorrelationId = FlowCorrelation.Current,
                InputPayload = FlowEvent.Summarize(input),
                OutputPayload = FlowEvent.Summarize(result),
                PayloadType = payloadType
            });
            return result;
        }
        catch (Exception ex)
        {
            sw.Stop();
            await sink.EmitAsync(new FlowEvent
            {
                SourceClass = sourceClass,
                SourceMethod = sourceMethod,
                TargetClass = targetClass,
                TargetMethod = targetMethod,
                LayerName = layerName,
                DurationMs = sw.Elapsed.TotalMilliseconds,
                IsError = true,
                ErrorMessage = ex.Message,
                CorrelationId = FlowCorrelation.Current,
                InputPayload = FlowEvent.Summarize(input),
                PayloadType = payloadType
            });
            throw;
        }
    }

    public async Task TraceAsync(
        string sourceClass, string sourceMethod,
        string targetClass, string targetMethod,
        string layerName,
        Func<Task> action,
        object? input = null, string? payloadType = null)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            await action();
            sw.Stop();
            await sink.EmitAsync(new FlowEvent
            {
                SourceClass = sourceClass,
                SourceMethod = sourceMethod,
                TargetClass = targetClass,
                TargetMethod = targetMethod,
                LayerName = layerName,
                DurationMs = sw.Elapsed.TotalMilliseconds,
                CorrelationId = FlowCorrelation.Current,
                InputPayload = FlowEvent.Summarize(input),
                PayloadType = payloadType
            });
        }
        catch (Exception ex)
        {
            sw.Stop();
            await sink.EmitAsync(new FlowEvent
            {
                SourceClass = sourceClass,
                SourceMethod = sourceMethod,
                TargetClass = targetClass,
                TargetMethod = targetMethod,
                LayerName = layerName,
                DurationMs = sw.Elapsed.TotalMilliseconds,
                IsError = true,
                ErrorMessage = ex.Message,
                CorrelationId = FlowCorrelation.Current,
                InputPayload = FlowEvent.Summarize(input),
                PayloadType = payloadType
            });
            throw;
        }
    }

    public T TraceSync<T>(
        string sourceClass, string sourceMethod,
        string targetClass, string targetMethod,
        string layerName,
        Func<T> action,
        object? input = null, string? payloadType = null)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var result = action();
            sw.Stop();
            sink.EmitAsync(new FlowEvent
            {
                SourceClass = sourceClass,
                SourceMethod = sourceMethod,
                TargetClass = targetClass,
                TargetMethod = targetMethod,
                LayerName = layerName,
                DurationMs = sw.Elapsed.TotalMilliseconds,
                CorrelationId = FlowCorrelation.Current,
                InputPayload = FlowEvent.Summarize(input),
                OutputPayload = FlowEvent.Summarize(result),
                PayloadType = payloadType
            }).GetAwaiter().GetResult();
            return result;
        }
        catch (Exception ex)
        {
            sw.Stop();
            sink.EmitAsync(new FlowEvent
            {
                SourceClass = sourceClass,
                SourceMethod = sourceMethod,
                TargetClass = targetClass,
                TargetMethod = targetMethod,
                LayerName = layerName,
                DurationMs = sw.Elapsed.TotalMilliseconds,
                IsError = true,
                ErrorMessage = ex.Message,
                CorrelationId = FlowCorrelation.Current,
                InputPayload = FlowEvent.Summarize(input),
                PayloadType = payloadType
            }).GetAwaiter().GetResult();
            throw;
        }
    }
}
