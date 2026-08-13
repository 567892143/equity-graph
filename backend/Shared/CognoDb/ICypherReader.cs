namespace EquityGraph.Api.Shared.CognoDb;

using Neo4j.Driver;

public interface ICypherReader
{
    Task<List<T>> ReadAsync<T>(string cypher, object? parameters, Func<IRecord, T> map);
}
