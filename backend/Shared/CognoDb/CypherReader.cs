namespace EquityGraph.Api.Shared.CognoDb;

using Neo4j.Driver;

public class CypherReader : ICypherReader
{
    private readonly CognoDbConnectionFactory _connectionFactory;

    public CypherReader(CognoDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

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
