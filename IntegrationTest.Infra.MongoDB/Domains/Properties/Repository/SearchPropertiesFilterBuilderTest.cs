using System.Collections.Generic;
using Core.Commons.Models;
using Core.Domains.Properties.Query;
using Infra.MongoDB.Domains.Properties.Model;
using Infra.MongoDB.Domains.Properties.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Infra.MongoDB.Domains.Properties.Repository;

public sealed class SearchPropertiesFilterBuilderTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SearchPropertiesFilterBuilderTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

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

    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Success to build filter definition when values are relevant")]
    public void SuccessToBuildFilterDefinitionWhenValuesAreRelevant()
    {
        // Arrange
        SearchPropertiesQuery searchPropertiesQuery =
            new SearchPropertiesQuery(
                type: "HOUSE",
                advertise: new SearchPropertiesQueryAdvertise(transaction: "RENT"),
                attributes: new SearchPropertiesQueryAttributes(
                    numberOfBedrooms: Range<byte>.Of(from: 1, to: 2),
                    numberOfToilets: Range<byte>.Of(from: 1, to: 2),
                    numberOfGarages: Range<byte>.Of(from: 1, to: 2),
                    area: Range<int>.Of(from: 1, to: 2),
                    builtArea: Range<int>.Of(from: 1, to: 2)),
                location:
                new SearchPropertiesQueryLocation(
                    state: "São Paulo",
                    city: "São Paulo",
                    districts: new List<string> {"Vila Mariana"}),
                prices:
                new SearchPropertiesQueryPrices(
                    sellingPrice: Range<double>.Of(from: 1, to: 2),
                    rentalTotalPrice: Range<double>.Of(from: 1, to: 2),
                    rentalPrice: Range<double>.Of(from: 1, to: 2),
                    priceByM2: Range<double>.Of(from: 1, to: 2)),
                status: "ACTIVE"
            );

        // Act
        FilterDefinition<PropertyEntity> filterDefinition = SearchPropertiesFilterBuilder.Build(searchPropertiesQuery);

        // Assert
        Assert.NotNull(filterDefinition);

        BsonDocument document = GetBsonDocument(filterDefinition);

        _testOutputHelper.WriteLine(document.ToString());

        Assert.Equal(14, document.ElementCount);

        Assert.Equal("HOUSE", document["type"].AsString);
        Assert.Equal("RENT", document["transaction"].AsString);
        Assert.Equal("ACTIVE", document["status"].AsString);
        Assert.Equal("São Paulo", document["city"].AsString);
        Assert.Equal("São Paulo", document["state"].AsString);
        Assert.Equal("Vila Mariana", document["district"].AsBsonDocument.GetElement("$in").Value.AsBsonArray[0].AsString);
        Assert.Equal(1, document["numberOfBedrooms"].AsBsonDocument.GetElement("$gte").Value.ToInt32());
        Assert.Equal(2, document["numberOfBedrooms"].AsBsonDocument.GetElement("$lte").Value.ToInt32());
        Assert.Equal(1, document["numberOfToilets"].AsBsonDocument.GetElement("$gte").Value.ToInt32());
        Assert.Equal(2, document["numberOfToilets"].AsBsonDocument.GetElement("$lte").Value.ToInt32());
        Assert.Equal(1, document["numberOfGarages"].AsBsonDocument.GetElement("$gte").Value.ToInt32());
        Assert.Equal(2, document["numberOfGarages"].AsBsonDocument.GetElement("$lte").Value.ToInt32());
        Assert.Equal(1, document["area"].AsBsonDocument.GetElement("$gte").Value.ToInt32());
        Assert.Equal(2, document["area"].AsBsonDocument.GetElement("$lte").Value.ToInt32());
        Assert.Equal(1, document["builtArea"].AsBsonDocument.GetElement("$gte").Value.ToInt32());
        Assert.Equal(2, document["builtArea"].AsBsonDocument.GetElement("$lte").Value.ToInt32());
    }

    private static BsonDocument GetBsonDocument<T>(FilterDefinition<T> filterDefinition)
    {
        IBsonSerializerRegistry serializerRegistry = BsonSerializer.SerializerRegistry;
        IBsonSerializer<T> documentSerializer = serializerRegistry.GetSerializer<T>();
        return filterDefinition.Render(documentSerializer, serializerRegistry);
    }
}