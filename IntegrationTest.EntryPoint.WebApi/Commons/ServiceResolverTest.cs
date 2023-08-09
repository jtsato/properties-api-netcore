using System;
using System.Diagnostics.CodeAnalysis;
using EntryPoint.WebApi.Commons;
using Moq;
using Xunit;

namespace IntegrationTest.EntryPoint.WebApi.Commons;

[Collection("WebApi Collection [NoContext]")]
public class ServiceResolverTest
{
    private readonly Mock<IServiceProvider> _serviceProvider;

    public ServiceResolverTest()
    {
        _serviceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
    }

    [Trait("Category", "Entrypoint (WebApi) Integration tests")]
    [Fact(DisplayName = "Successful to resolve service")]
    public void SuccessfulToResolveService()
    {
        _serviceProvider
            .Setup(self => self.GetService(typeof(DummyClass)))
            .Returns(new DummyClass("Black", "White"));

        ServiceResolver serviceResolver = new ServiceResolver();
        serviceResolver.Setup(_serviceProvider.Object);

        DummyClass dummyClass = serviceResolver.Resolve<DummyClass>();

        Assert.Equal(new DummyClass("Black", "White"), dummyClass);
    }

    [Trait("Category", "Entrypoint (WebApi) Integration tests")]
    [Fact(DisplayName = "Fail to resolve service")]
    public void FailToResolveService()
    {
        _serviceProvider
            .Setup(self => self.GetService(typeof(DummyClass)))
            .Returns(null);

        ServiceResolver serviceResolver = new ServiceResolver();
        serviceResolver.Setup(_serviceProvider.Object);

        Assert.Throws<ArgumentNullException>(() => serviceResolver.Resolve<DummyClass>());
    }

    [ExcludeFromCodeCoverage]
    private sealed class DummyClass
    {
        public string Foo { get; }
        public string Bar { get; }

        public DummyClass(string foo, string bar)
        {
            Foo = foo;
            Bar = bar;
        }

        private bool Equals(DummyClass other)
        {
            return Foo == other.Foo && Bar == other.Bar;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is DummyClass other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Foo, Bar);
        }

        public override string ToString()
        {
            return $"{nameof(Foo)}: {Foo}, {nameof(Bar)}: {Bar}";
        }
    }
}