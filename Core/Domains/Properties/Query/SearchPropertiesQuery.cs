using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using Core.Commons.Models;
using FluentValidation;

namespace Core.Domains.Properties.Query;

[ExcludeFromCodeCoverage]
public class SearchPropertiesQuery
{
    private static readonly SearchPropertiesQueryValidator QueryValidator = new SearchPropertiesQueryValidator();

    private SearchPropertiesQueryAdvertise Advertise { get; init; }
    private SearchPropertiesQueryAttributes Attributes { get; init; }
    private SearchPropertiesQueryLocation Location { get; init; }
    private SearchPropertiesQueryPrices Prices { get; init; }
    public Range<string> CreatedAt { get; init; }
    public Range<string> UpdatedAt { get; init; }

    public SearchPropertiesQuery(
        SearchPropertiesQueryAdvertise advertise,
        SearchPropertiesQueryAttributes attributes,
        SearchPropertiesQueryLocation location,
        SearchPropertiesQueryPrices prices,
        Range<string> createdAt,
        Range<string> updatedAt)
    {
        Advertise = advertise;
        Attributes = attributes;
        Location = location;
        Prices = prices;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        QueryValidator.ValidateAndThrow(this);
    }

    private bool Equals(SearchPropertiesQuery other)
    {
        return Equals(Advertise, other.Advertise)
               && Equals(Attributes, other.Attributes)
               && Equals(Location, other.Location)
               && Equals(Prices, other.Prices)
               && Equals(CreatedAt, other.CreatedAt)
               && Equals(UpdatedAt, other.UpdatedAt);
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is SearchPropertiesQuery other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Advertise, Attributes, Location, Prices, CreatedAt, UpdatedAt);
    }

    public override string ToString()
    {
        PropertyInfo[] properties = GetType().GetProperties();
        StringBuilder stringBuilder = new StringBuilder();
        foreach (PropertyInfo propertyInfo in properties)
        {
            stringBuilder.AppendLine($"{propertyInfo.Name}: {propertyInfo.GetValue(this)}");
        }

        return stringBuilder.ToString();
    }
}