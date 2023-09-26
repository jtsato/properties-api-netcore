using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Core.Commons.Models;

namespace Core.Domains.Properties.Query;

public class SearchPropertiesQueryPrices
{
    public Range<double> SellingPrice { get; init; }
    public Range<double> RentalPrice { get; init; }

    protected internal SearchPropertiesQueryPrices
    (
        Range<double> sellingPrice,
        Range<double> rentalPrice
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