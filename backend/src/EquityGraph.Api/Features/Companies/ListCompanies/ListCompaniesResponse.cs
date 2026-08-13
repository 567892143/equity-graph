namespace EquityGraph.Api.Features.Companies.ListCompanies;

/// <summary>Summary representation of a company in listing results.</summary>
public record CompanySummary(
    string Id,
    string Name,
    string Ticker,
    string Sector,
    double MarketCap
);
