namespace EquityGraph.Api.Features.Companies.GetBoardInterlocks;

/// <summary>Represents a board interlock relationship with a shared director.</summary>
public record BoardInterlock(
    string PersonId,
    string PersonName,
    int Since,
    string OtherCompanyId,
    string OtherCompanyName
);
