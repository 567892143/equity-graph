namespace EquityGraph.Api.Shared.Models;

/// <summary>Represents a corporate entity node in the equity graph.</summary>
public record Company(
    string Id,
    string Name,
    string Ticker,
    string Sector,
    double MarketCap
);
