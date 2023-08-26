using System;
using System.Collections.Generic;
using System.Globalization;
using Core.Commons.Models;
using Core.Domains.Properties.Query;
using Google.Cloud.Firestore;
using Infra.Firestore.Domain.Properties.Model;

namespace Infra.Firestore.Domain.Properties.Repository
{
    public static class SearchPropertiesFilterBuilder
    {
        private const string PropertyTypeAll = "ALL";

        public static IEnumerable<Filter> BuildFromQuery(SearchPropertiesQuery query)
        {
            List<Filter> filters = new List<Filter>();

            AddFilter(filters, ToLowerCamelCase(nameof(PropertyEntity.TenantId)), query.TenantId);
            AddFilter(filters, ToLowerCamelCase(nameof(PropertyEntity.Type)), query.Type.ToUpperInvariant() == PropertyTypeAll ? "" : query.Type);
            AddFilter(filters, ToLowerCamelCase(nameof(PropertyEntity.Transaction)), query.Advertise.Transaction.ToUpperInvariant());
            AddFilter(filters, ToLowerCamelCase(nameof(PropertyEntity.RefId)), query.Advertise.RefId.ToUpperInvariant());
            AddFilter(filters, ToLowerCamelCase(nameof(PropertyEntity.State)), query.Location.State);
            AddFilter(filters, ToLowerCamelCase(nameof(PropertyEntity.City)), query.Location.City);
            AddFilter(filters, ToLowerCamelCase(nameof(PropertyEntity.Status)), query.Status);
            AddFilter(filters, ToLowerCamelCase(nameof(PropertyEntity.District)), query.Location.Districts);

            AddRangeFilter(filters, ToLowerCamelCase(nameof(PropertyEntity.NumberOfBedrooms)), query.Attributes.NumberOfBedrooms);
            AddRangeFilter(filters, ToLowerCamelCase(nameof(PropertyEntity.NumberOfToilets)), query.Attributes.NumberOfToilets);
            AddRangeFilter(filters, ToLowerCamelCase(nameof(PropertyEntity.NumberOfGarages)), query.Attributes.NumberOfGarages);
            AddRangeFilter(filters, ToLowerCamelCase(nameof(PropertyEntity.Area)), query.Attributes.Area);
            AddRangeFilter(filters, ToLowerCamelCase(nameof(PropertyEntity.BuiltArea)), query.Attributes.BuiltArea);
            AddRangeFilter(filters, ToLowerCamelCase(nameof(PropertyEntity.SellingPrice)), query.Prices.SellingPrice);
            AddRangeFilter(filters, ToLowerCamelCase(nameof(PropertyEntity.RentalTotalPrice)), query.Prices.RentalTotalPrice);
            AddRangeFilter(filters, ToLowerCamelCase(nameof(PropertyEntity.RentalPrice)), query.Prices.RentalPrice);
            AddRangeFilter(filters, ToLowerCamelCase(nameof(PropertyEntity.PriceByM2)), query.Prices.PriceByM2);
            AddRangeFilter(filters, ToLowerCamelCase(nameof(PropertyEntity.Ranking)), query.Rankings.Ranking);

            AddDateRangeFilter(filters, ToLowerCamelCase(nameof(PropertyEntity.CreatedAt)), query.CreatedAt);
            AddDateRangeFilter(filters, ToLowerCamelCase(nameof(PropertyEntity.UpdatedAt)), query.UpdatedAt);

            return filters;
        }

        private static string ToLowerCamelCase(this string value) => char.ToLowerInvariant(value[0]) + value[1..];

        private static void AddFilter(ICollection<Filter> filters, string fieldPath, object value)
        {
            if (value is null || string.IsNullOrWhiteSpace(value.ToString())) return;

            switch (value)
            {
                case 0:
                    return;
                case List<string> {Count: 0}:
                    return;
                default:
                    filters.Add(value is List<string> list ? Filter.InArray(fieldPath, list) : Filter.EqualTo(fieldPath, value));
                    break;
            }
        }

        private static void AddRangeFilter<T>(ICollection<Filter> filters, string fieldPath, Range<T> range)
        {
            if (range.From is not null) filters.Add(Filter.GreaterThan(fieldPath, range.From));
            if (range.To is not null) filters.Add(Filter.LessThan(fieldPath, range.To));
        }
        
        private static void AddDateRangeFilter(ICollection<Filter> filters, string fieldPath, Range<string> range)
        {
            if (!string.IsNullOrWhiteSpace(range.From)) filters.Add(Filter.GreaterThanOrEqualTo(fieldPath, ParseToTimestamp(range.From)));
            if (!string.IsNullOrWhiteSpace(range.To)) filters.Add(Filter.LessThanOrEqualTo(fieldPath, ParseToTimestamp(range.To)));
        }

        private static Timestamp ParseToTimestamp(string dateTimeStr)
        {
            DateTime dateTime = DateTime.Parse(dateTimeStr, new CultureInfo("pt-BR"));
            return Timestamp.FromDateTime(dateTime.ToUniversalTime());
        }
    }
}
