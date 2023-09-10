namespace IntegrationTest.EntryPoint.WebApi.Commons;

using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Xunit.Sdk;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class UseCultureAttribute : BeforeAfterTestAttribute
{
    private readonly Lazy<CultureInfo> _culture;
    private readonly Lazy<CultureInfo> _uiCulture;

    private CultureInfo _originalCulture;
    private CultureInfo _originalUiCulture;

    private CultureInfo Culture => _culture.Value;
    private CultureInfo UiCulture => _uiCulture.Value;

    public UseCultureAttribute(string culture) : this(culture, culture) { }

    public UseCultureAttribute(string culture, string uiCulture)
    {
        _culture = new Lazy<CultureInfo>(() => new CultureInfo(culture, false));
        _uiCulture = new Lazy<CultureInfo>(() => new CultureInfo(uiCulture, false));
    }

    public override void Before(MethodInfo methodUnderTest)
    {
        _originalCulture = Thread.CurrentThread.CurrentCulture;
        _originalUiCulture = Thread.CurrentThread.CurrentUICulture;
        
        CultureInfo.DefaultThreadCurrentCulture = Culture;
        CultureInfo.DefaultThreadCurrentUICulture = UiCulture;

        Thread.CurrentThread.CurrentCulture = Culture;
        Thread.CurrentThread.CurrentUICulture = UiCulture;

        CultureInfo.CurrentCulture.ClearCachedData();
        CultureInfo.CurrentUICulture.ClearCachedData();
    }

    public override void After(MethodInfo methodUnderTest)
    {
        Thread.CurrentThread.CurrentCulture = _originalCulture;
        Thread.CurrentThread.CurrentUICulture = _originalUiCulture;

        CultureInfo.CurrentCulture.ClearCachedData();
        CultureInfo.CurrentUICulture.ClearCachedData();
    }
}