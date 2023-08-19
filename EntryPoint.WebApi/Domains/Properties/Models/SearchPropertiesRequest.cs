using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EntryPoint.WebApi.Domains.Properties.Models;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class SearchPropertiesRequest
{
    [SwaggerParameter(Required = false, Description = "Type")]
    [FromQuery(Name = "type")]
    public string Type { get; init; }

    [SwaggerParameter(Required = true, Description = "Transaction type")]
    [FromQuery(Name = "transaction")]
    public string Transaction { get; init; }

    [SwaggerParameter(Required = false, Description = "Reference id")]
    [FromQuery(Name = "ref-id")]
    public string RefId { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum number of bedrooms")]
    [FromQuery(Name = "number-of-bedrooms-min")]
    public byte NumberOfBedroomsMin { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum number of bedrooms")]
    [FromQuery(Name = "number-of-bedrooms-max")]
    public byte NumberOfBedroomsMax { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum number of toilets")]
    [FromQuery(Name = "number-of-toilets-min")]
    public byte NumberOfToiletsMin { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum number of toilets")]
    [FromQuery(Name = "number-of-toilets-max")]
    public byte NumberOfToiletsMax { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum number of garages")]
    [FromQuery(Name = "number-of-garages-min")]
    public byte NumberOfGaragesMin { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum number of garages")]
    [FromQuery(Name = "number-of-garages-max")]
    public byte NumberOfGaragesMax { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum area in square meters")]
    [FromQuery(Name = "area-min")]
    public int AreaMin { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum area in square meters")]
    [FromQuery(Name = "area-max")]
    public int AreaMax { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum built area in square meters")]
    [FromQuery(Name = "built-area-min")]
    public int BuiltAreaMin { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum built area in square meters")]
    [FromQuery(Name = "built-area-max")]
    public int BuiltAreaMax { get; init; }
    
    [SwaggerParameter(Required = false, Description = "State")]
    [FromQuery(Name = "state")]
    public string State { get; init; }

    [SwaggerParameter(Required = false, Description = "City")]
    [FromQuery(Name = "city")]
    public string City { get; init; }

    [SwaggerParameter(Required = false, Description = "List of districts")]
    [FromQuery(Name = "districts")]
    public List<string> Districts { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum selling price")]
    [FromQuery(Name = "selling-price-min")]
    public decimal SellingPriceMin { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum selling price")]
    [FromQuery(Name = "selling-price-max")]
    public decimal SellingPriceMax { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum rental price")]
    [FromQuery(Name = "rental-price-min")]
    public decimal RentalPriceMin { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum rental price")]
    [FromQuery(Name = "rental-price-max")]
    public decimal RentalPriceMax { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum total rental price")]
    [FromQuery(Name = "rental-total-price-min")]
    public decimal RentalTotalPriceMin { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum total rental price")]
    [FromQuery(Name = "rental-total-price-max")]
    public decimal RentalTotalPriceMax { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum price per square meter")]
    [FromQuery(Name = "price-by-m2-min")]
    public decimal PriceByM2Min { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum price per square meter")]
    [FromQuery(Name = "price-by-m2-max")]
    public decimal PriceByM2Max { get; init; }
    
    [SwaggerParameter(Required = false, Description = "Minimum property's ranking")]
    [FromQuery(Name = "ranking-min")]
    public int RankingMin { get; init; }
    
    [SwaggerParameter(Required = false, Description = "Maximum property's ranking")]
    [FromQuery(Name = "ranking-max")]
    public int RankingMax { get; init; }
    
    [SwaggerParameter(Required = false, Description = "Property's status")]
    [FromQuery(Name = "status")]
    public string Status { get; init; }

    [SwaggerParameter(Required = false,
        Description = "Filters properties' registration date after the specified date. " +
                      "Format: YYYY-MM-DD HH24:mm:ss")]
    [FromQuery(Name = "from-created-at")]
    public string FromCreatedAt { get; init; }

    [SwaggerParameter(Required = false,
        Description = "Filters properties' registration date before the specified date. " +
                      "Format: YYYY-MM-DD HH24:mm:ss")]
    [FromQuery(Name = "to-created-at")]
    public string ToCreatedAt { get; init; }

    [SwaggerParameter(Required = false,
        Description = "Filters properties' update date after the specified date. " +
                      "Format: YYYY-MM-DD HH24:mm:ss")]
    [FromQuery(Name = "from-update-at")]
    public string FromUpdatedAt { get; init; }

    [SwaggerParameter(Required = false,
        Description = "Filters properties' update date before the specified date. " +
                      "Format: YYYY-MM-DD HH24:mm:ss")]
    [FromQuery(Name = "to-update-at")]
    public string ToUpdatedAt { get; init; }
}
