using System.Collections.Generic;
using Core.Domains.Properties.Query;
using Google.Cloud.Firestore;
using Infra.Firestore.Domain.Properties.Model;

namespace Infra.Firestore.Domain.Properties.Providers;

public static class SearchPropertiesFilterDefinitionBuilder
{
    public static List<Filter> Of(SearchPropertiesQuery query)
    {
        List<Filter> filters = new List<Filter>();
        
        string propertyType = query.Type == "All" ? "" : query.Type;
        
        AddEqualToComparison(filters, nameof(PropertyEntity.Transaction), query.Advertise.Transaction);
        AddEqualToComparison(filters, nameof(PropertyEntity.Type), propertyType);
        AddEqualToComparison(filters, nameof(PropertyEntity.City), query.Location.City);

        return filters;
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