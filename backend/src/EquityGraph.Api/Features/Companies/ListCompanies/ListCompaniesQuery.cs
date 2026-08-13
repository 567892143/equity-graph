namespace EquityGraph.Api.Features.Companies.ListCompanies;

/// <summary>Query parameters for listing and filtering companies.</summary>
public record ListCompaniesQuery(
    string? Search,
    string? Sector
);
