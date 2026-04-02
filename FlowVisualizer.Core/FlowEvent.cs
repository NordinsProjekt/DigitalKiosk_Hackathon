using System.Text.Json;
using System.Text.Json.Serialization;

namespace FlowVisualizer.Core;

public class FlowEvent
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        MaxDepth = 3,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = false
    };

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
    public string? InputPayload { get; set; }
    public string? OutputPayload { get; set; }
    public string? PayloadType { get; set; }

    public static string? Summarize(object? obj, int maxLength = 2000)
    {
        if (obj is null) return null;
        try
        {
            var json = JsonSerializer.Serialize(obj, SerializerOptions);
            return json.Length > maxLength ? json[..maxLength] + "…" : json;
        }
        catch
        {
            return obj.ToString();
        }
    }
}
