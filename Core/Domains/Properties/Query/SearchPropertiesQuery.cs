using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        List<string> types,
        SearchPropertiesQueryAdvertise advertise,
        SearchPropertiesQueryAttributes attributes,
        SearchPropertiesQueryLocation location,
        SearchPropertiesQueryPrices prices,
        string status) : base(types, status)
    {
        Types = types;
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
        return Types.SequenceEqual(other.Types)
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
        hashCode.Add(Types);
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
            .AppendLine($"{nameof(Types)}: {Types}")
            .AppendLine($"{nameof(Advertise)}: {Advertise}")
            .AppendLine($"{nameof(Attributes)}: {Attributes}")
            .AppendLine($"{nameof(Location)}: {Location}")
            .AppendLine($"{nameof(Prices)}: {Prices}")
            .Append($"{nameof(Status)}: {Status}")
            .ToString();
    }
}