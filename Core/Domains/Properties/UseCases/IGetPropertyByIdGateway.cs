using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;

namespace Core.Domains.Properties.UseCases;

public interface IGetPropertyByIdGateway
{
    Task<Optional<Property>> ExecuteAsync(GetPropertyByIdQuery query);
}