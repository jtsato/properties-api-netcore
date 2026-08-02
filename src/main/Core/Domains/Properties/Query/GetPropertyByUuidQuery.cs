using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FluentValidation;

namespace Core.Domains.Properties.Query;

public sealed class GetPropertyByUuidQuery
{
    private static readonly GetPropertyByUuidQueryValidator Validator = new GetPropertyByUuidQueryValidator();

    public string Uuid { get; }

    public GetPropertyByUuidQuery(string uuid)
    {
        Uuid = uuid;
        Validator.ValidateAndThrow(this);
    }

    [ExcludeFromCodeCoverage]
    private bool Equals(GetPropertyByUuidQuery other)
    {
        return Uuid == other.Uuid;
    }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is GetPropertyByUuidQuery other && Equals(other);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        return HashCode.Combine(Uuid);
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(Uuid)}: {Uuid}")
            .ToString();
    }
}