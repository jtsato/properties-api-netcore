using System.Threading;
using System.Threading.Tasks;
using Core.Commons;
using EntryPoint.WebApi.Commons;
using EntryPoint.WebApi.Commons.Models;
using EntryPoint.WebApi.Domains.Properties.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EntryPoint.WebApi.Domains.Properties.EntryPoints;

[ApiController]
[Route("api/v1/properties")]
[ApiExplorerSettings(GroupName = "Properties")]
[Consumes("application/json")]
[Produces("application/json")]
public class SearchPropertiesApiMethod : IApiMethod
{
    private readonly SearchPropertiesController _controller;
    
    public SearchPropertiesApiMethod(SearchPropertiesController controller)
    {
        _controller = ArgumentValidator.CheckNull(controller, nameof(controller));
    }
    
    [SwaggerOperation(
        OperationId = nameof(SearchProperties),
        Tags = new[] { "Properties" },
        Summary = "Search properties"
    )]
    [ProducesResponseType(typeof(PageableSearchPropertiesResponse), 200)]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(PageableSearchPropertiesResponse), 206)]
    [ProducesResponseType(typeof(ResponseStatus), 400)]
    [ProducesResponseType(typeof(ResponseStatus), 500)]
    [HttpGet]
    public Task<IActionResult> SearchProperties(SearchPropertiesRequest request, QPageRequest qPageRequest, CancellationToken cancellationToken = default)  
    {
        return _controller.ExecuteAsync(request, qPageRequest);
    }
}