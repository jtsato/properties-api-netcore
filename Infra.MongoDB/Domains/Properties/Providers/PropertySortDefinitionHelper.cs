using System.Collections.Generic;
using System.Linq;
using Core.Commons.Paging;
using Infra.MongoDB.Commons.Helpers;
using Infra.MongoDB.Domains.Properties.Model;
using MongoDB.Driver;

namespace Infra.MongoDB.Domains.Properties.Providers;

public static class PropertySortDefinitionHelper
{
    private const string DefaultPrimarySortField = "ranking";
    private const string DefaultSecondarySortField = "updatedAt";

    public static SortDefinition<PropertyEntity> GetSortDefinitions(IEnumerable<Order> originalOrders)
    {
        IEnumerable<Order> arrayOfOrders = originalOrders.ToArray();
        List<Order> orders = new List<Order>(arrayOfOrders);
        
        // We always add two more sorting criteria (ranking and updatedAt).

        // If the user has specified any sorting criteria:
        if (arrayOfOrders.Any())
        {
            // The properties sponsorship will always be a tie-breaking criterion
            // if there are multiple properties tied at previous levels of ordering.
            
            // Check if DefaultPrimarySortField is already in the list of orders.
            if (orders.TrueForAll(order => order.Property != DefaultPrimarySortField))
            {
                orders.Add(new Order(Direction.Desc, DefaultPrimarySortField));
            }
            
            // Check if DefaultSecondarySortField is already in the list of orders.
            if (orders.TrueForAll(order => order.Property != DefaultSecondarySortField))
            {
                orders.Add(new Order(Direction.Desc, DefaultSecondarySortField));
            }
            
            return SortHelper.GetSortDefinitions<PropertyEntity>(orders);
        }

        // If the user has not specified any sorting criteria:
        orders.Insert(0, new Order(Direction.Desc, DefaultSecondarySortField));
        orders.Insert(0, new Order(Direction.Desc, DefaultPrimarySortField));

        return SortHelper.GetSortDefinitions<PropertyEntity>(orders);
    }
}