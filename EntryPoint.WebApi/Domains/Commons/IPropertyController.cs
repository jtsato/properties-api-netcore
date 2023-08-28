using System.Threading.Tasks;
using EntryPoint.WebApi.Commons.Controllers;
using EntryPoint.WebApi.Commons.Models;
using EntryPoint.WebApi.Domains.Properties.Models;
using Microsoft.AspNetCore.Mvc;

namespace EntryPoint.WebApi.Domains.Commons;

public interface IPropertyController : IController
{
}

public interface ISearchPropertiesController : IPropertyController
{
    Task<IActionResult> ExecuteAsync(SearchPropertiesRequest request, QPageRequest qPageRequest);
}

public interface IGetPropertyByIdController : IPropertyController
{
    Task<IActionResult> ExecuteAsync(string id);
}
