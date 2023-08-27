using System.Collections.Generic;
using Infra.MongoDB.Commons.Helpers;
using MongoDB.Driver;
using Xunit;

namespace IntegrationTest.Infra.MongoDB.Commons;

public class FilterDefinitionHelperTest
{
    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Fail to add filter definition if value is null")]
    public void FailToAddFilterDefinitionIfValueIsNull()
    {
        // Arrange
        List<FilterDefinition<DummyEntity>> filterDefinitions = new List<FilterDefinition<DummyEntity>>();

        // Act
        FilterHelper.AddEqualsFilter(filterDefinitions, document => document.Surname, null);
        FilterHelper.AddLikeFilter(filterDefinitions, document => document.Name, string.Empty);
        FilterHelper.AddDateAfterOrEqualFilter(filterDefinitions, document => document.BirthDate, null);
        FilterHelper.AddDateBeforeOrEqualFilter(filterDefinitions, document => document.BirthDate, null);

        // Assert
        Assert.Empty(filterDefinitions);
    }

    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Successful to add filter definition if value is not null")]
    public void SuccessfulToAddFilterDefinitionIfValueIsNotNull()
    {
        // Arrange
        List<FilterDefinition<DummyEntity>> filterDefinitions = new List<FilterDefinition<DummyEntity>>();

        // Act
        FilterHelper.AddEqualsFilter(filterDefinitions, document => document.Surname, "Smith");
        FilterHelper.AddLikeFilter(filterDefinitions, document => document.Name, "Kim");
        FilterHelper.AddDateAfterOrEqualFilter(filterDefinitions, document => document.BirthDate, "2020-05-20 23:59:59");
        FilterHelper.AddDateBeforeOrEqualFilter(filterDefinitions, document => document.BirthDate, "2020-05-20 23:59:59");
        
        // Assert
        Assert.Equal(4, filterDefinitions.Count);
    }
}

