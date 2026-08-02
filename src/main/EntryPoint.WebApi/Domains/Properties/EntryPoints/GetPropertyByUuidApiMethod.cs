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
[Route("v1/properties")]
[ApiExplorerSettings(GroupName = "Properties")]
[Consumes("application/json")]
[Produces("application/json")]
public class GetPropertyByUuidApiMethod : IApiMethod
{
    private readonly IGetPropertyByUuidController _controller;

    public GetPropertyByUuidApiMethod(IGetPropertyByUuidController controller)
    {
        _controller = ArgumentValidator.CheckNull(controller, nameof(controller));
    }

    [SwaggerOperation(
        OperationId = nameof(GetPropertyByUuid),
        Tags = new[] {"Properties"},
        Summary = "Get property by universal unique identifier.",
        Description = "Get property by universal unique identifier."
    )]
    [ProducesResponseType(typeof(PropertyResponse), 200)]
    [ProducesResponseType(typeof(ResponseStatus), 400)]
    [ProducesResponseType(typeof(ResponseStatus), 500)]
    [HttpGet("{uuid}")]
    public Task<IActionResult> GetPropertyByUuid(string uuid)
    {
        return _controller.ExecuteAsync(uuid);
    }
}