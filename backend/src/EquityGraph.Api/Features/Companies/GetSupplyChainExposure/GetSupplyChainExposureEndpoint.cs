namespace EquityGraph.Api.Features.Companies.GetSupplyChainExposure;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

/// <summary>Defines endpoint mappings for supply chain exposure analysis.</summary>
public static class GetSupplyChainExposureEndpoint
{
    /// <summary>Maps the GET /api/companies/{companyId}/supply-chain-exposure endpoint to the routing pipeline.</summary>
    public static IEndpointRouteBuilder MapGetSupplyChainExposureEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/companies/{companyId}/supply-chain-exposure", async (
            string companyId,
            [FromQuery] int? maxHops,
            GetSupplyChainExposureQueryHandler handler) =>
        {
            var hops = maxHops ?? 1;
            var results = await handler.HandleAsync(new GetSupplyChainExposureQuery(companyId, hops));
            return Results.Ok(results);
        })
        .WithName("GetSupplyChainExposure")
        .WithTags("Companies")
        .WithOpenApi();

        return app;
    }
}
