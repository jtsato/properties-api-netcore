using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Swashbuckle.AspNetCore.Annotations;

namespace EntryPoint.WebApi.Domains.Properties.Models;

public sealed class SearchPropertiesInnerResponse
{
    [SwaggerSchema(Nullable = false, Description = "Unique key of the property.")]
    public string Id { get; init; }

    [SwaggerSchema(Description = "Identifier of the tenant.")]
    public int TenantId { get; init; }

    [SwaggerSchema(Description = "Transaction type of the property.")]
    public string Transaction { get; init; }

    [SwaggerSchema(Description = "Title of the property.")]
    public string Title { get; init; }

    [SwaggerSchema(Description = "Description of the property.")]
    public string Description { get; init; }

    [SwaggerSchema(Description = "URL of the property.")]
    public string Url { get; init; }

    [SwaggerSchema(Description = "Reference ID of the property.")]
    public string RefId { get; init; }

    [SwaggerSchema(Description = "List of images of the property.")]
    public List<string> Images { get; init; }

    [SwaggerSchema(Description = "Number of bedrooms in the property.")]
    public byte NumberOfBedrooms { get; init; }

    [SwaggerSchema(Description = "Number of toilets in the property.")]
    public byte NumberOfToilets { get; init; }

    [SwaggerSchema(Description = "Number of garages in the property.")]
    public byte NumberOfGarages { get; init; }

    [SwaggerSchema(Description = "Total area of the property.")]
    public decimal Area { get; init; }

    [SwaggerSchema(Description = "Built area of the property.")]
    public decimal BuiltArea { get; init; }

    [SwaggerSchema(Description = "City where the property is located.")]
    public string City { get; init; }

    [SwaggerSchema(Description = "State where the property is located.")]
    public string State { get; init; }
    
    [SwaggerSchema(Description = "District where the property is located.")]
    public string District { get; init; }

    [SwaggerSchema(Description = "Address of the property.")]
    public string Address { get; init; }

    [SwaggerSchema(Description = "Selling price of the property.")]
    public decimal SellingPrice { get; init; }

    [SwaggerSchema(Description = "Total rental price of the property.")]
    public decimal RentalTotalPrice { get; init; }

    [SwaggerSchema(Description = "Rental price of the property.")]
    public decimal RentalPrice { get; init; }

    [SwaggerSchema(Description = "Discount applied to the property.")]
    public decimal Discount { get; init; }

    [SwaggerSchema(Description = "Condominium fee of the property.")]
    public decimal CondominiumFee { get; init; }

    [SwaggerSchema(Description = "Price per square meter of the property.")]
    public decimal PriceByM2 { get; init; }
    
    [SwaggerSchema(Description = "Ranking of the property.")]
    public byte Ranking { get; init; }
    
    [SwaggerSchema(Description = "Status of the property.")]
    public string Status { get; init; }

    [SwaggerSchema(Description = "Date of property creation.")]
    public string CreatedAt { get; init; }

    [SwaggerSchema(Description = "Date of the last update to the property.")]
    public string UpdatedAt { get; init; }

    [SwaggerSchema(Nullable = false, Description = "URL that defines a single resource")]
    public string Href { get; init; }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"Id: {Id}")
            .AppendLine($"TenantId: {TenantId}")
            .AppendLine($"Transaction: {Transaction}")
            .AppendLine($"Title: {Title}")
            .AppendLine($"Description: {Description}")
            .AppendLine($"Url: {Url}")
            .AppendLine($"RefId: {RefId}")
            .AppendLine($"Images: {Images}")
            .AppendLine($"NumberOfBedrooms: {NumberOfBedrooms}")
            .AppendLine($"NumberOfToilets: {NumberOfToilets}")
            .AppendLine($"NumberOfGarages: {NumberOfGarages}")
            .AppendLine($"Area: {Area}")
            .AppendLine($"BuiltArea: {BuiltArea}")
            .AppendLine($"City: {City}")
            .AppendLine($"State: {State}")
            .AppendLine($"District: {District}")
            .AppendLine($"Address: {Address}")
            .AppendLine($"SellingPrice: {SellingPrice}")
            .AppendLine($"RentalTotalPrice: {RentalTotalPrice}")
            .AppendLine($"RentalPrice: {RentalPrice}")
            .AppendLine($"Discount: {Discount}")
            .AppendLine($"CondominiumFee: {CondominiumFee}")
            .AppendLine($"PriceByM2: {PriceByM2}")
            .AppendLine($"Ranking: {Ranking}")
            .AppendLine($"Status: {Status}")
            .AppendLine($"CreatedAt: {CreatedAt}")
            .AppendLine($"UpdatedAt: {UpdatedAt}")
            .AppendLine($"Href: {Href}")
            .ToString();
    }
}