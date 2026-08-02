using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Domains.Properties.Query;
using Infra.MongoDB.Commons.Helpers;
using Infra.MongoDB.Domains.Properties.Model;
using MongoDB.Driver;

namespace Infra.MongoDB.Domains.Properties.Repository;

public static class SearchPropertiesFilterBuilder
{
    private const string NoFilter = "ALL";
    private const string RentTransaction = "RENT";
    private const string SaleTransaction = "SALE";
    private const string DefaultStatusFilter = "ACTIVE";
    
    private const int DefaultMaxArea = 999999;
    private const byte DefaultMaxRooms = 255;
    private const float DefaultMaxPrice = 100000000;

    public static FilterDefinition<PropertyEntity> Build(SearchPropertiesQuery query)
    {
        List<FilterDefinition<PropertyEntity>> filters = new List<FilterDefinition<PropertyEntity>>();

        List<string> types = query.Types.Contains(NoFilter) ? new List<string>() : query.Types;
        FilterHelper.AddInArrayFilter(filters, document => document.Type, types);

        string status = query.Status.ToUpperInvariant() == NoFilter ? DefaultStatusFilter : query.Status;
        FilterHelper.AddEqualsFilter(filters, document => document.State, query.Location.State);
        FilterHelper.AddEqualsFilter(filters, document => document.City, query.Location.City);
        FilterHelper.AddInArrayFilter(filters, document => document.District, query.Location.Districts);

        FilterHelper.AddGreaterOrEqualFilter(filters, document => document.NumberOfBedrooms, query.Attributes.NumberOfBedrooms.From);
        FilterHelper.AddGreaterOrEqualFilter(filters, document => document.NumberOfToilets, query.Attributes.NumberOfToilets.From);
        FilterHelper.AddGreaterOrEqualFilter(filters, document => document.NumberOfGarages, query.Attributes.NumberOfGarages.From);
        FilterHelper.AddGreaterOrEqualFilter(filters, document => document.Area, query.Attributes.Area.From);
        FilterHelper.AddGreaterOrEqualFilter(filters, document => document.BuiltArea, query.Attributes.BuiltArea.From);

        AddToFiltersIfLessOrThanDefaultValue(filters, document => document.NumberOfBedrooms, query.Attributes.NumberOfBedrooms.To, DefaultMaxRooms);
        AddToFiltersIfLessOrThanDefaultValue(filters, document => document.NumberOfToilets, query.Attributes.NumberOfToilets.To, DefaultMaxRooms);
        AddToFiltersIfLessOrThanDefaultValue(filters, document => document.NumberOfGarages, query.Attributes.NumberOfGarages.To, DefaultMaxRooms);
        AddToFiltersIfLessOrThanDefaultValue(filters, document => document.Area, query.Attributes.Area.To, DefaultMaxArea);
        AddToFiltersIfLessOrThanDefaultValue(filters, document => document.BuiltArea, query.Attributes.BuiltArea.To, DefaultMaxArea);

        switch (query.Advertise.Transaction.ToUpperInvariant())
        {
            case RentTransaction:
                FilterHelper.AddEqualsFilter(filters, document => document.Transaction, RentTransaction);
                filters.Add(Builders<PropertyEntity>.Filter.And(BuildRentFilterDefinitions(query)));
                break;
            case SaleTransaction:
                FilterHelper.AddEqualsFilter(filters, document => document.Transaction, SaleTransaction);
                filters.Add(Builders<PropertyEntity>.Filter.And(BuildSaleFilterDefinitions(query)));
                break;
            default:
            {
                AddPricesFilters(query, filters);
                break;
            }
        }

        FilterHelper.AddEqualsFilter(filters, document => document.Status, status);
        FilterHelper.AddGreaterOrEqualFilter(filters, document => document.Ranking, query.Ranking);

        return filters.Count == 0 ? Builders<PropertyEntity>.Filter.Empty : Builders<PropertyEntity>.Filter.And(filters);
    }

    private static void AddPricesFilters(SearchPropertiesQuery query, ICollection<FilterDefinition<PropertyEntity>> filters)
    {
        List<FilterDefinition<PropertyEntity>> rentFilterDefinitions = BuildRentFilterDefinitions(query);
        
        IEnumerable<FilterDefinition<PropertyEntity>> saleFilterDefinitions = BuildSaleFilterDefinitions(query);
        rentFilterDefinitions.Add(Builders<PropertyEntity>.Filter.Or(saleFilterDefinitions));
        filters.Add(Builders<PropertyEntity>.Filter.And(rentFilterDefinitions));
    }

    private static IEnumerable<FilterDefinition<PropertyEntity>> BuildSaleFilterDefinitions(SearchPropertiesQuery query)
    {
        List<FilterDefinition<PropertyEntity>> saleFilterDefinitions = new List<FilterDefinition<PropertyEntity>>();
        
        FilterHelper.AddGreaterOrEqualFilter(saleFilterDefinitions, document => document.SellingPrice, query.Prices.SellingPrice.From);
        AddToFiltersIfLessOrThanDefaultValue(saleFilterDefinitions, document => document.SellingPrice, query.Prices.SellingPrice.To, DefaultMaxPrice);
        
        return saleFilterDefinitions;
    }

    private static List<FilterDefinition<PropertyEntity>> BuildRentFilterDefinitions(SearchPropertiesQuery query)
    {
        List<FilterDefinition<PropertyEntity>> rentFilterDefinitions = new List<FilterDefinition<PropertyEntity>>();
        
        FilterHelper.AddGreaterOrEqualFilter(rentFilterDefinitions, document => document.RentalTotalPrice, query.Prices.RentalTotalPrice.From);
        AddToFiltersIfLessOrThanDefaultValue(rentFilterDefinitions, document => document.RentalTotalPrice, query.Prices.RentalTotalPrice.To, DefaultMaxPrice);

        return rentFilterDefinitions;
    }
    
    private static void AddToFiltersIfLessOrThanDefaultValue<T>(List<FilterDefinition<T>> filterDefinitions, Expression<Func<T, object>> expression, float value, float defaultValue)
    {
        if ( value <= 0 || value >= defaultValue)
        {
            FilterHelper.AddLessOrEqualFilter(filterDefinitions, expression, defaultValue);
            return;
        }

        FilterHelper.AddLessOrEqualFilter(filterDefinitions, expression, value);
    } 
}