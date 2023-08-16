
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Commons;
using Core.Commons.Paging;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;
using Core.Domains.Properties.UseCases;
using Google.Cloud.Firestore;
using Infra.Firestore.Commons.Repository;
using Infra.Firestore.Domain.Properties.Mapper;
using Infra.Firestore.Domain.Properties.Model;

namespace Infra.Firestore.Domain.Properties.Providers;

public class SearchPropertiesProvider : ISearchPropertiesGateway
{
    private readonly IRepository<PropertyEntity> _repository;

    public SearchPropertiesProvider(IRepository<PropertyEntity> repository)
    {
        _repository = ArgumentValidator.CheckNull(repository, nameof(repository));
    }

    public async Task<Page<Property>> ExecuteAsync(SearchPropertiesQuery query, PageRequest pageRequest)
    {
        IEnumerable<Filter> filters = SearchPropertiesFilterBuilder.BuildFromQuery(query);
        Page<PropertyEntity> page = await _repository.FindAllAsync(filters, pageRequest);
        List<Property> content = page.Content.Select(PropertyMapper.Of).ToList();
        return new Page<Property>(content, page.Pageable);
    }
}