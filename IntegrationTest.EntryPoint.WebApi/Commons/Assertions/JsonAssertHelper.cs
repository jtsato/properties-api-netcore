using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Core.Commons;
using Core.Commons.Extensions;
using Xunit;

namespace IntegrationTest.EntryPoint.WebApi.Commons.Assertions;

[ExcludeFromCodeCoverage]
[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
public class JsonAssertHelper
{
    private readonly JsonElement _jsonElement;

    private JsonAssertHelper(JsonElement jsonElement)
    {
        _jsonElement = jsonElement;
    }

    public static JsonAssertHelper AssertThat(JsonElement jsonElement)
    {
        return new JsonAssertHelper(jsonElement);
    }

    public JsonAssertHelper AndExpectThat<T>(JsonFrom jsonFrom, Is<T> matcher)
    {
        return matcher.AssertType switch
        {
            AssertType.Equal => Equal(jsonFrom.JsonPath, matcher.Excepted),
            AssertType.Empty => Empty(jsonFrom.JsonPath),
            AssertType.NotEmpty => NotEmpty(jsonFrom.JsonPath),
            AssertType.Single => Single(jsonFrom.JsonPath),
            AssertType.Count => Count(jsonFrom.JsonPath, Convert.ToInt32(matcher.Excepted)),
            AssertType.Null => Null(jsonFrom.JsonPath),
            _ => throw new InvalidOperationException()
        };
    }

    private JsonAssertHelper Equal<T>(string path, T excepted)
    {
        try
        {
            ArgumentValidator.CheckNull(_jsonElement.SelectToken(path), path);
            Assert.Equal(excepted, _jsonElement.SelectToken(path).ToObject<T>());
            return this;
        }
        catch (Exception exception)
        {
            throw CreateAssertionException(exception);
        }
    }

    private JsonAssertHelper Empty(string path)
    {
        try
        {
            ArgumentValidator.CheckNull(_jsonElement.SelectToken(path), path);
            Assert.Empty(_jsonElement.SelectToken(path).EnumerateArray());
            return this;
        }
        catch (Exception exception)
        {
            throw CreateAssertionException(exception);
        }
    }

    private JsonAssertHelper NotEmpty(string path)
    {
        try
        {
            ArgumentValidator.CheckNull(_jsonElement.SelectToken(path), path);
            Assert.NotEmpty(_jsonElement.SelectToken(path).EnumerateArray());
            return this;
        }
        catch (Exception exception)
        {
            throw CreateAssertionException(exception);
        }
    }

    private JsonAssertHelper Single(string path)
    {
        try
        {
            ArgumentValidator.CheckNull(_jsonElement.SelectToken(path), path);
            Assert.Single(_jsonElement.SelectToken(path).EnumerateArray());
            return this;
        }
        catch (Exception exception)
        {
            throw CreateAssertionException(exception);
        }
    }

    private JsonAssertHelper Count(string path, int excepted)
    {
        try
        {
            ArgumentValidator.CheckNull(_jsonElement.SelectToken(path), path);
            Assert.Equal(excepted, _jsonElement.SelectToken(path).GetArrayLength());
            return this;
        }
        catch (Exception exception)
        {
            throw CreateAssertionException(exception);
        }
    }

    private JsonAssertHelper Null(string path)
    {
        try
        {
            ArgumentValidator.CheckNull(_jsonElement.SelectToken(path), path);
            Assert.True(_jsonElement.SelectToken(path).ValueKind == JsonValueKind.Null);
            return this;
        }
        catch (Exception exception)
        {
            throw CreateAssertionException(exception);
        }
    }

    private static AssertionException CreateAssertionException(Exception exception, string message = null)
    {
        StackTrace stackTrace = new StackTrace(3, true);
        exception.SetStackTrace(stackTrace);
        return new AssertionException(exception.StackTrace, message ?? exception.Message);
    }
}