using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Core.Commons.Models;

namespace Core.Domains.Properties.Query;

[ExcludeFromCodeCoverage]
public class SearchPropertiesQueryRanking
{
    public Range<int> Ranking { get; init; }

    private bool Equals(SearchPropertiesQueryRanking other)
    {
        return Ranking.Equals(other.Ranking);
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is SearchPropertiesQueryRanking other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Ranking);
    }

    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(Ranking)}: {Ranking}")
            .ToString();
    }
}
