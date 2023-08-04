using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Core.Commons;
using EntryPoint.WebApi.Commons.Exceptions;
using EntryPoint.WebApi.Commons.Filters;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationTest.EntryPoint.WebApi.Commons;

[ExcludeFromCodeCoverage]
public sealed class ApiMethodInvoker
{
    private readonly IExceptionHandler _exceptionHandler;

    public ApiMethodInvoker(IExceptionHandler exceptionHandler)
    {
        _exceptionHandler = ArgumentValidator.CheckNull(exceptionHandler, nameof(exceptionHandler));
    }

    public async Task<ObjectResult> InvokeAsync(Func<Task<IActionResult>> method, string cultureName = null)
    {
        try
        {
            GetLanguageActionFilterAttribute.SetupLanguage(cultureName);
            return (ObjectResult) await method.Invoke();
        }
        catch (Exception exception)
        {
            return (ObjectResult) await _exceptionHandler.HandleAsync(exception);
        }
    }
}