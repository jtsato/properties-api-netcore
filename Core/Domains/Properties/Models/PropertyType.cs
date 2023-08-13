using System.Diagnostics.CodeAnalysis;
using Core.Commons;

namespace Core.Domains.Properties.Models;

[ExcludeFromCodeCoverage]
public class PropertyType : Enumeration<PropertyType>
{
    public static readonly PropertyType All = new PropertyType(0, nameof(All));
    public static readonly PropertyType Apartment = new PropertyType(1, nameof(Apartment));
    public static readonly PropertyType House = new PropertyType(2, nameof(House));
    public static readonly PropertyType TerracedHouse = new PropertyType(3, nameof(TerracedHouse));
    public static readonly PropertyType OfficeSpace = new PropertyType(4, nameof(OfficeSpace));
    public static readonly PropertyType Land = new PropertyType(5, nameof(Land));
    public static readonly PropertyType WareHouse = new PropertyType(6, nameof(WareHouse));

    private PropertyType(int id, string name) : base(id, name)
    {
    }
}