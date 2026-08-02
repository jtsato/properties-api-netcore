using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Core.Domains.Properties.Query;

public class SearchPropertiesQueryAdvertise
{
    public string Transaction { get; init; }

    protected internal SearchPropertiesQueryAdvertise(string transaction)
    {
        Transaction = transaction;
    }

    [ExcludeFromCodeCoverage]
    private bool Equals(SearchPropertiesQueryAdvertise other)
    {
        return Transaction == other.Transaction;
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is SearchPropertiesQueryAdvertise other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        return HashCode.Combine(Transaction);
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(Transaction)}: {Transaction}")
            .ToString();
    }
}