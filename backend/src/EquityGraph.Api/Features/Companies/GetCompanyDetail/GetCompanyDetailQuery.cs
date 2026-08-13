namespace EquityGraph.Api.Features.Companies.GetCompanyDetail;

/// <summary>Query parameters for fetching company details by company identifier.</summary>
public record GetCompanyDetailQuery(
    string CompanyId
);
