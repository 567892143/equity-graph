namespace EquityGraph.Api.Features.Companies.GetCompanyDetail;

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
