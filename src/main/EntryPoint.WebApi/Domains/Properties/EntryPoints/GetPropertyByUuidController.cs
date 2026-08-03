using System.Net;
using System.Threading.Tasks;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;
using Core.Domains.Properties.UseCases;
using EntryPoint.WebApi.Commons.Controllers;
using EntryPoint.WebApi.Domains.Commons;
using EntryPoint.WebApi.Domains.Properties.Models;
using EntryPoint.WebApi.Domains.Properties.Presenters;
using Microsoft.AspNetCore.Mvc;

namespace EntryPoint.WebApi.Domains.Properties.EntryPoints;

public class GetPropertyByUuidController(IGetPropertyByUuidUseCase useCase) : IGetPropertyByUuidController
{

    public async Task<IActionResult> ExecuteAsync(string uuid)
    {
        GetPropertyByUuidQuery query = new GetPropertyByUuidQuery(uuid);
        Property property = await useCase.ExecuteAsync(query);
        PropertyResponse propertyResponse = PropertyPresenter.Of(property);
        return await ResponseBuilder.BuildResponse(HttpStatusCode.OK, propertyResponse);
    }
}