using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace Core.Domains.Properties.Query;

[ExcludeFromCodeCoverage]
public class SearchPropertiesQueryAdvertise
{
    private string Transaction { get; init; }
    private string RefId { get; init; }
    
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
        PropertyInfo[] properties = GetType().GetProperties();
        StringBuilder stringBuilder = new StringBuilder();
        foreach (PropertyInfo propertyInfo in properties)
        {
            stringBuilder.AppendLine($"{propertyInfo.Name}: {propertyInfo.GetValue(this)}");
        }
        return stringBuilder.ToString();
    }
}
