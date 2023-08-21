namespace Core.Domains.Properties.Query;

public class SearchPropertiesQueryBase
{
    public int TenantId { get; init; }
    public string Type { get; init; }
    public string Status { get; init; }

    protected SearchPropertiesQueryBase(int tenantId, string type, string status)
    {
        TenantId = tenantId;
        Type = type?.Trim().ToUpperInvariant();
        Status = status?.Trim().ToUpperInvariant();
    }
}