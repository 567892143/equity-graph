namespace EquityGraph.Api.Shared.CognoDb;

using Microsoft.Extensions.Options;
using Neo4j.Driver;

/// <summary>Factory for managing the singleton Neo4j Bolt driver and creating database sessions.</summary>
public class CognoDbConnectionFactory : IAsyncDisposable, IDisposable
{
    private readonly IDriver _driver;

    /// <summary>Initializes a new instance of CognoDbConnectionFactory with injected options.</summary>
    public CognoDbConnectionFactory(IOptions<CognoDbOptions> options)
        : this(options.Value)
    {
    }

    /// <summary>Initializes a new instance of CognoDbConnectionFactory with raw options.</summary>
    public CognoDbConnectionFactory(CognoDbOptions options)
    {
        _driver = GraphDatabase.Driver(
            options.Uri,
            AuthTokens.Basic(options.Username, options.Password)
        );
    }

    /// <summary>Creates and returns a new asynchronous Neo4j driver session.</summary>
    public IAsyncSession CreateSession()
    {
        return _driver.AsyncSession();
    }

    /// <summary>Synchronously disposes the underlying Neo4j driver.</summary>
    public void Dispose()
    {
        _driver.Dispose();
    }

    /// <summary>Asynchronously disposes the underlying Neo4j driver.</summary>
    public async ValueTask DisposeAsync()
    {
        await _driver.DisposeAsync();
    }
}
