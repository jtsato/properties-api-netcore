﻿using System;
using System.Text;
using FluentValidation;

namespace Core.Domains.Properties.Query;


public sealed class GetPropertyByIdQuery
{
    private static readonly GetPropertyByIdQueryValidator Validator = new GetPropertyByIdQueryValidator();
    
    public string Id { get; }

    public GetPropertyByIdQuery(string id)
    {
        Id = id;
        Validator.ValidateAndThrow(this);
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