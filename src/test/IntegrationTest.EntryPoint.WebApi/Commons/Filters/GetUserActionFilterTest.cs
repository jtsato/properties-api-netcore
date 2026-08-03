using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Exceptions;
using EntryPoint.WebApi.Commons.Filters;
using EntryPoint.WebApi.Commons.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace IntegrationTest.EntryPoint.WebApi.Commons.Filters;

[Collection("WebApi Collection [NoContext]")]
public sealed class GetUserActionFilterTest
{
    [Trait("Category", "WebApi Collection [NoContext]")]
    [Theory(DisplayName = "Fail to get user when the authorization header is missing or malformed")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("Bearer")]
    [InlineData("Basic dXNlcjpwYXNzd29yZA==")]
    public void FailToGetUserWhenTheAuthorizationHeaderIsMissingOrMalformed(string authorization)
    {
        // Arrange
        WebRequest webRequest = new WebRequest();
        GetUserActionFilter getUserActionFilter = new GetUserActionFilter(webRequest);

        ActionExecutingContext actionExecutingContext = BuildActionExecutingContext(authorization);

        // Act
        AccessDeniedException exception = Assert.Throws<AccessDeniedException>(() => getUserActionFilter.OnActionExecuting(actionExecutingContext));

        // Assert
        Assert.Equal("CommonAccessDeniedException", exception.Message);
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to extract the user from a valid bearer token")]
    public void SuccessfulToExtractTheUserFromAValidBearerToken()
    {
        // Arrange
        string token = BuildToken(
        [
            new Claim("clientuid", "77c1c391-b488-488e-b312-652fa086b694"),
            new Claim("username", "duck.lover"),
            new Claim("email", "duck.lover@white-duck.com")
        ]);

        WebRequest webRequest = new WebRequest();
        GetUserActionFilter getUserActionFilter = new GetUserActionFilter(webRequest);

        ActionExecutingContext actionExecutingContext = BuildActionExecutingContext($"Bearer {token}");

        // Act
        getUserActionFilter.OnActionExecuting(actionExecutingContext);

        // Assert
        Assert.Equal("77c1c391-b488-488e-b312-652fa086b694", webRequest.ClientUid);
        Assert.Equal("duck.lover", webRequest.Username);
        Assert.Equal("duck.lover@white-duck.com", webRequest.Email);
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to extract the user from a valid bearer token even when claims are missing")]
    public void SuccessfulToExtractTheUserFromAValidBearerTokenEvenWhenClaimsAreMissing()
    {
        // Arrange
        string token = BuildToken([]);

        WebRequest webRequest = new WebRequest();
        GetUserActionFilter getUserActionFilter = new GetUserActionFilter(webRequest);

        ActionExecutingContext actionExecutingContext = BuildActionExecutingContext($"Bearer {token}");

        // Act
        getUserActionFilter.OnActionExecuting(actionExecutingContext);

        // Assert
        Assert.Null(webRequest.ClientUid);
        Assert.Null(webRequest.Username);
        Assert.Null(webRequest.Email);
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to do nothing when the action is executed")]
    public void SuccessfulToDoNothingWhenTheActionIsExecuted()
    {
        // Arrange
        GetUserActionFilter getUserActionFilter = new GetUserActionFilter(new WebRequest());

        // Act
        Exception exception = Record.Exception(() => getUserActionFilter.OnActionExecuted(null));

        // Assert
        Assert.Null(exception);
    }

    private static string BuildToken(List<Claim> claims)
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(claims: claims);
        return handler.WriteToken(jwtSecurityToken);
    }

    private static ActionExecutingContext BuildActionExecutingContext(string authorization)
    {
        DefaultHttpContext httpContext = new DefaultHttpContext();

        if (authorization != null)
        {
            httpContext.Request.Headers["authorization"] = new StringValues(authorization);
        }

        ActionContext actionContext = new ActionContext
        {
            HttpContext = httpContext,
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };

        return new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object>(),
            Mock.Of<Controller>()
        );
    }
}
