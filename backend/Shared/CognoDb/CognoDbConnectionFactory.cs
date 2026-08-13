namespace EquityGraph.Api.Shared.CognoDb;

using Microsoft.Extensions.Options;
using Neo4j.Driver;

public class CognoDbConnectionFactory : IAsyncDisposable, IDisposable
{
    private readonly IDriver _driver;

    public CognoDbConnectionFactory(IOptions<CognoDbOptions> options)
        : this(options.Value)
    {
    }

    public CognoDbConnectionFactory(CognoDbOptions options)
    {
        _driver = GraphDatabase.Driver(
            options.Uri,
            AuthTokens.Basic(options.Username, options.Password)
        );
    }

    public IAsyncSession CreateSession()
    {
        return _driver.AsyncSession();
    }

    public void Dispose()
    {
        _driver.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _driver.DisposeAsync();
    }
}
