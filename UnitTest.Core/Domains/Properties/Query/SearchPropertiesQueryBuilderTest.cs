using System.Collections.Generic;
using Core.Domains.Properties.Query;
using Xunit;

namespace UnitTest.Core.Domains.Properties.Query;

public class SearchPropertiesQueryBuilderTests
{
    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Successful to search properties query with all properties set")]
    public void SuccessfulToSearchPropertiesQueryWithAllPropertiesSet()
    {
        // Arrange
        SearchPropertiesQueryBuilder builder = new SearchPropertiesQueryBuilder()
            .WithType("House")
            .WithTransaction("Sale")
            .WithFromNumberOfBedrooms(2)
            .WithToNumberOfBedrooms(4)
            .WithFromNumberOfToilets(1)
            .WithToNumberOfToilets(3)
            .WithFromNumberOfGarages(2)
            .WithToNumberOfGarages(2)
            .WithCity("New York")
            .WithState("NY")
            .WithDistricts(new List<string> {"Downtown", "Midtown"})
            .WithFromArea(150)
            .WithToArea(300)
            .WithFromBuiltArea(120)
            .WithToBuiltArea(250)
            .WithFromSellingPrice(200000)
            .WithToSellingPrice(500000)
            .WithFromRentalTotalPrice(1000)
            .WithToRentalTotalPrice(3000)
            .WithFromRentalPrice(800)
            .WithToRentalPrice(1500)
            .WithFromPriceByM2(50)
            .WithToPriceByM2(100)
            .WithStatus("Available");

        // Act
        SearchPropertiesQuery actual = builder.Build();

        // Assert
        Assert.Equal("HOUSE", actual.Type);
        Assert.Equal("SALE", actual.Advertise.Transaction);
        Assert.Equal((byte) 2, actual.Attributes.NumberOfBedrooms.From);
        Assert.Equal((byte) 4, actual.Attributes.NumberOfBedrooms.To);
        Assert.Equal((byte) 1, actual.Attributes.NumberOfToilets.From);
        Assert.Equal((byte) 3, actual.Attributes.NumberOfToilets.To);
        Assert.Equal((byte) 2, actual.Attributes.NumberOfGarages.From);
        Assert.Equal((byte) 2, actual.Attributes.NumberOfGarages.To);
        Assert.Equal("New York", actual.Location.City);
        Assert.Equal("NY", actual.Location.State);
        Assert.Contains("Downtown", actual.Location.Districts);
        Assert.Contains("Midtown", actual.Location.Districts);
        Assert.Equal(150, actual.Attributes.Area.From);
        Assert.Equal(300, actual.Attributes.Area.To);
        Assert.Equal(120, actual.Attributes.BuiltArea.From);
        Assert.Equal(250, actual.Attributes.BuiltArea.To);
        Assert.Equal(200000, actual.Prices.SellingPrice.From);
        Assert.Equal(500000, actual.Prices.SellingPrice.To);
        Assert.Equal(1000, actual.Prices.RentalTotalPrice.From);
        Assert.Equal(3000, actual.Prices.RentalTotalPrice.To);
        Assert.Equal(800, actual.Prices.RentalPrice.From);
        Assert.Equal(1500, actual.Prices.RentalPrice.To);
        Assert.Equal(50, actual.Prices.PriceByM2.From);
        Assert.Equal(100, actual.Prices.PriceByM2.To);
        Assert.Equal("AVAILABLE", actual.Status);
    }

    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Successful to search properties query with only required properties set")]
    public void SuccessfulToSearchPropertiesQueryWithOnlyRequiredPropertiesSet()
    {
        // Arrange
        SearchPropertiesQueryBuilder builder = new SearchPropertiesQueryBuilder()
            .WithTransaction("Rent");

        // Act
        SearchPropertiesQuery actual = builder.Build();

        // Assert
        Assert.Equal("ALL", actual.Type);
        Assert.Equal("RENT", actual.Advertise.Transaction);
    }
}
