using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Core.Commons.Paging;
using Core.Domains.Properties.Gateways;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;
using IntegrationTest.Infra.MongoDB.Commons;
using Xunit;
using Xunit.Abstractions;
using Optional = Core.Commons.Optional<Core.Domains.Properties.Models.Property>;

namespace IntegrationTest.Infra.MongoDB.Domains.Properties.Providers;

[Collection("Database collection")]
public class SearchPropertiesProviderTest
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly IRegisterPropertyGateway _registerPropertyGateway;
    private readonly ISearchPropertiesGateway _searchPropertiesGateway;

    public SearchPropertiesProviderTest(ITestOutputHelper outputHelper, Context context)
    {
        _outputHelper = outputHelper;
        _registerPropertyGateway = context.ServiceResolver.Resolve<IRegisterPropertyGateway>();
        _searchPropertiesGateway = context.ServiceResolver.Resolve<ISearchPropertiesGateway>();
    }

    [Trait("Category", "Infrastructure (DB) Integration tests")]
    [Fact(DisplayName = "Successful to search properties")]
    public async Task SuccessfulToSearchProperties()
    {
        // Arrange
        List<Task> tasks = new List<Task>();

        for (int index = 1; index <= 10; index++)
        {
            Property property =
                new Property
                {
                    Type = PropertyType.Apartment,
                    Advertise = new PropertyAdvertise
                    {
                        TenantId = 1,
                        Transaction = Transaction.Rent,
                        Title = "Apartment for rent",
                        Description = "Apartment for rent",
                        Url = "https://www.apartment-for-rent.com",
                        RefId = $"REF 10{index:D2}",
                        Images = new List<string>
                        {
                            "https://www.apartment-for-rent.com/image1.jpg",
                            "https://www.apartment-for-rent.com/image2.jpg"
                        }
                    },
                    Attributes = new PropertyAttributes
                    {
                        NumberOfBedrooms = (byte) (2 * index),
                        NumberOfToilets = (byte) (1 * index),
                        NumberOfGarages = (byte) (1 * index),
                        Area = 100 * index,
                        BuiltArea = 200 * index,
                    },
                    Location = new PropertyLocation
                    {
                        State = "Duckland",
                        City = "White Duck",
                        District = "Downtown",
                        Address = $"Good Life Street, 10{index}",
                    },
                    Prices = new PropertyPrices
                    {
                        SellingPrice = 1000 * index,
                        RentalTotalPrice = 1000 * index,
                        RentalPrice = 700 * index,
                        PriceByM2 = 100 * index,
                        Discount = 100 * index,
                        CondominiumFee = 90 * index,
                    },
                    HashKey = "hash-key-1",
                    Ranking = 1,
                    Status = PropertyStatus.Active,
                    CreatedAt = DateTime.Parse("2023-01-01 23:59:59.999", CultureInfo.DefaultThreadCurrentCulture),
                    UpdatedAt = DateTime.Parse("2023-02-01 23:59:59.999", CultureInfo.DefaultThreadCurrentCulture),
                };

            tasks.Add(_registerPropertyGateway.ExecuteAsync(property));
        }

        await Task.WhenAll(tasks);

        SearchPropertiesQueryBuilder queryBuilder = new SearchPropertiesQueryBuilder();

        queryBuilder
            .WithType("Apartment")
            .WithTransaction("Rent")
            .WithState("Duckland")
            .WithCity("White Duck")
            .WithDistricts(new List<string> {"Downtown", "Alta Vista"})
            .WithFromNumberOfBedrooms(2)
            .WithToNumberOfBedrooms(20)
            .WithFromNumberOfToilets(1)
            .WithToNumberOfToilets(10)
            .WithFromNumberOfGarages(1)
            .WithToNumberOfGarages(10)
            .WithFromArea(100)
            .WithToArea(1000)
            .WithFromBuiltArea(200)
            .WithToBuiltArea(2000)
            .WithFromSellingPrice(1000)
            .WithToSellingPrice(10000)
            .WithFromRentalTotalPrice(1000)
            .WithToRentalTotalPrice(10000)
            .WithFromRentalPrice(700)
            .WithToRentalPrice(7000)
            .WithFromPriceByM2(100)
            .WithToPriceByM2(1000)
            .WithStatus("Active");

        SearchPropertiesQuery query = queryBuilder.Build();

        List<Order> orders = new List<Order>
        {
            new Order(Direction.Desc, "ranking"),
            new Order(Direction.Asc, "createdAt"),
            new Order(Direction.Asc, "updatedAt"),
            new Order(Direction.Asc, "status"),
            new Order(Direction.Asc, "type"),
            new Order(Direction.Desc, "sellingPrice")
        };

        // Act
        Page<Property> page = await _searchPropertiesGateway.ExecuteAsync(query, PageRequest.Of(2, 3, Sort.By(orders)));

        // Assert
        Assert.NotNull(page);
        _outputHelper.WriteLine(page.ToString());

        // Assert
        Assert.NotNull(page);
        Assert.Equal(2, page.Pageable.Page);
        Assert.Equal(3, page.Pageable.Size);
        Assert.Equal(3, page.Pageable.NumberOfElements);
        Assert.Equal(10, page.Pageable.TotalOfElements);
        Assert.Equal(4, page.Pageable.TotalPages);

        Assert.Equal(3, page.Content.Count);
        Property actual = page.Content[0];

        Assert.NotNull(actual);
        Assert.Equal(PropertyType.Apartment, actual.Type);
        Assert.Equal(Transaction.Rent, actual.Advertise.Transaction);
        Assert.Equal("Apartment for rent", actual.Advertise.Title);
        Assert.Equal("Apartment for rent", actual.Advertise.Description);
        Assert.Equal("https://www.apartment-for-rent.com", actual.Advertise.Url);
        Assert.Equal("REF 1004", actual.Advertise.RefId);
        Assert.Equal(2, actual.Advertise.Images.Count);
        Assert.Equal("https://www.apartment-for-rent.com/image1.jpg", actual.Advertise.Images[0]);
        Assert.Equal("https://www.apartment-for-rent.com/image2.jpg", actual.Advertise.Images[1]);
        Assert.Equal(8, actual.Attributes.NumberOfBedrooms);
        Assert.Equal(4, actual.Attributes.NumberOfToilets);
        Assert.Equal(4, actual.Attributes.NumberOfGarages);
        Assert.Equal(400, actual.Attributes.Area);
        Assert.Equal(800, actual.Attributes.BuiltArea);
        Assert.Equal("Duckland", actual.Location.State);
        Assert.Equal("White Duck", actual.Location.City);
        Assert.Equal("Downtown", actual.Location.District);
        Assert.Equal("Good Life Street, 104", actual.Location.Address);
        Assert.Equal(4000, actual.Prices.SellingPrice);
        Assert.Equal(4000, actual.Prices.RentalTotalPrice);
        Assert.Equal(2800, actual.Prices.RentalPrice);
        Assert.Equal(400, actual.Prices.PriceByM2);
        Assert.Equal(400, actual.Prices.Discount);
        Assert.Equal(360, actual.Prices.CondominiumFee);
        Assert.Equal("hash-key-1", actual.HashKey);
        Assert.Equal(1, actual.Ranking);
        Assert.Equal(PropertyStatus.Active, actual.Status);
        Assert.Equal(DateTime.Parse("2023-01-01 23:59:59.999", CultureInfo.DefaultThreadCurrentCulture), actual.CreatedAt);
        Assert.Equal(DateTime.Parse("2023-02-01 23:59:59.999", CultureInfo.DefaultThreadCurrentCulture), actual.UpdatedAt);
    }

    [Trait("Category", "Infrastructure (DB) Integration tests")]
    [Fact(DisplayName = "Successful to search properties with no sort")]
    public async Task SuccessfulToSearchPropertiesWithNoSort()
    {
        // Arrange
        Property property =
            new Property
            {
                Type = PropertyType.House,
                Advertise = new PropertyAdvertise
                {
                    TenantId = 1,
                    Transaction = Transaction.Rent,
                    Title = "House for rent",
                    Description = "House for rent",
                    Url = "https://www.house-for-rent.com",
                    RefId = "REF 201",
                    Images = new List<string>
                    {
                        "https://www.house-for-rent.com/image1.jpg",
                        "https://www.house-for-rent.com/image2.jpg"
                    }
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
                    State = "Duckland",
                    City = "White Duck",
                    District = "Downtown",
                    Address = "Good Life Street, 201",
                },
                Prices = new PropertyPrices
                {
                    SellingPrice = 1000,
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
            };

        await _registerPropertyGateway.ExecuteAsync(property);

        SearchPropertiesQueryBuilder queryBuilder = new SearchPropertiesQueryBuilder();

        queryBuilder
            .WithType("House")
            .WithTransaction("Rent")
            .WithState("Duckland")
            .WithCity("White Duck")
            .WithDistricts(new List<string> {"Downtown", "Alta Vista"})
            .WithStatus("Active");

        SearchPropertiesQuery query = queryBuilder.Build();

        // Act
        Page<Property> page = await _searchPropertiesGateway.ExecuteAsync(query, PageRequest.Of(0, 3));

        // Assert
        Assert.NotNull(page);
        _outputHelper.WriteLine(page.ToString());

        // Assert
        Assert.NotNull(page);
        Assert.Equal(0, page.Pageable.Page);
        Assert.Equal(3, page.Pageable.Size);
        Assert.Equal(1, page.Pageable.NumberOfElements);
        Assert.Equal(1, page.Pageable.TotalOfElements);
        Assert.Equal(1, page.Pageable.TotalPages);

        Assert.Single(page.Content);
        Property actual = page.Content[0];

        Assert.NotNull(actual);
        Assert.Equal(PropertyType.House, actual.Type);
        Assert.Equal(Transaction.Rent, actual.Advertise.Transaction);
        Assert.Equal("House for rent", actual.Advertise.Title);
        Assert.Equal("House for rent", actual.Advertise.Description);
        Assert.Equal("https://www.house-for-rent.com", actual.Advertise.Url);
        Assert.Equal("REF 201", actual.Advertise.RefId);
        Assert.Equal(2, actual.Advertise.Images.Count);
        Assert.Equal("https://www.house-for-rent.com/image1.jpg", actual.Advertise.Images[0]);
        Assert.Equal("https://www.house-for-rent.com/image2.jpg", actual.Advertise.Images[1]);
        Assert.Equal(2, actual.Attributes.NumberOfBedrooms);
        Assert.Equal(1, actual.Attributes.NumberOfToilets);
        Assert.Equal(1, actual.Attributes.NumberOfGarages);
        Assert.Equal(100, actual.Attributes.Area);
        Assert.Equal(200, actual.Attributes.BuiltArea);
        Assert.Equal("Duckland", actual.Location.State);
        Assert.Equal("White Duck", actual.Location.City);
        Assert.Equal("Downtown", actual.Location.District);
        Assert.Equal("Good Life Street, 201", actual.Location.Address);
        Assert.Equal(1000, actual.Prices.SellingPrice);
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