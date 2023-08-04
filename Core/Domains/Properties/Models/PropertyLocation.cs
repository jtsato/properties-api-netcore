using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace Core.Domains.Properties.Models;

[ExcludeFromCodeCoverage]
public sealed class PropertyLocation
{
    public string City { get; init; }
    public string District { get; init; }
    public string Address { get; init; }

    private bool Equals(PropertyLocation other)
    {
        return City == other.City 
               && District == other.District
               && Address == other.Address;
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is PropertyLocation other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(District, Address, City);
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