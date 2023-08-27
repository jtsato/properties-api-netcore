using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Core.Domains.Properties.Query;

[ExcludeFromCodeCoverage]
public class SearchPropertiesQueryAdvertise
{
    public string Transaction { get; init; }

    public SearchPropertiesQueryAdvertise(string transaction)
    {
        Transaction = transaction ?? string.Empty;
    }

    private bool Equals(SearchPropertiesQueryAdvertise other)
    {
        return Transaction == other.Transaction;
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is SearchPropertiesQueryAdvertise other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Transaction);
    }

    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(Transaction)}: {Transaction}")
            .ToString();
    }
}