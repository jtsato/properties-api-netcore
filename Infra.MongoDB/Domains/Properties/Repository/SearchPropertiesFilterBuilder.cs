using System.Collections.Generic;
using Core.Domains.Properties.Query;
using Infra.MongoDB.Commons.Helpers;
using Infra.MongoDB.Domains.Properties.Model;
using MongoDB.Driver;

namespace Infra.MongoDB.Domains.Properties.Repository;

public static class SearchPropertiesFilterBuilder
{
    private const string NoFilter = "ALL";

    public static FilterDefinition<PropertyEntity> Build(SearchPropertiesQuery query)
    {
        List<FilterDefinition<PropertyEntity>> filters = new List<FilterDefinition<PropertyEntity>>();

        string type = query.Type.ToUpperInvariant() == NoFilter ? "" : query.Type;
        string transaction = query.Advertise.Transaction.ToUpperInvariant() == NoFilter ? "" : query.Advertise.Transaction;
        string status = query.Status.ToUpperInvariant() == NoFilter ? "" : query.Status;

        FilterHelper.AddEqualsFilter(filters, document => document.Type, type);
        FilterHelper.AddEqualsFilter(filters, document => document.Transaction, transaction);
        FilterHelper.AddGreaterOrEqualFilter(filters, document => document.NumberOfBedrooms, query.Attributes.NumberOfBedrooms.From);
        FilterHelper.AddLessOrEqualFilter(filters, document => document.NumberOfBedrooms, query.Attributes.NumberOfBedrooms.To);
        FilterHelper.AddGreaterOrEqualFilter(filters, document => document.NumberOfToilets, query.Attributes.NumberOfToilets.From);
        FilterHelper.AddLessOrEqualFilter(filters, document => document.NumberOfToilets, query.Attributes.NumberOfToilets.To);
        FilterHelper.AddGreaterOrEqualFilter(filters, document => document.NumberOfGarages, query.Attributes.NumberOfGarages.From);
        FilterHelper.AddLessOrEqualFilter(filters, document => document.NumberOfGarages, query.Attributes.NumberOfGarages.To);
        FilterHelper.AddGreaterOrEqualFilter(filters, document => document.Area, query.Attributes.Area.From);
        FilterHelper.AddLessOrEqualFilter(filters, document => document.Area, query.Attributes.Area.To);
        FilterHelper.AddGreaterOrEqualFilter(filters, document => document.BuiltArea, query.Attributes.BuiltArea.From);
        FilterHelper.AddLessOrEqualFilter(filters, document => document.BuiltArea, query.Attributes.BuiltArea.To);
        FilterHelper.AddEqualsFilter(filters, document => document.State, query.Location.State);
        FilterHelper.AddEqualsFilter(filters, document => document.City, query.Location.City);
        FilterHelper.AddInArrayFilter(filters, document => document.District, query.Location.Districts);
        FilterHelper.AddGreaterOrEqualFilter(filters, document => document.SellingPrice, query.Prices.SellingPrice.From);
        FilterHelper.AddLessOrEqualFilter(filters, document => document.SellingPrice, query.Prices.SellingPrice.To);
        FilterHelper.AddGreaterOrEqualFilter(filters, document => document.RentalTotalPrice, query.Prices.RentalTotalPrice.From);
        FilterHelper.AddLessOrEqualFilter(filters, document => document.RentalTotalPrice, query.Prices.RentalTotalPrice.To);
        FilterHelper.AddGreaterOrEqualFilter(filters, document => document.RentalPrice, query.Prices.RentalPrice.From);
        FilterHelper.AddLessOrEqualFilter(filters, document => document.RentalPrice, query.Prices.RentalPrice.To);
        FilterHelper.AddEqualsFilter(filters, document => document.Status, status);

        return filters.Count == 0 ? Builders<PropertyEntity>.Filter.Empty : Builders<PropertyEntity>.Filter.And(filters);
    }
}