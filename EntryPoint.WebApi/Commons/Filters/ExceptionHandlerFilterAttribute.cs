using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Core.Commons;
using Core.Exceptions;
using EntryPoint.WebApi.Commons.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace EntryPoint.WebApi.Commons.Filters;

[ExcludeFromCodeCoverage]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class ExceptionHandlerFilterAttribute : ExceptionFilterAttribute
{
    private readonly List<Type> _businessExceptions = new List<Type>
    {
        typeof(AccessDeniedException),
        typeof(InvalidArgumentException),
        typeof(NotFoundException),
        typeof(ParentConstraintException),
        typeof(UniqueConstraintException),
        typeof(ValidationException)
    };

    private readonly IExceptionHandler _exceptionHandler;
    private readonly ILoggerAdapter _logger;

    public ExceptionHandlerFilterAttribute(IExceptionHandler exceptionHandler, ILoggerAdapter logger)
    {
        _exceptionHandler = ArgumentValidator.CheckNull(exceptionHandler, nameof(exceptionHandler));
        _logger = ArgumentValidator.CheckNull(logger, nameof(logger));
    }

    public override async Task OnExceptionAsync(ExceptionContext context)
    {
        string correlationId = GetCorrelationId(context);
        context.HttpContext.Response.Headers.Add(CorrelationIdHeaderReader.HeaderName, correlationId);
        LogException(correlationId, context.Exception);
        context.Result = await _exceptionHandler.HandleAsync(context.Exception);
        context.ExceptionHandled = true;
    }

    private void LogException(string correlationId, Exception exception)
    {
        if (_businessExceptions.Contains(exception.GetType()))
        {
            if (!_logger.IsEnabled(LogLevel.Warning)) return;
            _logger.LogWarning("{CorrelationIdHeader}: {CorrelationId}, {Exception}: {Message}", CorrelationIdHeaderReader.HeaderName, correlationId, exception.GetType(), exception.Message);
            return;
        }

        if (!_logger.IsEnabled(LogLevel.Error)) return;
        _logger.LogError("{CorrelationIdHeader}: {CorrelationId}, {Exception}: {Message}", CorrelationIdHeaderReader.HeaderName, correlationId, exception.GetType(), exception.Message);
    }

    private static string GetCorrelationId(ActionContext context)
    {
        Optional<string> optional = CorrelationIdHeaderReader.TryGetFromHeader(context.HttpContext);
        return optional.OrElse(Guid.NewGuid().ToString());
    }
}