using System.Diagnostics;

namespace FlowVisualizer.Core;

public class FlowTracer(IFlowEventSink sink)
{
    public async Task<T> TraceAsync<T>(
        string sourceClass, string sourceMethod,
        string targetClass, string targetMethod,
        string layerName,
        Func<Task<T>> action)
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
                CorrelationId = FlowCorrelation.Current
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
                CorrelationId = FlowCorrelation.Current
            });
            throw;
        }
    }

    public async Task TraceAsync(
        string sourceClass, string sourceMethod,
        string targetClass, string targetMethod,
        string layerName,
        Func<Task> action)
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
                CorrelationId = FlowCorrelation.Current
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
                CorrelationId = FlowCorrelation.Current
            });
            throw;
        }
    }

    public T TraceSync<T>(
        string sourceClass, string sourceMethod,
        string targetClass, string targetMethod,
        string layerName,
        Func<T> action)
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
                CorrelationId = FlowCorrelation.Current
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
                CorrelationId = FlowCorrelation.Current
            }).GetAwaiter().GetResult();
            throw;
        }
    }
}
