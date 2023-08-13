using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Core.Commons.Models;

namespace Core.Domains.Properties.Query;

[ExcludeFromCodeCoverage]
public class SearchPropertiesQueryPrices
{
    public Range<decimal> SellingPrice { get; init; }
    public Range<decimal> RentalTotalPrice { get; init; }
    public Range<decimal> RentalPrice { get; init; }
    public Range<decimal> PriceByM2 { get; init; }

    private bool Equals(SearchPropertiesQueryPrices other)
    {
        return SellingPrice.Equals(other.SellingPrice)
               && RentalTotalPrice.Equals(other.RentalTotalPrice)
               && RentalPrice.Equals(other.RentalPrice)
               && PriceByM2.Equals(other.PriceByM2);
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is SearchPropertiesQueryPrices other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(SellingPrice, RentalTotalPrice, RentalPrice, PriceByM2);
    }

    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(SellingPrice)}: {SellingPrice}")
            .AppendLine($"{nameof(RentalTotalPrice)}: {RentalTotalPrice}")
            .AppendLine($"{nameof(RentalPrice)}: {RentalPrice}")
            .Append($"{nameof(PriceByM2)}: {PriceByM2}")
            .ToString();
    }
}
