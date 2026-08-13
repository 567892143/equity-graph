namespace EquityGraph.Api.Shared.CognoDb;

public class CognoDbOptions
{
    public const string SectionName = "CognoDb";

    public string Uri { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
