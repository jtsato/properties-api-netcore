using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Core.Commons;
using Core.Domains.Properties.Gateways;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;
using IntegrationTest.Infra.MongoDB.Commons;
using Xunit;
using Xunit.Abstractions;
using Optional = Core.Commons.Optional<Core.Domains.Properties.Models.Property>;

namespace IntegrationTest.Infra.MongoDB.Domains.Properties.Providers;

[Collection("Database collection")]
public class GetPropertyByIdProviderTest
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly IRegisterPropertyGateway _registerPropertyGateway;
    private readonly IGetPropertyByIdGateway _getPropertyByIdGateway;

    public GetPropertyByIdProviderTest(ITestOutputHelper outputHelper, Context context)
    {
        _outputHelper = outputHelper;
        _registerPropertyGateway = context.ServiceResolver.Resolve<IRegisterPropertyGateway>();
        _getPropertyByIdGateway = context.ServiceResolver.Resolve<IGetPropertyByIdGateway>();
    }

    [Trait("Category", "Infrastructure (DB) Integration tests")]
    [Fact(DisplayName = "Fail to get property by id")]
    public async Task FailToGetPropertyById()
    {
        // Arrange
        // Act
        Optional<Property> optional = await _getPropertyByIdGateway.ExecuteAsync(new GetPropertyByIdQuery("1001"));

        // Assert
        Assert.False(optional.HasValue());
    }

    [Trait("Category", "Infrastructure (DB) Integration tests")]
    [Fact(DisplayName = "Successful to get property by id")]
    public async Task SuccessfulToGetPropertyById()
    {
        // Arrange
        long id = (await _registerPropertyGateway.ExecuteAsync(new Property
        {
            Id = 1001,
            Type = PropertyType.Apartment,
            Advertise = new PropertyAdvertise
            {
                TenantId = 1,
                Transaction = Transaction.Rent,
                Title = "Apartment for rent",
                Description = "Apartment for rent",
                Url = "https://www.apartment-for-rent.com",
                RefId = "REF 101",
                Images = new List<string> {"https://www.apartment-for-rent.com/image1.jpg", "https://www.apartment-for-rent.com/image2.jpg"}
            },
            Attributes = new PropertyAttributes
            {
                NumberOfBedrooms = 2,
                NumberOfToilets = 1,
                NumberOfGarages = 1,
                Area = 100,
                BuiltArea = 200,
            },
            Location = new PropertyLocation
            {
                State = "Duckland", City = "White Duck", District = "Downtown", Address = "Good Life Street, 101",
            },
            Prices = new PropertyPrices
            {
                SellingPrice = 100000,
                RentalTotalPrice = 1000,
                RentalPrice = 700,
                PriceByM2 = 100,
                Discount = 100,
                CondominiumFee = 90,
            },
            HashKey = "hash-key-1",
            Ranking = 1,
            Status = PropertyStatus.Active,
            CreatedAt = DateTime.Parse("2023-01-01 23:59:59.999", CultureInfo.DefaultThreadCurrentCulture),
            UpdatedAt = DateTime.Parse("2023-02-01 23:59:59.999", CultureInfo.DefaultThreadCurrentCulture),
        })).Id;

        // Act
        Optional<Property> optional = await _getPropertyByIdGateway.ExecuteAsync(new GetPropertyByIdQuery(Convert.ToString(id)));

        // Assert
        Assert.True(optional.HasValue());

        Property actual = optional.GetValue();
        _outputHelper.WriteLine(actual.ToString());

        Assert.NotNull(actual);

        Assert.Equal(1, actual.Id);
        Assert.Equal(PropertyType.Apartment, actual.Type);
        Assert.Equal(1, actual.Advertise.TenantId);
        Assert.Equal(Transaction.Rent, actual.Advertise.Transaction);
        Assert.Equal("Apartment for rent", actual.Advertise.Title);
        Assert.Equal("Apartment for rent", actual.Advertise.Description);
        Assert.Equal("https://www.apartment-for-rent.com", actual.Advertise.Url);
        Assert.Equal("REF 101", actual.Advertise.RefId);
        Assert.Equal(2, actual.Advertise.Images.Count);
        Assert.Equal("https://www.apartment-for-rent.com/image1.jpg", actual.Advertise.Images[0]);
        Assert.Equal("https://www.apartment-for-rent.com/image2.jpg", actual.Advertise.Images[1]);
        Assert.Equal(2, actual.Attributes.NumberOfBedrooms);
        Assert.Equal(1, actual.Attributes.NumberOfToilets);
        Assert.Equal(1, actual.Attributes.NumberOfGarages);
        Assert.Equal(100, actual.Attributes.Area);
        Assert.Equal(200, actual.Attributes.BuiltArea);
        Assert.Equal("Duckland", actual.Location.State);
        Assert.Equal("White Duck", actual.Location.City);
        Assert.Equal("Downtown", actual.Location.District);
        Assert.Equal("Good Life Street, 101", actual.Location.Address);
        Assert.Equal(100000, actual.Prices.SellingPrice);
        Assert.Equal(1000, actual.Prices.RentalTotalPrice);
        Assert.Equal(700, actual.Prices.RentalPrice);
        Assert.Equal(100, actual.Prices.PriceByM2);
        Assert.Equal(100, actual.Prices.Discount);
        Assert.Equal(90, actual.Prices.CondominiumFee);
        Assert.Equal("hash-key-1", actual.HashKey);
        Assert.Equal(1, actual.Ranking);
        Assert.Equal(PropertyStatus.Active, actual.Status);
        Assert.Equal(DateTime.Parse("2023-01-01 23:59:59.999", CultureInfo.DefaultThreadCurrentCulture), actual.CreatedAt);
        Assert.Equal(DateTime.Parse("2023-02-01 23:59:59.999", CultureInfo.DefaultThreadCurrentCulture), actual.UpdatedAt);
    }
}