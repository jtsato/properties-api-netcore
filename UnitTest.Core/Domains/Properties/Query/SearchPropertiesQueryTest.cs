using System.Collections.Generic;
using System.Linq;
using Core.Commons.Models;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;
using FluentValidation;
using Xunit;

namespace UnitTest.Core.Domains.Properties.Query;

public class SearchPropertiesQueryTest
{
    [Trait("Category", "Core Business Tests")]
    [Theory(DisplayName = "Fail to search properties with invalid parameters")]
    [InlineData("    ", "ValidationPropertyTransactionIsNullOrEmpty")]
    public void FailToSearchPropertiesWithInvalidParameters(string transaction, string expected)
    {
        // Arrange
        // Act
        ValidationException exception = Assert.Throws<ValidationException>(() =>
            new SearchPropertiesQuery(
                PropertyType.All.Name,
                new SearchPropertiesQueryAdvertise(transaction),
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
                    "São Paulo",
                    new List<string> {"Moema", "Vila Mariana"}
                ),
                new SearchPropertiesQueryPrices
                {
                    SellingPrice = Range<double>.Of(100000, 200000),
                    RentalTotalPrice = Range<double>.Of(2000, 3000),
                    RentalPrice = Range<double>.Of(4000, 5000),
                    PriceByM2 = Range<double>.Of(100, 200)
                },
                "Active"
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