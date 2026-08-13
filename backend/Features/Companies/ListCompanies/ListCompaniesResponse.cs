namespace EquityGraph.Api.Features.Companies.ListCompanies;

public record CompanySummary(
    string Id,
    string Name,
    string Ticker,
    string Sector,
    double MarketCap
);
