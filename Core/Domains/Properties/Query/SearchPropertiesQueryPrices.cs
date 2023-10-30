using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Core.Commons.Models;

namespace Core.Domains.Properties.Query;

public class SearchPropertiesQueryPrices
{
    public Range<float> SellingPrice { get; init; }
    public Range<float> RentalPrice { get; init; }

    protected internal SearchPropertiesQueryPrices
    (
        Range<float> sellingPrice,
        Range<float> rentalPrice
    )
    {
        SellingPrice = sellingPrice;
        RentalPrice = rentalPrice;
    }

    [ExcludeFromCodeCoverage]
    private bool Equals(SearchPropertiesQueryPrices other)
    {
        return SellingPrice.Equals(other.SellingPrice) && RentalPrice.Equals(other.RentalPrice);
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is SearchPropertiesQueryPrices other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        return HashCode.Combine(SellingPrice, RentalPrice);
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(SellingPrice)}: {SellingPrice}")
            .AppendLine($"{nameof(RentalPrice)}: {RentalPrice}")
            .ToString();
    }
}