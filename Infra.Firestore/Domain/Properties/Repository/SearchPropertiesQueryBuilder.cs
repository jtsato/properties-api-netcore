﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Core.Commons.Models;
using Core.Domains.Properties.Query;
using Google.Cloud.Firestore;
using Infra.Firestore.Domain.Properties.Model;

namespace Infra.Firestore.Domain.Properties.Repository
{
    public static class SearchPropertiesFirestoreQueryBuilder
    {
        private const string All = "ALL";

        public static Query BuildFromQuery(Query query, SearchPropertiesQuery searchPropertiesQuery)
        {
            string type = searchPropertiesQuery.Type.ToUpperInvariant() == All ? "" : searchPropertiesQuery.Type;

            query = AddFilter(query, ToLowerCamelCase(nameof(PropertyEntity.TenantId)), searchPropertiesQuery.TenantId);
            query = AddFilter(query, ToLowerCamelCase(nameof(PropertyEntity.Type)), type);
            query = AddFilter(query, ToLowerCamelCase(nameof(PropertyEntity.Transaction)), searchPropertiesQuery.Advertise.Transaction.ToUpperInvariant());
            query = AddFilter(query, ToLowerCamelCase(nameof(PropertyEntity.RefId)), searchPropertiesQuery.Advertise.RefId.ToUpperInvariant());
            query = AddFilter(query, ToLowerCamelCase(nameof(PropertyEntity.State)), searchPropertiesQuery.Location.State);
            query = AddFilter(query, ToLowerCamelCase(nameof(PropertyEntity.City)), searchPropertiesQuery.Location.City);
            query = AddFilter(query, ToLowerCamelCase(nameof(PropertyEntity.Status)), searchPropertiesQuery.Status);
            query = AddFilter(query, ToLowerCamelCase(nameof(PropertyEntity.District)), searchPropertiesQuery.Location.Districts);

            query = AddRangeFilter(query, ToLowerCamelCase(nameof(PropertyEntity.NumberOfBedrooms)), searchPropertiesQuery.Attributes.NumberOfBedrooms);
            query = AddRangeFilter(query, ToLowerCamelCase(nameof(PropertyEntity.NumberOfToilets)), searchPropertiesQuery.Attributes.NumberOfToilets);
            query = AddRangeFilter(query, ToLowerCamelCase(nameof(PropertyEntity.NumberOfGarages)), searchPropertiesQuery.Attributes.NumberOfGarages);
            query = AddRangeFilter(query, ToLowerCamelCase(nameof(PropertyEntity.Area)), searchPropertiesQuery.Attributes.Area);
            query = AddRangeFilter(query, ToLowerCamelCase(nameof(PropertyEntity.BuiltArea)), searchPropertiesQuery.Attributes.BuiltArea);
            query = AddRangeFilter(query, ToLowerCamelCase(nameof(PropertyEntity.SellingPrice)), searchPropertiesQuery.Prices.SellingPrice);
            query = AddRangeFilter(query, ToLowerCamelCase(nameof(PropertyEntity.RentalTotalPrice)), searchPropertiesQuery.Prices.RentalTotalPrice);
            query = AddRangeFilter(query, ToLowerCamelCase(nameof(PropertyEntity.RentalPrice)), searchPropertiesQuery.Prices.RentalPrice);
            query = AddRangeFilter(query, ToLowerCamelCase(nameof(PropertyEntity.PriceByM2)), searchPropertiesQuery.Prices.PriceByM2);
            query = AddRangeFilter(query, ToLowerCamelCase(nameof(PropertyEntity.Ranking)), searchPropertiesQuery.Rankings.Ranking);

            query = AddDateRangeFilter(query, ToLowerCamelCase(nameof(PropertyEntity.CreatedAt)), searchPropertiesQuery.CreatedAt);
            query = AddDateRangeFilter(query, ToLowerCamelCase(nameof(PropertyEntity.UpdatedAt)), searchPropertiesQuery.UpdatedAt);

            return query;
        }

        private static string ToLowerCamelCase(this string value) => char.ToLowerInvariant(value[0]) + value[1..];

        private static Query AddFilter(Query query, string fieldPath, object value)
        {
            if (value is null || string.IsNullOrWhiteSpace(value.ToString())) return query;

            switch (value)
            {
                case 0:
                    return query;
                case List<string> {Count: 0}:
                    return query;
                default:
                    if (value is List<string> list)
                    {
                        return query.WhereIn(fieldPath, list);
                    }

                    return query.WhereEqualTo(fieldPath, value);
            }
        }

        private static Query AddRangeFilter<T>(Query query, string fieldPath, Range<T> range)
        {
            if (range.From is null && range.To is null) return query;

            if (range.From is not null)
            {
                query = query.WhereGreaterThanOrEqualTo(fieldPath, range.From);
            }

            if (range.To is null) return query;

            query = query.WhereLessThanOrEqualTo(fieldPath, range.To);

            return query;
        }

        private static Query AddDateRangeFilter(Query query, string fieldPath, Range<string> range)
        {
            if (!string.IsNullOrWhiteSpace(range.From))
            {
                query = query.WhereGreaterThanOrEqualTo(fieldPath, ParseToTimestamp(range.From));
            }
            if (!string.IsNullOrWhiteSpace(range.To))
            {
                query = query.WhereLessThanOrEqualTo(fieldPath, ParseToTimestamp(range.To));
            }
            return query;
        }

        private static Timestamp ParseToTimestamp(string dateTimeAsString)
        {
            DateTime dateTime = DateTime.Parse(dateTimeAsString, new CultureInfo("pt-BR"));
            return Timestamp.FromDateTime(dateTime.ToUniversalTime());
        }
    }
}