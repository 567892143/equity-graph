namespace EquityGraph.Api.Features.Companies.GetCompanyDetail;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

/// <summary>Defines endpoint mappings for fetching company details.</summary>
public static class GetCompanyDetailEndpoint
{
    /// <summary>Maps the GET /api/companies/{companyId} endpoint to the routing pipeline.</summary>
    public static IEndpointRouteBuilder MapGetCompanyDetailEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/companies/{companyId}", async (
            string companyId,
            GetCompanyDetailQueryHandler handler) =>
        {
            var result = await handler.HandleAsync(new GetCompanyDetailQuery(companyId));
            if (result is null)
            {
                return Results.NotFound(new { message = $"Company with ID '{companyId}' not found." });
            }

            return Results.Ok(result);
        })
        .WithName("GetCompanyDetail")
        .WithTags("Companies")
        .WithOpenApi();

        return app;
    }
}
