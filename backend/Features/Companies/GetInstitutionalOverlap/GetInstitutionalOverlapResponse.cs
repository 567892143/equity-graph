namespace EquityGraph.Api.Features.Companies.GetInstitutionalOverlap;

public record InstitutionalOverlapEntry(
    string InstitutionId,
    string InstitutionName,
    double StakeInCompanyA,
    double StakeInCompanyB
);
