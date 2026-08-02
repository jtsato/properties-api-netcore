using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Core.Commons.Models;

namespace Core.Domains.Properties.Query;

public class SearchPropertiesQueryPrices
{
    public Range<float> SellingPrice { get; init; }
    public Range<float> RentalTotalPrice { get; init; }

    protected internal SearchPropertiesQueryPrices
    (
        Range<float> sellingPrice,
        Range<float> rentalTotalPrice
    )
    {
        SellingPrice = sellingPrice;
        RentalTotalPrice = rentalTotalPrice;
    }

    [ExcludeFromCodeCoverage]
    private bool Equals(SearchPropertiesQueryPrices other)
    {
        return SellingPrice.Equals(other.SellingPrice) && RentalTotalPrice.Equals(other.RentalTotalPrice);
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is SearchPropertiesQueryPrices other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        return HashCode.Combine(SellingPrice, RentalTotalPrice);
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(SellingPrice)}: {SellingPrice}")
            .AppendLine($"{nameof(RentalTotalPrice)}: {RentalTotalPrice}")
            .ToString();
    }
}