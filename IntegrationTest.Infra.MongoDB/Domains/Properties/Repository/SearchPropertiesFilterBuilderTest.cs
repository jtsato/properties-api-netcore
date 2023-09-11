using System.Collections.Generic;
using Core.Commons.Models;
using Core.Domains.Properties.Query;
using Infra.MongoDB.Domains.Properties.Model;
using Infra.MongoDB.Domains.Properties.Repository;
using MongoDB.Driver;
using Xunit;

namespace IntegrationTest.Infra.MongoDB.Domains.Properties.Repository;

public sealed class SearchPropertiesFilterBuilderTest
{
    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Success to build empty filter definition when values are not relevant")]
    public void SuccessToBuildEmptyFilterDefinitionWhenValuesAreNotRelevant()
    {
        // Arrange
        SearchPropertiesQuery searchPropertiesQuery =
            new SearchPropertiesQuery(
                type: "ALL",
                advertise: new SearchPropertiesQueryAdvertise(transaction: "ALL"),
                attributes: new SearchPropertiesQueryAttributes(
                    numberOfBedrooms: Range<byte>.Of(from: 0, to: 0),
                    numberOfToilets: Range<byte>.Of(from: 0, to: 0),
                    numberOfGarages: Range<byte>.Of(from: 0, to: 0),
                    area: Range<int>.Of(from: 0, to: 0),
                    builtArea: Range<int>.Of(from: 0, to: 0)),
                location:
                new SearchPropertiesQueryLocation(
                    state: "",
                    city: "",
                    districts: new List<string>(0)),
                prices:
                new SearchPropertiesQueryPrices(
                    sellingPrice: Range<double>.Of(from: 0, to: 0),
                    rentalTotalPrice: Range<double>.Of(from: 0, to: 0),
                    rentalPrice: Range<double>.Of(from: 0, to: 0),
                    priceByM2: Range<double>.Of(from: 0, to: 0)),
                status: "ALL"
            );

        // Act
        FilterDefinition<PropertyEntity> filterDefinition = SearchPropertiesFilterBuilder.Build(searchPropertiesQuery);

        // Assert
        Assert.NotNull(filterDefinition);
        Assert.Equal(Builders<PropertyEntity>.Filter.Empty, filterDefinition);
    }
}
