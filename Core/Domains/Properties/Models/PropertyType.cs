using Core.Commons;

namespace Core.Domains.Properties.Models;


public class PropertyType : Enumeration<PropertyType>
{
    public static readonly PropertyType All = new PropertyType(0, "ALL");
    public static readonly PropertyType Apartment = new PropertyType(1, "APARTMENT");
    public static readonly PropertyType House = new PropertyType(2, "HOUSE");
    public static readonly PropertyType TwoStoreyHouse = new PropertyType(3, "TWO_STOREY_HOUSE");
    public static readonly PropertyType Office = new PropertyType(4, "OFFICE");
    public static readonly PropertyType Land = new PropertyType(5, "LAND");
    public static readonly PropertyType WareHouse = new PropertyType(6, "WAREHOUSE");
    public static readonly PropertyType Store = new PropertyType(7, "STORE");
    public static readonly PropertyType Other = new PropertyType(9, "OTHER");

    private PropertyType(int id, string name) : base(id, name)
    {
        // TODO: Capture the Store property type 
    }
}