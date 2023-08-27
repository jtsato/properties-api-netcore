using System.Threading.Tasks;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;

namespace Core.Domains.Properties.UseCases;

public interface IGetPropertyByIdUseCase
{
    Task<Property> ExecuteAsync(GetPropertyByIdQuery query);
}
