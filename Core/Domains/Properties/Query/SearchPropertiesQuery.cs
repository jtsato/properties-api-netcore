﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FluentValidation;

namespace Core.Domains.Properties.Query;

public class SearchPropertiesQuery : SearchPropertiesQueryBase
{
    private static readonly SearchPropertiesQueryValidator Validator = new SearchPropertiesQueryValidator();

    public SearchPropertiesQueryAdvertise Advertise { get; init; }
    public SearchPropertiesQueryAttributes Attributes { get; init; }
    public SearchPropertiesQueryLocation Location { get; init; }
    public SearchPropertiesQueryPrices Prices { get; init; }

    protected internal SearchPropertiesQuery(
        string type,
        SearchPropertiesQueryAdvertise advertise,
        SearchPropertiesQueryAttributes attributes,
        SearchPropertiesQueryLocation location,
        SearchPropertiesQueryPrices prices,
        string status) : base(type, status)
    {
        Type = type;
        Status = status;
        Advertise = advertise;
        Attributes = attributes;
        Location = location;
        Prices = prices;
        Validator.ValidateAndThrow(this);
    }

    [ExcludeFromCodeCoverage]
    private bool Equals(SearchPropertiesQuery other)
    {
        return Equals(Type, other.Type)
               && Equals(Advertise, other.Advertise)
               && Equals(Attributes, other.Attributes)
               && Equals(Location, other.Location)
               && Equals(Prices, other.Prices)
               && Equals(Status, other.Status);
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is SearchPropertiesQuery other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        HashCode hashCode = new HashCode();
        hashCode.Add(Type);
        hashCode.Add(Advertise);
        hashCode.Add(Attributes);
        hashCode.Add(Location);
        hashCode.Add(Prices);
        hashCode.Add(Status);
        return hashCode.ToHashCode();
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(Type)}: {Type}")
            .AppendLine($"{nameof(Advertise)}: {Advertise}")
            .AppendLine($"{nameof(Attributes)}: {Attributes}")
            .AppendLine($"{nameof(Location)}: {Location}")
            .AppendLine($"{nameof(Prices)}: {Prices}")
            .Append($"{nameof(Status)}: {Status}")
            .ToString();
    }
}