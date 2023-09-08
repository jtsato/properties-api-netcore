using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Core.Domains.Properties.Query;

public class SearchPropertiesQueryLocation
{
    public string State { get; init; }
    public string City { get; init; }
    public List<string> Districts { get; init; }

    public SearchPropertiesQueryLocation(string state, string city, List<string> districts)
    {
        State = state ?? string.Empty;
        City = city ?? string.Empty;
        Districts = districts ?? new List<string>();
    }
    
    [ExcludeFromCodeCoverage]
    private bool Equals(SearchPropertiesQueryLocation other)
    {
        return State == other.State 
               && City == other.City
               && !Districts.Except(other.Districts).Any() && !other.Districts.Except(Districts).Any();
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is SearchPropertiesQueryLocation other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        return HashCode.Combine(State, City, Districts);
    }

    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(State)}: {State}")
            .AppendLine($"{nameof(City)}: {City}")
            .AppendLine($"{nameof(Districts)}: {string.Join(",", Districts)}")
            .ToString();
    }
}