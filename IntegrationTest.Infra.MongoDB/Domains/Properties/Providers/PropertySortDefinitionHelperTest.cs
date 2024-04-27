using System.Collections.Generic;
using System.Linq;
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
public class PropertySortDefinitionHelperTest
{
    private readonly ITestOutputHelper _outputHelper;
    
    public PropertySortDefinitionHelperTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    
    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Success to  return default sorting criteria when the user has not specified any sorting criteria")]
    public void SuccessToReturnDefaultSortingCriteriaWhenTheUserHasNotSpecifiedAnySortingCriteria()
    {
        // Arrange
        IEnumerable<Order> orders = Enumerable.Empty<Order>();
        
        // Act
        SortDefinition<PropertyEntity> sortDefinition = PropertySortDefinitionHelper.GetSortDefinitions(orders);
        
        // Assert
        Assert.NotNull(sortDefinition);
        BsonDocument document = sortDefinition.Render(BsonSerializer.SerializerRegistry.GetSerializer<PropertyEntity>(), BsonSerializer.SerializerRegistry);
        
        Assert.NotNull(document);
        _outputHelper.WriteLine(document.ToString());
        
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
        BsonDocument document = sortDefinition.Render(BsonSerializer.SerializerRegistry.GetSerializer<PropertyEntity>(), BsonSerializer.SerializerRegistry);
        
        Assert.NotNull(document);
        _outputHelper.WriteLine(document.ToString());
        
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
        BsonDocument document = sortDefinition.Render(BsonSerializer.SerializerRegistry.GetSerializer<PropertyEntity>(), BsonSerializer.SerializerRegistry);
        
        Assert.NotNull(document);
        _outputHelper.WriteLine(document.ToString());
        
        Assert.Equal(3, document.ElementCount);
        Assert.Equal("""{ "name" : 1, "updatedAt" : -1, "ranking" : -1 }""", document.ToString());
    }
}