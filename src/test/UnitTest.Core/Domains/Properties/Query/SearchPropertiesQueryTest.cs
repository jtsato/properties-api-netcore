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
    [Fact(DisplayName = "Success to search properties with equal range limits")]
    public void SuccessToSearchPropertiesWithEqualRangeLimits()
    {
        SearchPropertiesQuery query = CreateQuery(
            Range<byte>.Of(1, 1), Range<byte>.Of(2, 2), Range<byte>.Of(3, 3),
            Range<int>.Of(4, 4), Range<int>.Of(5, 5),
            Range<float>.Of(6, 6), Range<float>.Of(7, 7)
        );

        Assert.Equal(1, query.Attributes.NumberOfBedrooms.From);
        Assert.Equal(6, query.Prices.SellingPrice.To);
    }

    [Trait("Category", "Core Business Tests")]
    [Fact(DisplayName = "Fail to search properties when price maximum equals epsilon")]
    public void FailToSearchPropertiesWhenPriceMaximumEqualsEpsilon()
    {
        ValidationException exception = Assert.Throws<ValidationException>(() => CreateQuery(
            Range<byte>.Of(1, 1), Range<byte>.Of(1, 1), Range<byte>.Of(1, 1),
            Range<int>.Of(1, 1), Range<int>.Of(1, 1),
            Range<float>.Of(1, 0.0001f), Range<float>.Of(1, 0.0001f)
        ));

        string[] messages = [.. exception.Errors.Select(failure => failure.ErrorMessage)];

        Assert.Contains("ValidationPropertySellingPriceIsInvalid", messages);
        Assert.Contains("ValidationPropertyRentalPriceIsInvalid", messages);
    }

    [Trait("Category", "Core Business Tests")]
    [Fact(DisplayName = "Fail to search properties with invalid parameters")]
    public void FailToSearchPropertiesWithInvalidParameters()
    {
        // Arrange
        // Act
        ValidationException exception = Assert.Throws<ValidationException>(() =>
            new SearchPropertiesQuery(
                ["InvalidPropertyType"],
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
                    ["Moema", "Vila Mariana"]
                ),
                new SearchPropertiesQueryPrices
                (
                    sellingPrice: Range<float>.Of(200000, 100000),
                    rentalTotalPrice: Range<float>.Of(5000, 4000)
                ),
                "InvalidStatus",
                0
            )
        );

        // Assert
        List<string> messages =
        [
            .. exception
                .Errors
                .Select(failure => failure.ErrorMessage)
        ];

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
            ["House"],
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
                ["Moema", "Vila Mariana"]
            ),
            new SearchPropertiesQueryPrices
            (
                sellingPrice: Range<float>.Of(100000, 0),
                rentalTotalPrice: Range<float>.Of(4000, 0)
            ),
            "Active",
            0
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
        Assert.Equal(4000, query.Prices.RentalTotalPrice.From);
        Assert.Equal(0, query.Prices.RentalTotalPrice.To);
        Assert.Equal("Active", query.Status);
        Assert.Equal(0, query.Ranking);
    }

    [Trait("Category", "Core Business Tests")]
    [Fact(DisplayName = "Success to search properties without min parameters values")]
    public void SuccessToSearchPropertiesWithoutMinParametersValues()
    {
        // Arrange
        // Act
        SearchPropertiesQuery query = new SearchPropertiesQuery(
            ["House"],
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
                ["Moema", "Vila Mariana"]
            ),
            new SearchPropertiesQueryPrices
            (
                sellingPrice: Range<float>.Of(0, 100000),
                rentalTotalPrice: Range<float>.Of(0, 5000)
            ),
            "Active",
            0
        );

        // Assert
        Assert.Equal(["House"], query.Types);
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
        Assert.Equal(0, query.Prices.RentalTotalPrice.From);
        Assert.Equal(5000, query.Prices.RentalTotalPrice.To);
        Assert.Equal("Active", query.Status);
    }

    private static SearchPropertiesQuery CreateQuery(
        Range<byte> bedrooms,
        Range<byte> toilets,
        Range<byte> garages,
        Range<int> area,
        Range<int> builtArea,
        Range<float> sellingPrice,
        Range<float> rentalPrice)
    {
        return new SearchPropertiesQuery(
            ["House"],
            new SearchPropertiesQueryAdvertise("Sale"),
            new SearchPropertiesQueryAttributes(bedrooms, toilets, garages, area, builtArea),
            new SearchPropertiesQueryLocation("São Paulo", "São Paulo", ["Moema"]),
            new SearchPropertiesQueryPrices(sellingPrice, rentalPrice),
            "Active",
            0
        );
    }
}
