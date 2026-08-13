namespace EquityGraph.Api.Features.Companies.ListCompanies;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

/// <summary>Defines endpoint mappings for listing companies.</summary>
public static class ListCompaniesEndpoint
{
    /// <summary>Maps the GET /api/companies endpoint to the routing pipeline.</summary>
    public static IEndpointRouteBuilder MapListCompaniesEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/companies", async (
            [FromQuery] string? search,
            [FromQuery] string? sector,
            ListCompaniesQueryHandler handler) =>
        {
            var query = new ListCompaniesQuery(search, sector);
            var results = await handler.HandleAsync(query);
            return Results.Ok(results);
        })
        .WithName("ListCompanies")
        .WithTags("Companies")
        .WithOpenApi();

        return app;
    }
}
