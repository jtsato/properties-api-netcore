using Core.Commons;

namespace Core.Domains.Properties.Models;

public sealed class PropertyStatus : Enumeration<PropertyStatus>
{
    public static readonly PropertyStatus All = new PropertyStatus(0, "ALL");
    public static readonly PropertyStatus Active = new PropertyStatus(1, "ACTIVE");
    public static readonly PropertyStatus Inactive = new PropertyStatus(2, "INACTIVE");

    private PropertyStatus(int id, string name) : base(id, name)
    {
    }
}