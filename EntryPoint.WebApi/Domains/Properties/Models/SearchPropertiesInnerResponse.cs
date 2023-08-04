using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Swashbuckle.AspNetCore.Annotations;

namespace EntryPoint.WebApi.Domains.Properties.Models
{
    [ExcludeFromCodeCoverage]
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
        public string Area { get; init; }

        [SwaggerSchema(Description = "Built area of the property.")]
        public string BuiltArea { get; init; }

        [SwaggerSchema(Description = "City where the property is located.")]
        public string City { get; init; }

        [SwaggerSchema(Description = "District where the property is located.")]
        public string District { get; init; }

        [SwaggerSchema(Description = "Address of the property.")]
        public string Address { get; init; }

        [SwaggerSchema(Description = "Selling price of the property.")]
        public string SellingPrice { get; init; }

        [SwaggerSchema(Description = "Total rental price of the property.")]
        public string RentalTotalPrice { get; init; }

        [SwaggerSchema(Description = "Rental price of the property.")]
        public string RentalPrice { get; init; }

        [SwaggerSchema(Description = "Discount applied to the property.")]
        public string Discount { get; init; }

        [SwaggerSchema(Description = "Condominium fee of the property.")]
        public string CondominiumFee { get; init; }

        [SwaggerSchema(Description = "Price per square meter of the property.")]
        public string PriceByM2 { get; init; }

        [SwaggerSchema(Description = "Date of property creation.")]
        public string CreatedAt { get; init; }

        [SwaggerSchema(Description = "Date of the last update to the property.")]
        public string UpdatedAt { get; init; }

        [SwaggerSchema(Nullable = false, Description = "URL that defines a single resource")]
        public string Href { get; init; }
        
        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(TenantId)}: {TenantId}, {nameof(Transaction)}: {Transaction}, {nameof(Title)}: {Title}, {nameof(Description)}: {Description}, {nameof(Url)}: {Url}, {nameof(RefId)}: {RefId}, {nameof(Images)}: {Images}, {nameof(NumberOfBedrooms)}: {NumberOfBedrooms}, {nameof(NumberOfToilets)}: {NumberOfToilets}, {nameof(NumberOfGarages)}: {NumberOfGarages}, {nameof(Area)}: {Area}, {nameof(BuiltArea)}: {BuiltArea}, {nameof(City)}: {City}, {nameof(District)}: {District}, {nameof(Address)}: {Address}, {nameof(SellingPrice)}: {SellingPrice}, {nameof(RentalTotalPrice)}: {RentalTotalPrice}, {nameof(RentalPrice)}: {RentalPrice}, {nameof(Discount)}: {Discount}, {nameof(CondominiumFee)}: {CondominiumFee}, {nameof(PriceByM2)}: {PriceByM2}, {nameof(CreatedAt)}: {CreatedAt}, {nameof(UpdatedAt)}: {UpdatedAt}, {nameof(Href)}: {Href}"; 
        }
    }
}
