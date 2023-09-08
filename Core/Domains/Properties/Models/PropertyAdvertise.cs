using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Core.Domains.Properties.Models;

public sealed class PropertyAdvertise
{
    public int TenantId { get; init; }
    public Transaction Transaction { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public string Url { get; init; }
    public string RefId { get; init; }
    public List<string> Images { get; init; }
    
    [ExcludeFromCodeCoverage]
    private bool Equals(PropertyAdvertise other)
    {
        return TenantId == other.TenantId
               && Transaction == other.Transaction
               && Title == other.Title
               && Description == other.Description
               && Url == other.Url
               && RefId == other.RefId
               && Equals(Images, other.Images);
    }
    
    [ExcludeFromCodeCoverage]
    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is PropertyAdvertise other && Equals(other);
    }
    
    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        return HashCode.Combine(TenantId, Transaction.Id, Title, Description, Url, RefId, Images);
    }
    
    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"{nameof(TenantId)}: {TenantId}")
            .AppendLine($"{nameof(Transaction)}: {Transaction}")
            .AppendLine($"{nameof(Title)}: {Title}")
            .AppendLine($"{nameof(Description)}: {Description}")
            .AppendLine($"{nameof(Url)}: {Url}")
            .AppendLine($"{nameof(RefId)}: {RefId}")
            .AppendLine($"{nameof(Images)}: {string.Join(",", Images)}")
            .ToString();
    }
}
