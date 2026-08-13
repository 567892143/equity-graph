namespace EquityGraph.Api.Features.Companies.GetCompanyDetail;

/// <summary>Detailed response containing company profile and aggregated graph metrics.</summary>
public record CompanyDetailResponse(
    string Id,
    string Name,
    string Ticker,
    string Sector,
    double MarketCap,
    int DirectorCount,
    double MaxSupplyDependencyPct,
    int InstitutionCount
);
