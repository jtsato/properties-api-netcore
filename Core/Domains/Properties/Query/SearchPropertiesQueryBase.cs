namespace Core.Domains.Properties.Query;

public class SearchPropertiesQueryBase
{
    public string Type { get; init; }
    public string Status { get; init; }

    protected SearchPropertiesQueryBase(string type, string status)
    {
        Type = type;
        Status = status;
    }
}