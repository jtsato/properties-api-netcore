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
                "InvalidPropertyType",
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
                    rentalTotalPrice: Range<double>.Of(3000, 2000),
                    rentalPrice: Range<double>.Of(5000, 4000),
                    priceByM2: Range<double>.Of(200, 100)
                ),
                "InvalidStatus"
            )
        );

        // Assert
        List<string> messages = exception
            .Errors
            .Select(failure => failure.ErrorMessage)
            .ToList();

        Assert.Contains("ValidationPropertyTypeIsInvalid", messages);
        Assert.Contains("ValidationPropertyTransactionIsInvalid", messages);
        Assert.Contains("ValidationPropertyNumberOfBedroomsIsInvalid", messages);
        Assert.Contains("ValidationPropertyNumberOfToiletsIsInvalid", messages);
        Assert.Contains("ValidationPropertyNumberOfGaragesIsInvalid", messages);
        Assert.Contains("ValidationPropertyAreaIsInvalid", messages);
        Assert.Contains("ValidationPropertyBuiltAreaIsInvalid", messages);
        Assert.Contains("ValidationPropertySellingPriceIsInvalid", messages);
        Assert.Contains("ValidationPropertyRentalTotalPriceIsInvalid", messages);
        Assert.Contains("ValidationPropertyRentalPriceIsInvalid", messages);
        Assert.Contains("ValidationPropertyPriceByM2IsInvalid", messages);
        Assert.Contains("ValidationPropertyStatusIsInvalid", messages);
    }
}