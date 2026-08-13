namespace EquityGraph.Api.Shared.CognoDb;

using Neo4j.Driver;

/// <summary>Executes read-only Cypher queries and maps records to strongly-typed models.</summary>
public class CypherReader : ICypherReader
{
    private readonly CognoDbConnectionFactory _connectionFactory;

    /// <summary>Initializes a new instance of CypherReader with the connection factory.</summary>
    public CypherReader(CognoDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>Executes a Cypher read query and maps the resulting records.</summary>
    public async Task<List<T>> ReadAsync<T>(string cypher, object? parameters, Func<IRecord, T> map)
    {
        await using var session = _connectionFactory.CreateSession();
        var cursor = await session.RunAsync(cypher, parameters);
        var results = new List<T>();

        while (await cursor.FetchAsync())
        {
            results.Add(map(cursor.Current));
        }

        return results;
    }
}
