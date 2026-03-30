namespace FlowVisualizer.Core;

public class FlowEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public required string SourceClass { get; set; }
    public required string SourceMethod { get; set; }
    public required string TargetClass { get; set; }
    public required string TargetMethod { get; set; }
    public double DurationMs { get; set; }
    public required string LayerName { get; set; }
    public bool IsError { get; set; }
    public string? ErrorMessage { get; set; }
    public Guid CorrelationId { get; set; }
}
