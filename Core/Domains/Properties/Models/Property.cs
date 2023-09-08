using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Core.Domains.Properties.Models;

public sealed class Property
{
    public long Id { get; init; }
    public PropertyType Type { get; init; }
    public PropertyAdvertise Advertise { get; init; }
    public PropertyAttributes Attributes { get; init; }
    public PropertyLocation Location { get; init; }
    public PropertyPrices Prices { get; init; }
    public string HashKey { get; init; }
    public byte Ranking { get; init; }
    public PropertyStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    
    [ExcludeFromCodeCoverage]
    private bool Equals(Property other)
    {
        return Id == other.Id
               && Type == other.Type
               && Equals(Advertise, other.Advertise)
               && Equals(Attributes, other.Attributes)
               && Equals(Location, other.Location)
               && Equals(Prices, other.Prices)
               && HashKey == other.HashKey
               && Ranking == other.Ranking
               && Status == other.Status
               && CreatedAt.Equals(other.CreatedAt)
               && UpdatedAt.Equals(other.UpdatedAt);
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is Property other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        HashCode hashCode = new HashCode();
        hashCode.Add(Id);
        hashCode.Add(Type.Id);
        hashCode.Add(Advertise);
        hashCode.Add(Attributes);
        hashCode.Add(Location);
        hashCode.Add(Prices);
        hashCode.Add(HashKey);
        hashCode.Add(Ranking);
        hashCode.Add(Status.Id);
        hashCode.Add(CreatedAt);
        hashCode.Add(UpdatedAt);
        return hashCode.ToHashCode();
    }

    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(Id)}: {Id}")
            .AppendLine($"{nameof(Type)}: {Type}")
            .AppendLine($"{nameof(Advertise)}: {Advertise}")
            .AppendLine($"{nameof(Attributes)}: {Attributes}")
            .AppendLine($"{nameof(Location)}: {Location}")
            .AppendLine($"{nameof(Prices)}: {Prices}")
            .AppendLine($"{nameof(HashKey)}: {HashKey}")
            .AppendLine($"{nameof(Ranking)}: {Ranking}")
            .AppendLine($"{nameof(Status)}: {Status}")
            .AppendLine($"{nameof(CreatedAt)}: {CreatedAt}")
            .AppendLine($"{nameof(UpdatedAt)}: {UpdatedAt}")
            .ToString();
    }
}