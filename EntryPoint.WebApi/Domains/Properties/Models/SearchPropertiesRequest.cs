using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EntryPoint.WebApi.Domains.Properties.Models;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class SearchPropertiesRequest
{
    [SwaggerParameter(Required = false, Description = "Type")]
    [FromQuery(Name = "type")]
    [DefaultValue("ALL")]
    public string Type { get; init; }

    [SwaggerParameter(Required = false, Description = "Transaction type")]
    [FromQuery(Name = "transaction")]
    [DefaultValue("ALL")]
    public string Transaction { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum number of bedrooms")]
    [FromQuery(Name = "number-of-bedrooms-min")]
    [DefaultValue(0)]
    public byte NumberOfBedroomsMin { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum number of bedrooms")]
    [FromQuery(Name = "number-of-bedrooms-max")]
    [DefaultValue(255)]
    public byte NumberOfBedroomsMax { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum number of toilets")]
    [FromQuery(Name = "number-of-toilets-min")]
    [DefaultValue(0)]
    public byte NumberOfToiletsMin { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum number of toilets")]
    [FromQuery(Name = "number-of-toilets-max")]
    [DefaultValue(255)]
    public byte NumberOfToiletsMax { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum number of garages")]
    [FromQuery(Name = "number-of-garages-min")]
    [DefaultValue(0)]
    public byte NumberOfGaragesMin { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum number of garages")]
    [FromQuery(Name = "number-of-garages-max")]
    [DefaultValue(255)]
    public byte NumberOfGaragesMax { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum area in square meters")]
    [FromQuery(Name = "area-min")]
    [DefaultValue(0)]
    public int AreaMin { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum area in square meters")]
    [FromQuery(Name = "area-max")]
    [DefaultValue(9999)]
    public int AreaMax { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum built area in square meters")]
    [FromQuery(Name = "built-area-min")]
    [DefaultValue(0)]
    public int BuiltAreaMin { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum built area in square meters")]
    [FromQuery(Name = "built-area-max")]
    [DefaultValue(9999)]
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
    [DefaultValue(0)]
    public double SellingPriceMin { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum selling price")]
    [FromQuery(Name = "selling-price-max")]
    [DefaultValue(9999)]
    public double SellingPriceMax { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum rental price")]
    [FromQuery(Name = "rental-price-min")]
    [DefaultValue(0)]
    public double RentalPriceMin { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum rental price")]
    [FromQuery(Name = "rental-price-max")]
    [DefaultValue(9999)]
    public double RentalPriceMax { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum total rental price")]
    [FromQuery(Name = "rental-total-price-min")]
    [DefaultValue(0)]
    public double RentalTotalPriceMin { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum total rental price")]
    [FromQuery(Name = "rental-total-price-max")]
    [DefaultValue(9999)]
    public double RentalTotalPriceMax { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum price per square meter")]
    [FromQuery(Name = "price-by-m2-min")]
    [DefaultValue(0)]
    public double PriceByM2Min { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum price per square meter")]
    [FromQuery(Name = "price-by-m2-max")]
    [DefaultValue(9999)]
    public double PriceByM2Max { get; init; }

    [SwaggerParameter(Required = false, Description = "Property's status")]
    [FromQuery(Name = "status")]
    [DefaultValue("ALL")]
    public string Status { get; init; }
}