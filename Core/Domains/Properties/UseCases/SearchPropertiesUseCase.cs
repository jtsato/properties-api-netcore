using System.Threading.Tasks;
using Core.Commons;
using Core.Commons.Paging;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;

namespace Core.Domains.Properties.UseCases;

public sealed class SearchPropertiesUseCase : ISearchPropertiesUseCase
{
    private readonly ISearchPropertiesGateway _searchPropertiesGateway;
    
    public SearchPropertiesUseCase(IServiceResolver serviceResolver)
    {
        ArgumentValidator.CheckNull(serviceResolver, nameof(serviceResolver));
        _searchPropertiesGateway = serviceResolver.Resolve<ISearchPropertiesGateway>();
    }
    
    public async Task<Page<Property>> ExecuteAsync(SearchPropertiesQuery query, PageRequest pageRequest)
    {
        return await _searchPropertiesGateway.ExecuteAsync(query, pageRequest);
    }
}