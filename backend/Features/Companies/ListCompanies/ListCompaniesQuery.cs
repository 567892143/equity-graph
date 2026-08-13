namespace EquityGraph.Api.Features.Companies.ListCompanies;

public record ListCompaniesQuery(
    string? Search,
    string? Sector
);
