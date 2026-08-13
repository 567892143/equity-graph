namespace EquityGraph.Api.Features.Companies.GetInstitutionalOverlap;

/// <summary>Represents an institutional investor with ownership stakes in two compared companies.</summary>
public record InstitutionalOverlapEntry(
    string InstitutionId,
    string InstitutionName,
    double StakeInCompanyA,
    double StakeInCompanyB
);
