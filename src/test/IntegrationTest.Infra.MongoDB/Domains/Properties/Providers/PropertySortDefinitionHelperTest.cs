using System.Collections.Generic;
using Core.Commons.Paging;
using Infra.MongoDB.Domains.Properties.Model;
using Infra.MongoDB.Domains.Properties.Providers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Infra.MongoDB.Domains.Properties.Providers;

[Collection("Database collection [NoContext]")]
public class PropertySortDefinitionHelperTest(ITestOutputHelper outputHelper)
{

    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Success to  return default sorting criteria when the user has not specified any sorting criteria")]
    public void SuccessToReturnDefaultSortingCriteriaWhenTheUserHasNotSpecifiedAnySortingCriteria()
    {
        // Arrange
        IEnumerable<Order> orders = [];
        
        // Act
        SortDefinition<PropertyEntity> sortDefinition = PropertySortDefinitionHelper.GetSortDefinitions(orders);
        
        // Assert
        Assert.NotNull(sortDefinition);
        BsonDocument document = sortDefinition.Render(new RenderArgs<PropertyEntity>(BsonSerializer.SerializerRegistry.GetSerializer<PropertyEntity>(), BsonSerializer.SerializerRegistry));
        
        Assert.NotNull(document);
        outputHelper.WriteLine(document.ToString());
        
        Assert.Equal(2, document.ElementCount);
        Assert.Equal("""{ "ranking" : -1, "updatedAt" : -1 }""", document.ToString());
    }
    
    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Success to return sorting criteria when the user has specified any sorting criteria")]
    public void SuccessToReturnSortingCriteriaWhenTheUserHasSpecifiedAnySortingCriteria()
    {
        // Arrange
        IEnumerable<Order> orders = new List<Order>
        {
            new Order(Direction.Asc, "name"),
        };
        
        // Act
        SortDefinition<PropertyEntity> sortDefinition = PropertySortDefinitionHelper.GetSortDefinitions(orders);
        
        // Assert
        Assert.NotNull(sortDefinition);
        BsonDocument document = sortDefinition.Render(new RenderArgs<PropertyEntity>(BsonSerializer.SerializerRegistry.GetSerializer<PropertyEntity>(), BsonSerializer.SerializerRegistry));
        
        Assert.NotNull(document);
        outputHelper.WriteLine(document.ToString());
        
        Assert.Equal(3, document.ElementCount);
        Assert.Equal("""{ "name" : 1, "ranking" : -1, "updatedAt" : -1 }""", document.ToString());
    }
    
    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Success to return sorting criteria when the user has specified any sorting criteria with default sorting criteria")]
    public void SuccessToReturnSortingCriteriaWhenTheUserHasSpecifiedAnySortingCriteriaWithDefaultSortingCriteria()
    {
        // Arrange
        IEnumerable<Order> orders = new List<Order>
        {
            new Order(Direction.Asc, "name"),
            new Order(Direction.Desc, "updatedAt"),
        };
        
        // Act
        SortDefinition<PropertyEntity> sortDefinition = PropertySortDefinitionHelper.GetSortDefinitions(orders);
        
        // Assert
        Assert.NotNull(sortDefinition);
        BsonDocument document = sortDefinition.Render(new RenderArgs<PropertyEntity>(BsonSerializer.SerializerRegistry.GetSerializer<PropertyEntity>(), BsonSerializer.SerializerRegistry));
        
        Assert.NotNull(document);
        outputHelper.WriteLine(document.ToString());
        
        Assert.Equal(3, document.ElementCount);
        Assert.Equal("""{ "name" : 1, "updatedAt" : -1, "ranking" : -1 }""", document.ToString());
    }
}