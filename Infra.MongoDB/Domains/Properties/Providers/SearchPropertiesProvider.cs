using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Commons;
using Core.Commons.Paging;
using Core.Domains.Properties.Gateways;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;
using Infra.MongoDB.Commons.Helpers;
using Infra.MongoDB.Commons.Repository;
using Infra.MongoDB.Domains.Properties.Mapper;
using Infra.MongoDB.Domains.Properties.Model;
using Infra.MongoDB.Domains.Properties.Repository;
using MongoDB.Driver;

namespace Infra.MongoDB.Domains.Properties.Providers;

public class SearchPropertiesProvider : ISearchPropertiesGateway
{
    private const string DefaultPrimarySortField = "ranking";
    private const string DefaultSecondarySortField = "updatedAt";

    private readonly IRepository<PropertyEntity> _propertyRepository;

    public SearchPropertiesProvider(IRepository<PropertyEntity> propertyRepository)
    {
        _propertyRepository = ArgumentValidator.CheckNull(propertyRepository, nameof(propertyRepository));
    }

    public async Task<Page<Property>> ExecuteAsync(SearchPropertiesQuery query, PageRequest pageRequest)
    {
        FilterDefinition<PropertyEntity> filter = SearchPropertiesFilterBuilder.Build(query);
        SortDefinition<PropertyEntity> sort = GetSortDefinitions(pageRequest.Sort.GetOrders());
        Page<PropertyEntity> page = await _propertyRepository.FindAllAsync(filter, sort, pageRequest.PageNumber, pageRequest.PageSize);
        List<Property> content = page.Content.Select(PropertyMapper.Map).ToList();

        return new Page<Property>(content, page.Pageable);
    }

    private static SortDefinition<PropertyEntity> GetSortDefinitions(IEnumerable<Order> originalOrders)
    {
        IEnumerable<Order> arrayOfOrders = originalOrders.ToArray();
        List<Order> orders = new List<Order>(arrayOfOrders);

        if (!arrayOfOrders.Any())
        {
            orders.Insert(0, new Order(Direction.Desc, DefaultSecondarySortField));
        }

        orders.Insert(0, new Order(Direction.Desc, DefaultPrimarySortField));

        return SortHelper.GetSortDefinitions<PropertyEntity>(orders);
    }
}