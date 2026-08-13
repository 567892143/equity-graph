namespace EquityGraph.Api.Features.Companies.GetShortestPath;

public record GetShortestPathQuery(
    string FromCompanyId,
    string ToCompanyId
);
