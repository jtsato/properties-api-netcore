using System.Threading.Tasks;
using Core.Commons.Paging;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;

namespace Core.Domains.Properties.UseCases;

public interface ISearchPropertiesGateway
{
    Task<Page<Property>> ExecuteAsync(SearchPropertiesQuery query, PageRequest pageRequest);
}