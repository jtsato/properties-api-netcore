using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core.Domains.Properties.Query;

[ExcludeFromCodeCoverage]
public class SearchPropertiesQueryLocation
{
    private string City { get; init; }
    private List<string> Districts { get; init; }

    public SearchPropertiesQueryLocation(string city, List<string> districts)
    {
        City = city ?? string.Empty;
        Districts = districts ?? new List<string>();
    }

    private bool Equals(SearchPropertiesQueryLocation other)
    {
        return City == other.City
               && !Districts.Except(other.Districts).Any() && !other.Districts.Except(Districts).Any();
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is SearchPropertiesQueryLocation other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Districts, City);
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