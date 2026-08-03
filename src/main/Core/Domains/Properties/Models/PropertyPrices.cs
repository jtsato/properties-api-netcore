using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Core.Domains.Properties.Models;

public sealed class PropertyPrices
{
    // Epsilon is used to compare floating point numbers
    private const double Epsilon = 0.0001;

    public double SellingPrice { get; init; }
    public double RentalTotalPrice { get; init; }
    public double RentalPrice { get; init; }
    public double Discount { get; init; }
    public double CondominiumFee { get; init; }
    public double PriceByM2 { get; init; }

    [ExcludeFromCodeCoverage]
    private bool Equals(PropertyPrices other)
    {
        return Math.Abs(SellingPrice - other.SellingPrice) < Epsilon
               && Math.Abs(RentalTotalPrice - other.RentalTotalPrice) < Epsilon
               && Math.Abs(RentalPrice - other.RentalPrice) < Epsilon
               && Math.Abs(Discount - other.Discount) < Epsilon
               && Math.Abs(CondominiumFee - other.CondominiumFee) < Epsilon
               && Math.Abs(PriceByM2 - other.PriceByM2) < Epsilon;
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is PropertyPrices other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        return HashCode.Combine(SellingPrice, RentalTotalPrice, RentalPrice, Discount, CondominiumFee, PriceByM2);
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(SellingPrice)}: {SellingPrice}")
            .AppendLine($"{nameof(RentalTotalPrice)}: {RentalTotalPrice}")
            .AppendLine($"{nameof(RentalPrice)}: {RentalPrice}")
            .AppendLine($"{nameof(Discount)}: {Discount}")
            .AppendLine($"{nameof(CondominiumFee)}: {CondominiumFee}")
            .Append($"{nameof(PriceByM2)}: {PriceByM2}")
            .ToString();
    }
}