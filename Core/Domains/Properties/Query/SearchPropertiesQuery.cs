using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Core.Commons.Models;
using FluentValidation;

namespace Core.Domains.Properties.Query;

[ExcludeFromCodeCoverage]
public class SearchPropertiesQuery
{
    private static readonly SearchPropertiesQueryValidator QueryValidator = new SearchPropertiesQueryValidator();

    public string Type { get; init; }
    public SearchPropertiesQueryAdvertise Advertise { get; init; }
    public SearchPropertiesQueryAttributes Attributes { get; init; }
    public SearchPropertiesQueryLocation Location { get; init; }
    public SearchPropertiesQueryPrices Prices { get; init; }
    // TODO: Add Ranking Range
    // TODO: Add Status Property
    public Range<string> CreatedAt { get; init; }
    public Range<string> UpdatedAt { get; init; }

    public SearchPropertiesQuery(
        string type,
        SearchPropertiesQueryAdvertise advertise,
        SearchPropertiesQueryAttributes attributes,
        SearchPropertiesQueryLocation location,
        SearchPropertiesQueryPrices prices,
        Range<string> createdAt,
        Range<string> updatedAt)
    {
        Type = type?.Trim().ToUpperInvariant();
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
        return Equals(Type, other.Type)
               && Equals(Advertise, other.Advertise)
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
        return HashCode.Combine(Type, Advertise, Attributes, Location, Prices, CreatedAt, UpdatedAt);
    }

    public override string ToString()
    {
        return new StringBuilder()
            .Append($"{nameof(Type)}: {Type}")
            .AppendLine($"{nameof(Advertise)}: {Advertise}")
            .AppendLine($"{nameof(Attributes)}: {Attributes}")
            .AppendLine($"{nameof(Location)}: {Location}")
            .AppendLine($"{nameof(Prices)}: {Prices}")
            .AppendLine($"{nameof(CreatedAt)}: {CreatedAt}")
            .AppendLine($"{nameof(UpdatedAt)}: {UpdatedAt}")
            .ToString();
    }
}