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
    [Theory(DisplayName = "Fail to search properties with invalid parameters")]
    [InlineData("2023-02-30", "2023-04-31", "ValidationPropertyFromCreatedAtIsInvalid")]
    [InlineData("2023-04-31", "2023-02-30", "ValidationPropertyToCreatedAtIsInvalid")]
    [InlineData("2023-02-30", "2023-04-31", "ValidationPropertyFromUpdatedAtIsInvalid")]
    [InlineData("2023-04-31", "2023-02-30", "ValidationPropertyToUpdatedAtIsInvalid")]
    public void FailToSearchPropertiesWithInvalidParameters(string createdAt, string updatedAt, string expected)
    {
        // Arrange
        // Act
        ValidationException exception = Assert.Throws<ValidationException>(() =>
            new SearchPropertiesQuery(
                new SearchPropertiesQueryAdvertise(null, null),
                new SearchPropertiesQueryAttributes
                {
                    NumberOfBedrooms = Range<byte>.Of(1, 2),
                    NumberOfToilets = Range<byte>.Of(3, 4),
                    NumberOfGarages = Range<byte>.Of(5, 5),
                    Area = Range<int>.Of(100, 200),
                    BuiltArea = Range<int>.Of(10, 20)
                },
                new SearchPropertiesQueryLocation(
                    "São Paulo",
                    new List<string> {"Moema", "Vila Mariana"}
                ),
                new SearchPropertiesQueryPrices
                {
                    SellingPrice = Range<decimal>.Of(100000, 200000),
                    RentalTotalPrice = Range<decimal>.Of(1000, 2000),
                    RentalPrice = Range<decimal>.Of(1000, 2000),
                    PriceByM2 = Range<decimal>.Of(1000, 2000)
                },
                Range<string>.Of(createdAt, updatedAt),
                Range<string>.Of(createdAt, createdAt)
            )
        );

        // Assert
        List<string> messages = exception
            .Errors
            .Select(failure => failure.ErrorMessage)
            .ToList();

        Assert.Contains(expected, messages);
    }
}