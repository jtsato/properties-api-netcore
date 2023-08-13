using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Xunit.Sdk;

namespace IntegrationTest.EntryPoint.WebApi.Commons;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class UseInvariantCultureAttribute : BeforeAfterTestAttribute
{
    private CultureInfo _originalCulture;
    private CultureInfo _originalUiCulture;

    public override void Before(MethodInfo methodUnderTest)
    {
        _originalCulture = CultureInfo.DefaultThreadCurrentCulture;
        _originalUiCulture = CultureInfo.DefaultThreadCurrentUICulture;
        
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
    }

    public override void After(MethodInfo methodUnderTest)
    {
        CultureInfo.DefaultThreadCurrentCulture = _originalCulture;
        CultureInfo.DefaultThreadCurrentUICulture = _originalUiCulture;

        if (_originalCulture is null) return;
        
        Thread.CurrentThread.CurrentCulture = _originalCulture;
        Thread.CurrentThread.CurrentUICulture = _originalUiCulture;
    }
}