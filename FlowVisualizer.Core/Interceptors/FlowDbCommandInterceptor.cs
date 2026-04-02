using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace FlowVisualizer.Core.Interceptors;

public class FlowDbCommandInterceptor(IFlowEventSink sink) : DbCommandInterceptor
{
    public override ValueTask<DbDataReader> ReaderExecutedAsync(
        DbCommand command, CommandExecutedEventData eventData, DbDataReader result,
        CancellationToken cancellationToken = default)
    {
        EmitEvent(command, eventData);
        return new ValueTask<DbDataReader>(result);
    }

    public override ValueTask<int> NonQueryExecutedAsync(
        DbCommand command, CommandExecutedEventData eventData, int result,
        CancellationToken cancellationToken = default)
    {
        EmitEvent(command, eventData);
        return new ValueTask<int>(result);
    }

    public override ValueTask<object?> ScalarExecutedAsync(
        DbCommand command, CommandExecutedEventData eventData, object? result,
        CancellationToken cancellationToken = default)
    {
        EmitEvent(command, eventData);
        return new ValueTask<object?>(result);
    }

    public override void CommandFailed(DbCommand command, CommandErrorEventData eventData)
    {
        EmitErrorEvent(command, eventData);
    }

    public override Task CommandFailedAsync(
        DbCommand command, CommandErrorEventData eventData,
        CancellationToken cancellationToken = default)
    {
        EmitErrorEvent(command, eventData);
        return Task.CompletedTask;
    }

    private void EmitEvent(DbCommand command, CommandExecutedEventData eventData)
    {
        var commandType = GetCommandType(command.CommandText);
        _ = sink.EmitAsync(new FlowEvent
        {
            SourceClass = "Repository",
            SourceMethod = "EF Core",
            TargetClass = "SqlServer",
            TargetMethod = commandType,
            LayerName = "Database",
            DurationMs = eventData.Duration.TotalMilliseconds,
            CorrelationId = FlowCorrelation.Current,
            InputPayload = TruncateSql(command.CommandText),
            OutputPayload = SummarizeParameters(command),
            PayloadType = "SQL"
        });
    }

    private void EmitErrorEvent(DbCommand command, CommandErrorEventData eventData)
    {
        var commandType = GetCommandType(command.CommandText);
        _ = sink.EmitAsync(new FlowEvent
        {
            SourceClass = "Repository",
            SourceMethod = "EF Core",
            TargetClass = "SqlServer",
            TargetMethod = commandType,
            LayerName = "Database",
            DurationMs = eventData.Duration.TotalMilliseconds,
            IsError = true,
            ErrorMessage = eventData.Exception?.Message,
            CorrelationId = FlowCorrelation.Current,
            InputPayload = TruncateSql(command.CommandText),
            OutputPayload = SummarizeParameters(command),
            PayloadType = "SQL"
        });
    }

    private static string? TruncateSql(string? sql)
    {
        if (sql is null) return null;
        return sql.Length > 2000 ? sql[..2000] + "\u2026" : sql;
    }

    private static string? SummarizeParameters(DbCommand command)
    {
        if (command.Parameters.Count == 0) return null;
        var dict = new Dictionary<string, string?>();
        foreach (DbParameter p in command.Parameters)
            dict[p.ParameterName] = p.Value?.ToString();
        return FlowEvent.Summarize(dict);
    }

    private static string GetCommandType(string sql)
    {
        var trimmed = sql.TrimStart();
        if (trimmed.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase)) return "SELECT";
        if (trimmed.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase)) return "INSERT";
        if (trimmed.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase)) return "UPDATE";
        if (trimmed.StartsWith("DELETE", StringComparison.OrdinalIgnoreCase)) return "DELETE";
        return "QUERY";
    }
}
