using System.Collections.Generic;
using Core.Domains.Properties.Query;
using Google.Cloud.Firestore;
using Infra.Firestore.Domain.Properties.Models;

namespace Infra.Firestore.Domain.Properties.Providers;

public static class SearchPropertiesFilterDefinitionBuilder
{
    public static List<Filter> Of(SearchPropertiesQuery query)
    {
        List<Filter> filters = new List<Filter>();

        AddEqualToComparison(filters, nameof(PropertyEntity.Transaction), query.Advertise.Transaction);
        AddEqualToComparison(filters, nameof(PropertyEntity.Type), query.Type);
        AddEqualToComparison(filters, nameof(PropertyEntity.City), query.Location.City);

        return filters;
    }

    private static void AddEqualToComparison(ICollection<Filter> filters, string fieldPath, object value)
    {
        if (value is null) return;
        if (value is string && string.IsNullOrWhiteSpace(value.ToString())) return;
        filters.Add(Filter.EqualTo(fieldPath, value));
    }
}