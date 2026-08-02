using System.Threading.Tasks;
using Core.Domains.Properties.Models;

namespace Core.Domains.Properties.Gateways;

public interface IRegisterPropertyGateway
{
    Task<Property> ExecuteAsync(Property property);
}