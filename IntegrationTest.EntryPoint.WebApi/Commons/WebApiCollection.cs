﻿using Xunit;

namespace IntegrationTest.EntryPoint.WebApi.Commons;

[CollectionDefinition("WebApi Collection Context")]
public class WebApiCollection : ICollectionFixture<ApiMethodInvokerHolder>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}