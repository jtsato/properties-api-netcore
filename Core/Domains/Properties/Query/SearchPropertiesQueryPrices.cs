﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Core.Commons.Models;

namespace Core.Domains.Properties.Query;

public class SearchPropertiesQueryPrices
{
    public Range<double> SellingPrice { get; init; }
    public Range<double> RentalTotalPrice { get; init; }
    public Range<double> RentalPrice { get; init; }
    public Range<double> PriceByM2 { get; init; }
    
    protected internal SearchPropertiesQueryPrices(
        Range<double> sellingPrice,
        Range<double> rentalTotalPrice,
        Range<double> rentalPrice,
        Range<double> priceByM2)
    {
        SellingPrice = sellingPrice;
        RentalTotalPrice = rentalTotalPrice;
        RentalPrice = rentalPrice;
        PriceByM2 = priceByM2;
    }

    [ExcludeFromCodeCoverage]
    private bool Equals(SearchPropertiesQueryPrices other)
    {
        return SellingPrice.Equals(other.SellingPrice)
               && RentalTotalPrice.Equals(other.RentalTotalPrice)
               && RentalPrice.Equals(other.RentalPrice)
               && PriceByM2.Equals(other.PriceByM2);
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is SearchPropertiesQueryPrices other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        return HashCode.Combine(SellingPrice, RentalTotalPrice, RentalPrice, PriceByM2);
    }

    [ExcludeFromCodeCoverage]
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