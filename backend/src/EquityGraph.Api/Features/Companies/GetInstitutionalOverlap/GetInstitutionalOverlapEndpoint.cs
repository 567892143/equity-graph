namespace EquityGraph.Api.Features.Companies.GetInstitutionalOverlap;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

/// <summary>Defines endpoint mappings for institutional ownership overlap.</summary>
public static class GetInstitutionalOverlapEndpoint
{
    /// <summary>Maps the GET /api/companies/overlap endpoint to the routing pipeline.</summary>
    public static IEndpointRouteBuilder MapGetInstitutionalOverlapEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/companies/overlap", async (
            [FromQuery] string companyIdA,
            [FromQuery] string companyIdB,
            GetInstitutionalOverlapQueryHandler handler) =>
        {
            var results = await handler.HandleAsync(new GetInstitutionalOverlapQuery(companyIdA, companyIdB));
            return Results.Ok(results);
        })
        .WithName("GetInstitutionalOverlap")
        .WithTags("Companies")
        .WithOpenApi();

        return app;
    }
}
