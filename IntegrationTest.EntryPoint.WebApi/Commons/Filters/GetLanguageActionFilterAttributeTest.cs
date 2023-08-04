using System.Threading;
using EntryPoint.WebApi.Commons.Filters;
using Xunit;

namespace IntegrationTest.EntryPoint.WebApi.Commons.Filters;

[Collection("HttpTrigger collection [NoContext]")]
public class GetLanguageActionFilterAttributeTest
{
    [Trait("Category", "Entrypoint (WebApi) Integration tests")]
    [Theory(DisplayName = "Successful to handle multiple cultures")]
    [InlineData("pt-br", "pt-BR")]
    [InlineData("en-us", "en-US")]
    [InlineData("pt-us", "en-US")]
    [InlineData("", "en-US")]
    [InlineData(null, "en-US")]
    public void SuccessfulToHandleMultipleCultures(string language, string expected)
    {
        GetLanguageActionFilterAttribute.SetupLanguage(language);
        Assert.Equal(Thread.CurrentThread.CurrentCulture.Name, expected);
    }
}