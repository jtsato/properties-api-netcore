using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Core.Domains.Properties.Query;

[ExcludeFromCodeCoverage]
public class SearchPropertiesQueryAdvertise
{
    public string Transaction { get; init; }
    public string RefId { get; init; }

    public SearchPropertiesQueryAdvertise(string transaction, string refId)
    {
        Transaction = transaction ?? string.Empty;
        RefId = refId ?? string.Empty;
    }

    private bool Equals(SearchPropertiesQueryAdvertise other)
    {
        return Transaction == other.Transaction
               && RefId == other.RefId;
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is SearchPropertiesQueryAdvertise other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Transaction, RefId);
    }

    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(Transaction)}: {Transaction}")
            .Append($"{nameof(RefId)}: {RefId}")
            .ToString();
    }
}