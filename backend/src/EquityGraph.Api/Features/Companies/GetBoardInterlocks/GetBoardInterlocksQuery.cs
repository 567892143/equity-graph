namespace EquityGraph.Api.Features.Companies.GetBoardInterlocks;

/// <summary>Query parameters for fetching board interlocks for a target company.</summary>
public record GetBoardInterlocksQuery(
    string CompanyId
);
