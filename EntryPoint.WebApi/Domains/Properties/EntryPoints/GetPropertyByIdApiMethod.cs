using System.Threading.Tasks;
using Core.Commons;
using EntryPoint.WebApi.Commons;
using EntryPoint.WebApi.Commons.Models;
using EntryPoint.WebApi.Domains.Commons;
using EntryPoint.WebApi.Domains.Properties.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EntryPoint.WebApi.Domains.Properties.EntryPoints;

[ApiController]
[Route("api/v1/properties")]
[ApiExplorerSettings(GroupName = "Properties")]
[Consumes("application/json")]
[Produces("application/json")]
public class GetPropertyByIdApiMethod : IApiMethod
{
    private readonly IGetPropertyByIdController _controller;
    
    public GetPropertyByIdApiMethod(IGetPropertyByIdController controller)
    {
        _controller = ArgumentValidator.CheckNull(controller, nameof(controller));
    }
    
    [SwaggerOperation(
        OperationId = nameof(GetPropertyById),
        Tags = new[] { "Properties" },
        Summary = "Get property by identifier.",
        Description = "Get property by identifier."
    )]
    [ProducesResponseType(typeof(PropertyResponse), 200)]
    [ProducesResponseType(typeof(ResponseStatus), 400)]
    [ProducesResponseType(typeof(ResponseStatus), 500)]
    [HttpGet("{id:long:min(1)}")]
    public Task<IActionResult> GetPropertyById(string id)
    {
        return _controller.ExecuteAsync(id);
    }
}