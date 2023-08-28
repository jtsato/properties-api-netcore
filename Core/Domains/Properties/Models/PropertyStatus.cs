using Core.Commons;

namespace Core.Domains.Properties.Models;


public sealed class PropertyStatus : Enumeration<PropertyStatus>
{
    public static readonly PropertyStatus None = new PropertyStatus(0, nameof(None));
    public static readonly PropertyStatus Active = new PropertyStatus(1, nameof(Active));
    public static readonly PropertyStatus Inactive = new PropertyStatus(2, nameof(Inactive));
    
    private PropertyStatus(int id, string name) : base(id, name)
    {
    }
}