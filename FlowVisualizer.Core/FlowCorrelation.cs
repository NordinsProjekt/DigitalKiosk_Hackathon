namespace FlowVisualizer.Core;

public static class FlowCorrelation
{
    private static readonly AsyncLocal<Guid> _correlationId = new();

    public static Guid Current
    {
        get
        {
            if (_correlationId.Value == Guid.Empty)
                _correlationId.Value = Guid.NewGuid();
            return _correlationId.Value;
        }
        set => _correlationId.Value = value;
    }
}
