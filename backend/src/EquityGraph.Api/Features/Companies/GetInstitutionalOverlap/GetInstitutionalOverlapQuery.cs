namespace EquityGraph.Api.Features.Companies.GetInstitutionalOverlap;

/// <summary>Query parameters for calculating overlapping institutional ownership between two companies.</summary>
public record GetInstitutionalOverlapQuery(
    string CompanyIdA,
    string CompanyIdB
);
