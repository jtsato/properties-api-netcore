using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Core.Commons.Paging;
using Core.Domains.Properties.Gateways;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;
using Core.Domains.Properties.UseCases;
using Moq;
using UnitTest.Core.Commons;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.Core.Domains.Properties.UseCases;

public sealed class SearchPropertiesUseCaseTest : IDisposable
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly Mock<ISearchPropertiesGateway> _gateway;
    private readonly ISearchPropertiesUseCase _useCase;

    public SearchPropertiesUseCaseTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
        _gateway = new Mock<ISearchPropertiesGateway>(MockBehavior.Strict);
        ServiceResolverMocker serviceResolverMocker = new ServiceResolverMocker()
            .AddService(_gateway.Object);

        _useCase = new SearchPropertiesUseCase(serviceResolverMocker.Object);
    }

    private bool _disposed;

    ~SearchPropertiesUseCaseTest() => Dispose(false);

    public void Dispose()
    {
        _gateway.VerifyAll();
        Dispose(true);
        _outputHelper.WriteLine($"{nameof(SearchPropertiesUseCaseTest)} disposed.");
        GC.SuppressFinalize(this);
    }

    [ExcludeFromCodeCoverage]
    private void Dispose(bool disposing)
    {
        if (_disposed || !disposing) return;
        _disposed = true;
    }

    [Trait("Category", "Core Business Tests")]
    [Fact(DisplayName = "Search properties with valid parameters")]
    public async Task SearchPropertiesWithValidParameters()
    {
        // Arrange
        SearchPropertiesQueryBuilder searchPropertiesQueryBuilder = new SearchPropertiesQueryBuilder();
        searchPropertiesQueryBuilder.WithTransaction("Rent");
        searchPropertiesQueryBuilder.WithType("All");
        SearchPropertiesQuery query = searchPropertiesQueryBuilder.Build();
        
        _gateway
            .Setup(gateway => gateway.ExecuteAsync(query, PageRequest.Of(0, 10, Sort.Unsorted)))
            .ReturnsAsync(new Page<Property>(
                new List<Property>
                {
                    new Property
                    {
                        Id = 1001,
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
                    },
                }, new Pageable(0, 10, 1, 1, 1)
            ));

        PageRequest pageRequest = PageRequest.Of(0, 10, Sort.Unsorted);

        // Act
        Page<Property> page = await _useCase.ExecuteAsync(query, pageRequest);
        
        // Assert
        Assert.NotNull(page);
        Assert.Equal(1, page.Pageable.NumberOfElements);
        Assert.Equal(1, page.Pageable.TotalPages);
        Assert.Equal(1, page.Pageable.NumberOfElements);
        Assert.Equal(0, page.Pageable.Page);
        Assert.Equal(10, page.Pageable.Size);

        Assert.Single(page.Content);
        
        Property property = page.Content[0];
        Assert.NotNull(property);
        
        Assert.Equal(1001, property.Id);
        Assert.Equal("Title 1", property.Advertise.Title);
        Assert.Equal("Description 1", property.Advertise.Description);
        Assert.Equal("https://www.patolar.com.com/rent/1", property.Advertise.Url);
        Assert.Equal("Ref 001", property.Advertise.RefId);
        Assert.Equal("https://www.patolar.com.com/rent/1/image1.jpg", property.Advertise.Images[0]);
        Assert.Equal("https://www.patolar.com.com/rent/1/image2.jpg", property.Advertise.Images[1]);
        Assert.Equal("https://www.patolar.com.com/rent/1/image3.jpg", property.Advertise.Images[2]);
        Assert.Equal(3, property.Attributes.NumberOfBedrooms);
        Assert.Equal(2, property.Attributes.NumberOfToilets);
        Assert.Equal(1, property.Attributes.NumberOfGarages);
        Assert.Equal(100, property.Attributes.Area);
        Assert.Equal(80, property.Attributes.BuiltArea);
        Assert.Equal("City 1", property.Location.City);
        Assert.Equal("State 1", property.Location.State);
        Assert.Equal("District 1", property.Location.District);
        Assert.Equal("Address 1", property.Location.Address);
        Assert.Equal(900000, property.Prices.SellingPrice);
        Assert.Equal(9000, property.Prices.RentalTotalPrice);
        Assert.Equal(10000, property.Prices.RentalPrice);
        Assert.Equal(2000, property.Prices.Discount);
        Assert.Equal(1000, property.Prices.CondominiumFee);
        Assert.Equal(9000, property.Prices.PriceByM2);
        Assert.Equal("HashKey 1", property.HashKey);
        Assert.Equal(1, property.Ranking);
        Assert.Equal(PropertyStatus.Active, property.Status);
        
        Assert.Equal(new DateTime(2021, 4, 23, 10, 0, 1, DateTimeKind.Local), property.CreatedAt);
        Assert.Equal(new DateTime(2021, 4, 23, 10, 0, 1, DateTimeKind.Utc), property.UpdatedAt);
    }
}