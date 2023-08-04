using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using Core.Commons.Models;

namespace Core.Domains.Properties.Query;

[ExcludeFromCodeCoverage]
public class SearchPropertiesQueryAttributes
{
    public Range<byte> NumberOfBedrooms { get; init; }
    public Range<byte> NumberOfToilets { get; init; }
    public Range<byte> NumberOfGarages { get; init; }
    public Range<int> Area { get; init; }
    public Range<int> BuiltArea { get; init; }

    private bool Equals(SearchPropertiesQueryAttributes other)
    {
        return NumberOfBedrooms.Equals(other.NumberOfBedrooms)
               && NumberOfToilets.Equals(other.NumberOfToilets)
               && NumberOfGarages.Equals(other.NumberOfGarages)
               && Area.Equals(other.Area)
               && BuiltArea.Equals(other.BuiltArea);
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is SearchPropertiesQueryAttributes other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(NumberOfBedrooms, NumberOfToilets, NumberOfGarages, Area, BuiltArea);
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