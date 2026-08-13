namespace EquityGraph.Api.Features.Companies.GetBoardInterlocks;

public record BoardInterlock(
    string PersonId,
    string PersonName,
    int Since,
    string OtherCompanyId,
    string OtherCompanyName
);
