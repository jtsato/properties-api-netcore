﻿using System.Net;
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

public class GetPropertyByIdController : IGetPropertyByIdController
{
    private readonly IGetPropertyByIdUseCase _useCase;
    
    public GetPropertyByIdController(IGetPropertyByIdUseCase useCase)
    {
        _useCase = useCase;
    }
    
    public async Task<IActionResult> ExecuteAsync(string id)
    {
        GetPropertyByIdQuery query = new GetPropertyByIdQuery(id);
        Property property = await _useCase.ExecuteAsync(query);
        PropertyResponse propertyResponse = PropertyPresenter.Of(property);
        return await ResponseBuilder.BuildResponse(HttpStatusCode.OK, propertyResponse);
    }
}
