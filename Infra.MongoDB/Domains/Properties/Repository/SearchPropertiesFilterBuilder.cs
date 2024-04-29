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

        List<string> types = query.Types.Contains(NoFilter) ? new List<string>() : query.Types;
        FilterHelper.AddInArrayFilter(filters, document => document.Type, types);

        string status = query.Status.ToUpperInvariant() == NoFilter ? "ACTIVE" : query.Status;

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

        switch (query.Advertise.Transaction.ToUpperInvariant())
        {
            case "RENT":
                List<FilterDefinition<PropertyEntity>> rentFilterDefinitions = BuildRentFilterDefinitions(query);
                filters.Add(Builders<PropertyEntity>.Filter.And(rentFilterDefinitions));
                break;
            case "SALE":
                List<FilterDefinition<PropertyEntity>> saleFilterDefinitions = BuildSaleFilterDefinitions(query);
                filters.Add(Builders<PropertyEntity>.Filter.And(saleFilterDefinitions));
                break;
            default:
            {
                AddPricesFilters(query, filters);
                break;
            }
        }

        FilterHelper.AddEqualsFilter(filters, document => document.Status, status);
        FilterHelper.AddEqualsFilter(filters, document => document.Ranking, query.Ranking);

        return filters.Count == 0 ? Builders<PropertyEntity>.Filter.Empty : Builders<PropertyEntity>.Filter.And(filters);
    }

    private static void AddPricesFilters(SearchPropertiesQuery query, ICollection<FilterDefinition<PropertyEntity>> filters)
    {
        List<FilterDefinition<PropertyEntity>> rentFilterDefinitions = BuildRentFilterDefinitions(query);
        List<FilterDefinition<PropertyEntity>> saleFilterDefinitions = BuildSaleFilterDefinitions(query);

        rentFilterDefinitions.Add(Builders<PropertyEntity>.Filter.Or(saleFilterDefinitions));
        filters.Add(Builders<PropertyEntity>.Filter.And(rentFilterDefinitions));
    }

    private static List<FilterDefinition<PropertyEntity>> BuildSaleFilterDefinitions(SearchPropertiesQuery query)
    {
        List<FilterDefinition<PropertyEntity>> saleFilterDefinitions = new List<FilterDefinition<PropertyEntity>>();
        FilterHelper.AddEqualsFilter(saleFilterDefinitions, document => document.Transaction, "SALE");
        FilterHelper.AddGreaterOrEqualFilter(saleFilterDefinitions, document => document.SellingPrice, query.Prices.SellingPrice.From);
        FilterHelper.AddLessOrEqualFilter(saleFilterDefinitions, document => document.SellingPrice, query.Prices.SellingPrice.To);
        return saleFilterDefinitions;
    }

    private static List<FilterDefinition<PropertyEntity>> BuildRentFilterDefinitions(SearchPropertiesQuery query)
    {
        List<FilterDefinition<PropertyEntity>> rentFilterDefinitions = new List<FilterDefinition<PropertyEntity>>();
        FilterHelper.AddEqualsFilter(rentFilterDefinitions, document => document.Transaction, "RENT");
        FilterHelper.AddGreaterOrEqualFilter(rentFilterDefinitions, document => document.RentalPrice, query.Prices.RentalPrice.From);
        FilterHelper.AddLessOrEqualFilter(rentFilterDefinitions, document => document.RentalPrice, query.Prices.RentalPrice.To);
        return rentFilterDefinitions;
    }
}