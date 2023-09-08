using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Core.Domains.Properties.Models;

public sealed class PropertyLocation
{
    public string City { get; init; }
    public string State { get; init; }
    public string District { get; init; }
    public string Address { get; init; }

    [ExcludeFromCodeCoverage]
    private bool Equals(PropertyLocation other)
    {
        return City == other.City 
               && State == other.State
               && District == other.District
               && Address == other.Address;
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is PropertyLocation other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        return HashCode.Combine(City, State, District, Address);
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(City)}: {City}")
            .AppendLine($"{nameof(State)}: {State}")
            .AppendLine($"{nameof(District)}: {District}")
            .AppendLine($"{nameof(Address)}: {Address}")
            .ToString();
    }
}