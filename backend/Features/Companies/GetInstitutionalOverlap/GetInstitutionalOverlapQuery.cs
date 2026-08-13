namespace EquityGraph.Api.Features.Companies.GetInstitutionalOverlap;

public record GetInstitutionalOverlapQuery(
    string CompanyIdA,
    string CompanyIdB
);
