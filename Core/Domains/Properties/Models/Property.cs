using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace Core.Domains.Properties.Models;

[ExcludeFromCodeCoverage]
public sealed class Property
{
    public long Id { get; init; }
    public PropertyType Type { get; init; }
    public PropertyAdvertise Advertise { get; init; }
    public PropertyAttributes Attributes { get; init; }
    public PropertyLocation Location { get; init; }
    public PropertyPrices Prices { get; init; }
    public string HashKey { get; init; }
    public byte Ranking { get; init; }
    public PropertyStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    
    private bool Equals(Property other)
    {
        return Id == other.Id
               && Type == other.Type
               && Equals(Advertise, other.Advertise)
               && Equals(Attributes, other.Attributes)
               && Equals(Location, other.Location)
               && Equals(Prices, other.Prices)
               && HashKey == other.HashKey
               && Ranking == other.Ranking
               && Status == other.Status
               && CreatedAt.Equals(other.CreatedAt)
               && UpdatedAt.Equals(other.UpdatedAt);
    }
    
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is Property other && Equals(other); 
    }
    
    public override int GetHashCode()
    {
        const int prime = 23;
        return prime + (Id == null ? 0 : Id.GetHashCode());
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
