using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;
using Core.Domains.Properties.UseCases;
using Core.Exceptions;
using Moq;
using UnitTest.Core.Commons;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.Core.Domains.Properties.UseCases;

[ExcludeFromCodeCoverage]
public sealed class GetPropertyByIdUseCaseTest : IDisposable
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly Mock<IGetPropertyByIdGateway> _gateway;
    private readonly IGetPropertyByIdUseCase _useCase;

    public GetPropertyByIdUseCaseTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
        _gateway = new Mock<IGetPropertyByIdGateway>(MockBehavior.Strict);
        ServiceResolverMocker serviceResolverMocker = new ServiceResolverMocker()
            .AddService(_gateway.Object);

        _useCase = new GetPropertyByIdUseCase(serviceResolverMocker.Object);
    }

    private bool _disposed;

    ~GetPropertyByIdUseCaseTest() => Dispose(false);

    public void Dispose()
    {
        _gateway.VerifyAll();
        Dispose(true);
        _outputHelper.WriteLine($"{nameof(GetPropertyByIdUseCaseTest)} disposed.");
        GC.SuppressFinalize(this);
    }

    [ExcludeFromCodeCoverage]
    private void Dispose(bool disposing)
    {
        if (_disposed || !disposing) return;
        _disposed = true;
    }

    [Trait("Category", "Core Business Tests")]
    [Fact(DisplayName = "Fail to get property when property not found")]
    public async Task FailToGetPropertyWhenPropertyNotFound()
    {
        // Arrange
        _gateway.Setup(gateway => gateway.ExecuteAsync(new GetPropertyByIdQuery("1001")))
            .ReturnsAsync(Optional<Property>.Empty());

        // Act
        NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            _useCase.ExecuteAsync(new GetPropertyByIdQuery("1001")));

        // Assert
        Assert.Equal("ValidationPropertyNotFound", exception.Message);
        Assert.Equal("1001", exception.Parameters[0]);
    }

    [Trait("Category", "Core Business Tests")]
    [Fact(DisplayName = "Successfully get property when property found")]
    public async Task SuccessfullyGetPropertyWhenPropertyFound()
    {
        // Arrange
        _gateway.Setup(gateway => gateway.ExecuteAsync(new GetPropertyByIdQuery("1001")))
            .ReturnsAsync(Optional<Property>.Of(
                    new Property
                    {
                        Id = 1001,
                        Type = PropertyType.House,
                        Advertise = new PropertyAdvertise
                        {
                            TenantId = 1,
                            Transaction = Transaction.Rent,
                            Title = "Title 1",
                            Description = "Description 1",
                            Url = "https://www.patolar.com.com/rent/1",
                            RefId = "Ref 001",
                            Images = new List<string>
                            {
                                "https://www.patolar.com.com/rent/1/image1.jpg",
                                "https://www.patolar.com.com/rent/1/image2.jpg",
                                "https://www.patolar.com.com/rent/1/image3.jpg",
                            },
                        },
                        Attributes = new PropertyAttributes
                        {
                            NumberOfBedrooms = 3,
                            NumberOfToilets = 2,
                            NumberOfGarages = 1,
                            Area = 100,
                            BuiltArea = 80,
                        },
                        Location = new PropertyLocation
                        {
                            City = "City 1",
                            State = "State 1",
                            District = "District 1",
                            Address = "Address 1"
                        },
                        Prices = new PropertyPrices
                        {
                            SellingPrice = 900000,
                            RentalTotalPrice = 9000,
                            RentalPrice = 10000,
                            Discount = 2000,
                            CondominiumFee = 1000,
                            PriceByM2 = 9000,
                        },
                        HashKey = "HashKey 1",
                        Ranking = 1,
                        Status = PropertyStatus.Active,
                        CreatedAt = new DateTime(2021, 4, 23, 10, 0, 1, DateTimeKind.Local),
                        UpdatedAt = new DateTime(2021, 4, 23, 10, 0, 1, DateTimeKind.Local)
                    }
                )
            );

        // Act
        Property actual = await _useCase.ExecuteAsync(new GetPropertyByIdQuery("1001"));

        // Assert
        Assert.NotNull(actual);
        
        Assert.Equal(1001, actual.Id);
        Assert.Equal(PropertyType.House, actual.Type);
        Assert.Equal("Title 1", actual.Advertise.Title);
        Assert.Equal("Description 1", actual.Advertise.Description);
        Assert.Equal("https://www.patolar.com.com/rent/1", actual.Advertise.Url);
        Assert.Equal("Ref 001", actual.Advertise.RefId);
        Assert.Equal("https://www.patolar.com.com/rent/1/image1.jpg", actual.Advertise.Images[0]);
        Assert.Equal("https://www.patolar.com.com/rent/1/image2.jpg", actual.Advertise.Images[1]);
        Assert.Equal("https://www.patolar.com.com/rent/1/image3.jpg", actual.Advertise.Images[2]);
        Assert.Equal(3, actual.Attributes.NumberOfBedrooms);
        Assert.Equal(2, actual.Attributes.NumberOfToilets);
        Assert.Equal(1, actual.Attributes.NumberOfGarages);
        Assert.Equal(100, actual.Attributes.Area);
        Assert.Equal(80, actual.Attributes.BuiltArea);
        Assert.Equal("City 1", actual.Location.City);
        Assert.Equal("State 1", actual.Location.State);
        Assert.Equal("District 1", actual.Location.District);
        Assert.Equal("Address 1", actual.Location.Address);
        Assert.Equal(900000, actual.Prices.SellingPrice);
        Assert.Equal(9000, actual.Prices.RentalTotalPrice);
        Assert.Equal(10000, actual.Prices.RentalPrice);
        Assert.Equal(2000, actual.Prices.Discount);
        Assert.Equal(1000, actual.Prices.CondominiumFee);
        Assert.Equal(9000, actual.Prices.PriceByM2);
        Assert.Equal("HashKey 1", actual.HashKey);
        Assert.Equal(1, actual.Ranking);
        Assert.Equal(PropertyStatus.Active, actual.Status);
        
        Assert.Equal(new DateTime(2021, 4, 23, 10, 0, 1, DateTimeKind.Local), actual.CreatedAt);
        Assert.Equal(new DateTime(2021, 4, 23, 10, 0, 1, DateTimeKind.Utc), actual.UpdatedAt);
    }
}