using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Core.Domains.Properties.Models;

public sealed class PropertyPrices
{
    public double SellingPrice { get; init; }
    public double RentalTotalPrice { get; init; }
    public double RentalPrice { get; init; }
    public double Discount { get; init; }
    public double CondominiumFee { get; init; }
    public double PriceByM2 { get; init; }

    [ExcludeFromCodeCoverage]
    private bool Equals(PropertyPrices other)
    {
        return SellingPrice.Equals(other.SellingPrice)
               && RentalTotalPrice.Equals(other.RentalTotalPrice)
               && RentalPrice.Equals(other.RentalPrice)
               && Discount.Equals(other.Discount)
               && CondominiumFee.Equals(other.CondominiumFee)
               && PriceByM2.Equals(other.PriceByM2);
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