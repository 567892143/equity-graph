namespace EquityGraph.Api.Features.Health.CheckDbHealth;

using EquityGraph.Api.Shared.CognoDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Neo4j.Driver;

/// <summary>Defines endpoint mappings for database health checks.</summary>
public static class CheckDbHealthEndpoint
{
    /// <summary>Maps the GET /api/health/db endpoint to verify connectivity to CognoDB.</summary>
    public static IEndpointRouteBuilder MapCheckDbHealthEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/health/db", async (ICypherReader reader) =>
        {
            var results = await reader.ReadAsync(
                "RETURN 1 AS result",
                null,
                record => record["result"].As<int>()
            );

            var value = results.FirstOrDefault();
            return Results.Ok(new { status = "connected", result = value });
        })
        .WithName("CheckDbHealth")
        .WithTags("Health")
        .WithOpenApi();

        return app;
    }
}
