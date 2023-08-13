using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Domains.Properties.Query;

public class SearchPropertiesQueryLocation
{
    public string City { get; init; }
    public List<string> Districts { get; init; }

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
        return new StringBuilder()
            .AppendLine($"{nameof(City)}: {City}")
            .Append($"{nameof(Districts)}: {string.Join(", ", Districts)}")
            .ToString();
    }
}