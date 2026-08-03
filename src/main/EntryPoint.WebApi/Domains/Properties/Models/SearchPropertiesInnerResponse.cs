using Swashbuckle.AspNetCore.Annotations;

namespace EntryPoint.WebApi.Domains.Properties.Models;

public sealed class SearchPropertiesInnerResponse
{
    [SwaggerSchema(Nullable = false, Description = "Sequential identifier of the property.")]
    public long Id { get; init; }

    [SwaggerSchema(Nullable = false, Description = "UUID of the property.")]
    public string Uuid { get; init; }

    [SwaggerSchema(Description = "Transaction type of the property.")]
    public string Transaction { get; init; }

    [SwaggerSchema(Description = "Type of the property.")]
    public string Type { get; init; }

    [SwaggerSchema(Description = "Cover image of the property.")]
    public string CoverImage { get; init; }

    [SwaggerSchema(Description = "Number of bedrooms in the property.")]
    public byte NumberOfBedrooms { get; init; }

    [SwaggerSchema(Description = "Number of garages in the property.")]
    public byte NumberOfGarages { get; init; }

    [SwaggerSchema(Description = "Total area of the property.")]
    public double Area { get; init; }

    [SwaggerSchema(Description = "Built area of the property.")]
    public double BuiltArea { get; init; }

    [SwaggerSchema(Description = "City where the property is located.")]
    public string City { get; init; }

    [SwaggerSchema(Description = "State where the property is located.")]
    public string State { get; init; }

    [SwaggerSchema(Description = "District where the property is located.")]
    public string District { get; init; }

    [SwaggerSchema(Description = "Selling price of the property.")]
    public double SellingPrice { get; init; }

    [SwaggerSchema(Description = "Total rental price of the property.")]
    public double RentalTotalPrice { get; init; }

    [SwaggerSchema(Description = "Ranking of the property.")]
    public byte Ranking { get; init; }
}
