namespace EquityGraph.Api.Features.Companies.GetShortestPath;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

public static class GetShortestPathEndpoint
{
    public static IEndpointRouteBuilder MapGetShortestPathEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/companies/shortest-path", async (
            [FromQuery] string fromCompanyId,
            [FromQuery] string toCompanyId,
            GetShortestPathQueryHandler handler) =>
        {
            var result = await handler.HandleAsync(new GetShortestPathQuery(fromCompanyId, toCompanyId));
            if (result is null)
            {
                return Results.NotFound(new { message = $"No connection found between '{fromCompanyId}' and '{toCompanyId}'." });
            }

            return Results.Ok(result);
        })
        .WithName("GetShortestPath")
        .WithTags("Companies")
        .WithOpenApi();

        return app;
    }
}
