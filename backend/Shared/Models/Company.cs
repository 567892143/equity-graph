namespace EquityGraph.Api.Shared.Models;

public record Company(
    string Id,
    string Name,
    string Ticker,
    string Sector,
    double MarketCap
);
