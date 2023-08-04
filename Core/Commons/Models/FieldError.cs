using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Commons.Models;

[ExcludeFromCodeCoverage]
public sealed class FieldError
{
    public string PropertyName { get; init; }
    
    public string ErrorMessage { get; init; }
    
    public string AttemptedValue { get; init; }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((FieldError) obj);
    }

    private bool Equals(FieldError other)
    {
        return PropertyName == other.PropertyName && ErrorMessage == other.ErrorMessage && AttemptedValue == other.AttemptedValue;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PropertyName, ErrorMessage, AttemptedValue);
    }
}