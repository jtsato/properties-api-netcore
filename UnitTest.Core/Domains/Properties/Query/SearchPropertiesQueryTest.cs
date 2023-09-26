using System.Collections.Generic;
using System.Linq;
using Core.Commons.Models;
using Core.Domains.Properties.Query;
using FluentValidation;
using Xunit;

namespace UnitTest.Core.Domains.Properties.Query;

public class SearchPropertiesQueryTest
{
    [Trait("Category", "Core Business Tests")]
    [Fact(DisplayName = "Fail to search properties with invalid parameters")]
    public void FailToSearchPropertiesWithInvalidParameters()
    {
        // Arrange
        // Act
        ValidationException exception = Assert.Throws<ValidationException>(() =>
            new SearchPropertiesQuery(
                new List<string> {"InvalidPropertyType"},
                new SearchPropertiesQueryAdvertise("InvalidTransaction"),
                new SearchPropertiesQueryAttributes
                (
                    numberOfBedrooms: Range<byte>.Of(2, 1),
                    numberOfToilets: Range<byte>.Of(4, 3),
                    numberOfGarages: Range<byte>.Of(6, 5),
                    area: Range<int>.Of(200, 100),
                    builtArea: Range<int>.Of(20, 10)
                ),
                new SearchPropertiesQueryLocation(
                    "São Paulo",
                    "São Paulo",
                    new List<string> {"Moema", "Vila Mariana"}
                ),
                new SearchPropertiesQueryPrices
                (
                    sellingPrice: Range<double>.Of(200000, 100000),
                    rentalPrice: Range<double>.Of(5000, 4000)
                ),
                "InvalidStatus"
            )
        );

        // Assert
        List<string> messages = exception
            .Errors
            .Select(failure => failure.ErrorMessage)
            .ToList();

        Assert.Contains("ValidationPropertyTypesAreInvalid", messages);
        Assert.Contains("ValidationPropertyTransactionIsInvalid", messages);
        Assert.Contains("ValidationPropertyNumberOfBedroomsIsInvalid", messages);
        Assert.Contains("ValidationPropertyNumberOfToiletsIsInvalid", messages);
        Assert.Contains("ValidationPropertyNumberOfGaragesIsInvalid", messages);
        Assert.Contains("ValidationPropertyAreaIsInvalid", messages);
        Assert.Contains("ValidationPropertyBuiltAreaIsInvalid", messages);
        Assert.Contains("ValidationPropertySellingPriceIsInvalid", messages);
        Assert.Contains("ValidationPropertyRentalPriceIsInvalid", messages);
        Assert.Contains("ValidationPropertyStatusIsInvalid", messages);
    }

