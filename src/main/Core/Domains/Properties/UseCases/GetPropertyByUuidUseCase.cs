using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.Properties.Gateways;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;
using Core.Exceptions;

namespace Core.Domains.Properties.UseCases;

public class GetPropertyByUuidUseCase : IGetPropertyByUuidUseCase
{
    private readonly IGetPropertyByUuidGateway _getPropertyByUuidGateway;

    public GetPropertyByUuidUseCase(IServiceResolver serviceResolver)
    {
        ArgumentValidator.CheckNull(serviceResolver, nameof(serviceResolver));
        _getPropertyByUuidGateway = serviceResolver.Resolve<IGetPropertyByUuidGateway>();
    }

    public async Task<Property> ExecuteAsync(GetPropertyByUuidQuery query)
    {
        Optional<Property> optional = await _getPropertyByUuidGateway.ExecuteAsync(query);
        return optional.OrElseThrow(() => new NotFoundException("ValidationPropertyNotFound", query.Uuid));
    }
}