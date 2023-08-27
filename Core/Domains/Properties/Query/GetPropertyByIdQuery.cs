using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Core.Domains.Properties.Query;

[ExcludeFromCodeCoverage]
public sealed class GetPropertyByIdQuery
{
    public string Id { get; }

    public GetPropertyByIdQuery(string id)
    {
        Id = id;
    }

    private bool Equals(GetPropertyByIdQuery other)
    {
        return Id == other.Id;
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is GetPropertyByIdQuery other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }

    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(Id)}: {Id}")
            .ToString();
    }
}