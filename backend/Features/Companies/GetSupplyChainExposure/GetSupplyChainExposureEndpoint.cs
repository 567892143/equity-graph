namespace EquityGraph.Api.Features.Companies.GetSupplyChainExposure;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

public static class GetSupplyChainExposureEndpoint
{
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
