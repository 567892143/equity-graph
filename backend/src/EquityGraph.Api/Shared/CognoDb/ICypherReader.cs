namespace EquityGraph.Api.Shared.CognoDb;

using Neo4j.Driver;

/// <summary>Provides read-only Cypher query execution against the graph database.</summary>
public interface ICypherReader
{
    /// <summary>Executes a Cypher read query and maps the resulting records.</summary>
    Task<List<T>> ReadAsync<T>(string cypher, object? parameters, Func<IRecord, T> map);
}
