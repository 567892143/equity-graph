namespace EquityGraph.Api.Features.Companies.GetBoardInterlocks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

public static class GetBoardInterlocksEndpoint
{
    public static IEndpointRouteBuilder MapGetBoardInterlocksEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/companies/{companyId}/board-interlocks", async (
            string companyId,
            GetBoardInterlocksQueryHandler handler) =>
        {
            var results = await handler.HandleAsync(new GetBoardInterlocksQuery(companyId));
            return Results.Ok(results);
        })
        .WithName("GetBoardInterlocks")
        .WithTags("Companies")
        .WithOpenApi();

        return app;
    }
}
