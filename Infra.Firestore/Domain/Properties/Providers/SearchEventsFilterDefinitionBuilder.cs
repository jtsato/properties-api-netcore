using System.Collections.Generic;
using Core.Domains.Properties.Query;
using Google.Cloud.Firestore;
using Infra.Firestore.Domain.Properties.Model;

namespace Infra.Firestore.Domain.Properties.Providers;

public static class SearchPropertiesFilterBuilder
{
    private const string PropertyTypeAll = "ALL";
    
    public static IEnumerable<Filter> BuildFromQuery(SearchPropertiesQuery query)
    {
        List<Filter> filters = new List<Filter>();

        string propertyType = query.Type.ToUpperInvariant() == PropertyTypeAll ? "" : query.Type;
        
        AddEqualToComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.Transaction)), query.Advertise.Transaction.ToUpperInvariant());
        AddEqualToComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.Type)), propertyType);
        AddEqualToComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.City)), query.Location.City);
        AddEqualToComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.State)), query.Location.State);
        
        
        // TODO: Add more filters

        return filters;
    }
    
    private static string ToLowerCamelCase(this string value)
    {
        return char.ToLowerInvariant(value[0]) + value[1..];
    }
    
    private static void AddEqualToComparison(ICollection<Filter> filters, string fieldPath, object value)
    {
        switch (value)
        {
            case null:
            case string when string.IsNullOrWhiteSpace(value.ToString()):
                return;
            default:
                filters.Add(Filter.EqualTo(fieldPath, value));
                break;
        }
    }
}