using System.Threading.Tasks;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;

namespace Core.Domains.Properties.UseCases;

public interface IGetPropertyByUuidUseCase
{
    Task<Property> ExecuteAsync(GetPropertyByUuidQuery query);
}