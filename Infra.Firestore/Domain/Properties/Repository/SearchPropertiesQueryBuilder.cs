using System.Collections.Generic;
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

            query = AddWhere(query, ToLowerCamelCase(nameof(PropertyEntity.Type)), type);
            query = AddWhere(query, ToLowerCamelCase(nameof(PropertyEntity.Transaction)), searchPropertiesQuery.Advertise.Transaction.ToUpperInvariant());
            query = AddWhere(query, ToLowerCamelCase(nameof(PropertyEntity.State)), searchPropertiesQuery.Location.State);
            query = AddWhere(query, ToLowerCamelCase(nameof(PropertyEntity.City)), searchPropertiesQuery.Location.City);
            query = AddWhere(query, ToLowerCamelCase(nameof(PropertyEntity.Status)), searchPropertiesQuery.Status);
            query = AddWhere(query, ToLowerCamelCase(nameof(PropertyEntity.District)), searchPropertiesQuery.Location.Districts);

            query = AddRangeWhere(query, ToLowerCamelCase(nameof(PropertyEntity.NumberOfBedrooms)), searchPropertiesQuery.Attributes.NumberOfBedrooms);
            query = AddRangeWhere(query, ToLowerCamelCase(nameof(PropertyEntity.NumberOfToilets)), searchPropertiesQuery.Attributes.NumberOfToilets);
            query = AddRangeWhere(query, ToLowerCamelCase(nameof(PropertyEntity.NumberOfGarages)), searchPropertiesQuery.Attributes.NumberOfGarages);
            query = AddRangeWhere(query, ToLowerCamelCase(nameof(PropertyEntity.Area)), searchPropertiesQuery.Attributes.Area);
            query = AddRangeWhere(query, ToLowerCamelCase(nameof(PropertyEntity.BuiltArea)), searchPropertiesQuery.Attributes.BuiltArea);
            query = AddRangeWhere(query, ToLowerCamelCase(nameof(PropertyEntity.SellingPrice)), searchPropertiesQuery.Prices.SellingPrice);
            query = AddRangeWhere(query, ToLowerCamelCase(nameof(PropertyEntity.RentalTotalPrice)), searchPropertiesQuery.Prices.RentalTotalPrice);
            query = AddRangeWhere(query, ToLowerCamelCase(nameof(PropertyEntity.PriceByM2)), searchPropertiesQuery.Prices.PriceByM2);

            return query;
        }

        private static string ToLowerCamelCase(this string value) => char.ToLowerInvariant(value[0]) + value[1..];

        private static Query AddWhere(Query query, string fieldPath, object value)
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

        private static Query AddRangeWhere<T>(Query query, string fieldPath, Range<T> range)
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
    }
}