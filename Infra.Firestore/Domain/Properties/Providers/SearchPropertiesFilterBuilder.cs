using System;
using System.Collections.Generic;
using System.Globalization;
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

        AddEqualToComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.TenantId)), query.TenantId);
        string propertyType = query.Type.ToUpperInvariant() == PropertyTypeAll ? "" : query.Type;
        
        AddEqualToComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.Type)), propertyType);
        AddEqualToComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.Transaction)), query.Advertise.Transaction.ToUpperInvariant());
        AddEqualToComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.RefId)), query.Advertise.RefId.ToUpperInvariant());
        AddEqualToComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.State)), query.Location.State);
        AddEqualToComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.City)), query.Location.City);
        AddEqualToComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.Status)), query.Status);

        AddInArrayComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.District)), query.Location.Districts);
        
        AddGreaterThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.NumberOfBedrooms)), query.Attributes.NumberOfBedrooms.From);
        AddGreaterThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.NumberOfToilets)), query.Attributes.NumberOfToilets.From);
        AddGreaterThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.NumberOfGarages)), query.Attributes.NumberOfGarages.From);
        AddGreaterThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.Area)), query.Attributes.Area.From);
        AddGreaterThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.BuiltArea)), query.Attributes.BuiltArea.From);
        AddGreaterThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.SellingPrice)), query.Prices.SellingPrice.From);
        AddGreaterThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.RentalTotalPrice)), query.Prices.RentalTotalPrice.From);
        AddGreaterThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.RentalPrice)), query.Prices.RentalPrice.From);
        AddGreaterThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.PriceByM2)), query.Prices.PriceByM2.From);
        AddGreaterThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.Ranking)), query.Rankings.Ranking.From);
        
        AddLessThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.NumberOfBedrooms)), query.Attributes.NumberOfBedrooms.To);
        AddLessThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.NumberOfToilets)), query.Attributes.NumberOfToilets.To);
        AddLessThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.NumberOfGarages)), query.Attributes.NumberOfGarages.To);
        AddLessThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.Area)), query.Attributes.Area.To);
        AddLessThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.BuiltArea)), query.Attributes.BuiltArea.To);
        AddLessThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.SellingPrice)), query.Prices.SellingPrice.To);
        AddLessThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.RentalTotalPrice)), query.Prices.RentalTotalPrice.To);
        AddLessThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.RentalPrice)), query.Prices.RentalPrice.To);
        AddLessThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.PriceByM2)), query.Prices.PriceByM2.To);
        AddLessThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.Ranking)), query.Rankings.Ranking.To);
        
        AddDateGreaterThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.CreatedAt)), query.CreatedAt.From);
        AddDateGreaterThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.UpdatedAt)), query.UpdatedAt.From);
        AddDateLessThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.CreatedAt)), query.CreatedAt.To);
        AddDateLessThanComparison(filters, ToLowerCamelCase(nameof(PropertyEntity.UpdatedAt)), query.UpdatedAt.To);

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
            case 0:
                return;
            default:
                filters.Add(Filter.EqualTo(fieldPath, value));
                break;
        }
    }
    
    private static void AddInArrayComparison(ICollection<Filter> filters, string fieldPath, List<string> values)
    {
        switch (values)
        {
            case null:
            case not null when values.Count == 0:
                return;
            default:
                filters.Add(Filter.InArray(fieldPath, values));
                break;
        }
    }
    
    private static void AddGreaterThanComparison(ICollection<Filter> filters, string fieldPath, object from)
    {
        switch (from)
        {
            case null:
            case string when string.IsNullOrWhiteSpace(from.ToString()):
                return;
            case byte and <= 0:
                return;
            case <= 0:
                return;
            default:
                filters.Add(Filter.GreaterThan(fieldPath, from));
                break;
        }
    }

    private static void AddLessThanComparison(ICollection<Filter> filters, string fieldPath, object to)
    {
        switch (to)
        {
            case null:
            case string when string.IsNullOrWhiteSpace(to.ToString()):
                return;
            case byte and <= 0:
                return;
            case <= 0:
                return;
            default:
                filters.Add(Filter.LessThan(fieldPath, to));
                break;
        }
    }
    
    private static void AddDateGreaterThanComparison(ICollection<Filter> filters, string fieldPath, string from)
    {
        DateTime dateTime = DateTime.Parse(from, new CultureInfo("pt-BR"));
        Timestamp timestamp = Timestamp.FromDateTime(dateTime.ToUniversalTime()); 
        filters.Add(Filter.GreaterThanOrEqualTo(fieldPath, timestamp));
    }
    
    private static void AddDateLessThanComparison(ICollection<Filter> filters, string fieldPath, string to)
    {
        DateTime dateTime = DateTime.Parse(to, new CultureInfo("pt-BR"));
        Timestamp timestamp = Timestamp.FromDateTime(dateTime.ToUniversalTime()); 
        filters.Add(Filter.LessThanOrEqualTo(fieldPath, timestamp));
    }
}