    [Trait("Category", "Core Business Tests")]
    [Fact(DisplayName = "Success to search properties without max parameters values")]
    public void SuccessToSearchPropertiesWithoutMaxParametersValues()
    {
        // Arrange
        // Act
        SearchPropertiesQuery query = new SearchPropertiesQuery(
            new List<string> {"House"},
            new SearchPropertiesQueryAdvertise("Sale"),
            new SearchPropertiesQueryAttributes
            (
                numberOfBedrooms: Range<byte>.Of(1, 0),
                numberOfToilets: Range<byte>.Of(3, 0),
                numberOfGarages: Range<byte>.Of(5, 0),
                area: Range<int>.Of(100, 0),
                builtArea: Range<int>.Of(10, 0)
            ),
            new SearchPropertiesQueryLocation(
                "São Paulo",
                "São Paulo",
                new List<string> {"Moema", "Vila Mariana"}
            ),
            new SearchPropertiesQueryPrices
            (
                sellingPrice: Range<double>.Of(100000, 0),
                rentalPrice: Range<double>.Of(4000, 0)
            ),
            "Active"
        );

        // Assert
        Assert.True(query.Types.SequenceEqual(new List<string> {"House"}));
        Assert.Equal("Sale", query.Advertise.Transaction);
        Assert.Equal(1, query.Attributes.NumberOfBedrooms.From);
        Assert.Equal(0, query.Attributes.NumberOfBedrooms.To);
        Assert.Equal(3, query.Attributes.NumberOfToilets.From);
        Assert.Equal(0, query.Attributes.NumberOfToilets.To);
        Assert.Equal(5, query.Attributes.NumberOfGarages.From);
        Assert.Equal(0, query.Attributes.NumberOfGarages.To);
        Assert.Equal(100, query.Attributes.Area.From);
        Assert.Equal(0, query.Attributes.Area.To);
        Assert.Equal(10, query.Attributes.BuiltArea.From);
        Assert.Equal(0, query.Attributes.BuiltArea.To);
        Assert.Equal("São Paulo", query.Location.State);
        Assert.Equal("São Paulo", query.Location.City);
        Assert.Equal(2, query.Location.Districts.Count);
        Assert.Equal("Moema", query.Location.Districts[0]);
        Assert.Equal("Vila Mariana", query.Location.Districts[1]);
        Assert.Equal(100000, query.Prices.SellingPrice.From);
        Assert.Equal(0, query.Prices.SellingPrice.To);
        Assert.Equal(4000, query.Prices.RentalPrice.From);
        Assert.Equal(0, query.Prices.RentalPrice.To);
        Assert.Equal("Active", query.Status);
    }

    [Trait("Category", "Core Business Tests")]
    [Fact(DisplayName = "Success to search properties without min parameters values")]
    public void SuccessToSearchPropertiesWithoutMinParametersValues()
    {
        // Arrange
        // Act
        SearchPropertiesQuery query = new SearchPropertiesQuery(
            new List<string> {"House"},
            new SearchPropertiesQueryAdvertise("Sale"),
            new SearchPropertiesQueryAttributes
            (
                numberOfBedrooms: Range<byte>.Of(0, 2),
                numberOfToilets: Range<byte>.Of(0, 4),
                numberOfGarages: Range<byte>.Of(0, 6),
                area: Range<int>.Of(0, 200),
                builtArea: Range<int>.Of(0, 20)
            ),
            new SearchPropertiesQueryLocation(
                "São Paulo",
                "São Paulo",
                new List<string> {"Moema", "Vila Mariana"}
            ),
            new SearchPropertiesQueryPrices
            (
                sellingPrice: Range<double>.Of(0, 100000),
                rentalPrice: Range<double>.Of(0, 5000)
            ),
            "Active"
        );

        // Assert
        Assert.Equal(new List<string> {"House"}, query.Types);
        Assert.Equal("Sale", query.Advertise.Transaction);
        Assert.Equal(0, query.Attributes.NumberOfBedrooms.From);
        Assert.Equal(2, query.Attributes.NumberOfBedrooms.To);
        Assert.Equal(0, query.Attributes.NumberOfToilets.From);
        Assert.Equal(4, query.Attributes.NumberOfToilets.To);
        Assert.Equal(0, query.Attributes.NumberOfGarages.From);
        Assert.Equal(6, query.Attributes.NumberOfGarages.To);
        Assert.Equal(0, query.Attributes.Area.From);
        Assert.Equal(200, query.Attributes.Area.To);
        Assert.Equal(0, query.Attributes.BuiltArea.From);
        Assert.Equal(20, query.Attributes.BuiltArea.To);
        Assert.Equal("São Paulo", query.Location.State);
        Assert.Equal("São Paulo", query.Location.City);
        Assert.Equal(2, query.Location.Districts.Count);
        Assert.Equal("Moema", query.Location.Districts[0]);
        Assert.Equal("Vila Mariana", query.Location.Districts[1]);
        Assert.Equal(0, query.Prices.SellingPrice.From);
        Assert.Equal(100000, query.Prices.SellingPrice.To);
        Assert.Equal(0, query.Prices.RentalPrice.From);
        Assert.Equal(5000, query.Prices.RentalPrice.To);
        Assert.Equal("Active", query.Status);
    }
}