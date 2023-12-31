﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Core.Domains.Properties.Models;

public sealed class PropertyAttributes
{
    public byte NumberOfBedrooms { get; init; }
    public byte NumberOfToilets { get; init; }
    public byte NumberOfGarages { get; init; }
    public int Area { get; init; }
    public int BuiltArea { get; init; }

    [ExcludeFromCodeCoverage]
    private bool Equals(PropertyAttributes other)
    {
        return NumberOfBedrooms == other.NumberOfBedrooms
               && NumberOfToilets == other.NumberOfToilets
               && NumberOfGarages == other.NumberOfGarages
               && Area == other.Area
               && BuiltArea == other.BuiltArea;
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is PropertyAttributes other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        return HashCode.Combine(NumberOfBedrooms, NumberOfToilets, NumberOfGarages, Area, BuiltArea);
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(NumberOfBedrooms)}: {NumberOfBedrooms}")
            .AppendLine($"{nameof(NumberOfToilets)}: {NumberOfToilets}")
            .AppendLine($"{nameof(NumberOfGarages)}: {NumberOfGarages}")
            .AppendLine($"{nameof(Area)}: {Area}")
            .AppendLine($"{nameof(BuiltArea)}: {BuiltArea}")
            .ToString();
    }
}