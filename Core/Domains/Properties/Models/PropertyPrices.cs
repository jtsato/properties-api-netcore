using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace Core.Domains.Properties.Models;

[ExcludeFromCodeCoverage]
public sealed class PropertyPrices
{
    public decimal SellingPrice { get; init; }
    public decimal RentalTotalPrice { get; init; }
    public decimal RentalPrice { get; init; }
    public decimal Discount { get; init; }
    public decimal CondominiumFee { get; init; }
    public decimal PriceByM2 { get; init; }
    
    private bool Equals(PropertyPrices other)
    {
        return SellingPrice == other.SellingPrice
               && RentalTotalPrice == other.RentalTotalPrice
               && RentalPrice == other.RentalPrice
               && Discount == other.Discount
               && CondominiumFee == other.CondominiumFee
               && PriceByM2 == other.PriceByM2;
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is PropertyPrices other && Equals(other);
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(SellingPrice, RentalTotalPrice, RentalPrice, Discount, CondominiumFee, PriceByM2);
    }

    public override string ToString()
    {
        PropertyInfo[] properties = GetType().GetProperties();
        StringBuilder stringBuilder = new StringBuilder();
        foreach (PropertyInfo propertyInfo in properties)
        {
            stringBuilder.AppendLine($"{propertyInfo.Name}: {propertyInfo.GetValue(this)}");
        }
        return stringBuilder.ToString();
    }
}
