using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.Properties.Gateways;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;
using Core.Exceptions;

namespace Core.Domains.Properties.UseCases;

public class GetPropertyByIdUseCase : IGetPropertyByIdUseCase
{
    private readonly IGetPropertyByIdGateway _getPropertyByIdGateway;
    
    public GetPropertyByIdUseCase(IServiceResolver serviceResolver)
    {
        ArgumentValidator.CheckNull(serviceResolver, nameof(serviceResolver));
        _getPropertyByIdGateway = serviceResolver.Resolve<IGetPropertyByIdGateway>();
    }
    
    public async Task<Property> ExecuteAsync(GetPropertyByIdQuery query)
    {
        Optional<Property> optional = await _getPropertyByIdGateway.ExecuteAsync(query);
        return optional.OrElseThrow(() => new NotFoundException("ValidationPropertyNotFound", query.Id));
    }
}