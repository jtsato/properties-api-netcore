using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EntryPoint.WebApi.Domains.Properties.Models;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class SearchPropertiesRequest
{
    [SwaggerParameter(Required = false, Description = "Types")]
    [FromQuery(Name = "types")]
    [DefaultValue("ALL")]
    public List<string> Types { get; init; }

    [SwaggerParameter(Required = false, Description = "Transaction type")]
    [FromQuery(Name = "transaction")]
    [DefaultValue("ALL")]
    public string Transaction { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum number of bedrooms")]
    [FromQuery(Name = "minBedrooms")]
    [DefaultValue(0)]
    public byte MinBedrooms { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum number of bedrooms")]
    [FromQuery(Name = "maxBedrooms")]
    [DefaultValue(255)]
    public byte MaxBedrooms { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum number of toilets")]
    [FromQuery(Name = "minToilets")]
    [DefaultValue(0)]
    public byte MinToilets { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum number of toilets")]
    [FromQuery(Name = "maxToilets")]
    [DefaultValue(255)]
    public byte MaxToilets { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum number of garages")]
    [FromQuery(Name = "minGarages")]
    [DefaultValue(0)]
    public byte MinGarages { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum number of garages")]
    [FromQuery(Name = "maxGarages")]
    [DefaultValue(255)]
    public byte MaxGarages { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum area in square meters")]
    [FromQuery(Name = "minArea")]
    [DefaultValue(0)]
    public int MinArea { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum area in square meters")]
    [FromQuery(Name = "maxArea")]
    [DefaultValue(9999)]
    public int MaxArea { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum built area in square meters")]
    [FromQuery(Name = "minBuiltArea")]
    [DefaultValue(0)]
    public int MinBuiltArea { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum built area in square meters")]
    [FromQuery(Name = "maxBuiltArea")]
    [DefaultValue(9999)]
    public int MaxBuiltArea { get; init; }

    [SwaggerParameter(Required = false, Description = "Federative unit")]
    [FromQuery(Name = "uf")]
    public string Uf { get; init; }

    [SwaggerParameter(Required = false, Description = "City")]
    [FromQuery(Name = "city")]
    public string City { get; init; }

    [SwaggerParameter(Required = false, Description = "List of districts")]
    [FromQuery(Name = "districts")]
    public List<string> Districts { get; init; }

    [SwaggerParameter(Required = false, Description = "Minimum selling price")]
    [FromQuery(Name = "minPrice")]
    [DefaultValue(0)]
    public double MinPrice { get; init; }

    [SwaggerParameter(Required = false, Description = "Maximum selling price")]
    [FromQuery(Name = "maxPrice")]
    [DefaultValue(9999)]
    public double MaxPrice { get; init; }

    [SwaggerParameter(Required = false, Description = "Property's status")]
    [FromQuery(Name = "status")]
    [DefaultValue("ALL")]
    public string Status { get; init; }
}
