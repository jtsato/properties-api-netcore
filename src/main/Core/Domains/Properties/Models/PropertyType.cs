using Core.Commons;

namespace Core.Domains.Properties.Models;

public class PropertyType : Enumeration<PropertyType>
{
    public static readonly PropertyType All = new PropertyType(0, "ALL"); // Todos
    public static readonly PropertyType Apartment = new PropertyType(1, "APARTMENT"); // Apartamento
    public static readonly PropertyType WareHouse = new PropertyType(2, "WAREHOUSE"); // Barracão
    public static readonly PropertyType House = new PropertyType(3, "HOUSE"); // Casa
    public static readonly PropertyType CountryHouse = new PropertyType(4, "COUNTRY_HOUSE"); // Chácara
    public static readonly PropertyType Farm = new PropertyType(5, "FARM"); // Fazenda
    public static readonly PropertyType Garage = new PropertyType(6, "GARAGE"); // Garagem
    public static readonly PropertyType LandDivision = new PropertyType(7, "LAND_DIVISION"); // Loteamento
    public static readonly PropertyType BusinessPremises = new PropertyType(8, "BUSINESS_PREMISES"); // Ponto Comercial
    public static readonly PropertyType Office = new PropertyType(9, "OFFICE"); // Sala Comercial
    public static readonly PropertyType TwoStoreyHouse = new PropertyType(10, "TWO_STOREY_HOUSE"); // Sobrado
    public static readonly PropertyType Land = new PropertyType(11, "LAND"); // Terreno
    public static readonly PropertyType Other = new PropertyType(12, "OTHER"); // Any other type of property

    private PropertyType(int id, string name) : base(id, name)
    {
    }
}