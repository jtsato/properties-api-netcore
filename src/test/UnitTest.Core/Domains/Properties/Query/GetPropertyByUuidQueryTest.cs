using System.Collections.Generic;
using System.Linq;
using Core.Domains.Properties.Query;
using FluentValidation;
using Xunit;

namespace UnitTest.Core.Domains.Properties.Query;

public sealed class GetPropertyByUuidQueryTest
{
    [Trait("Category", "Core Business Tests")]
    [Theory(DisplayName = "Fail to get property by uuid with null or empty uuid")]
    [InlineData(null, "ValidationPropertyIdIsNullOrEmpty")]
    public void FailToGetPropertyByUuidWithNullOrEmptyUuid(string id, string expected)
    {
        // Arrange
        // Act
        // Assert
        ValidationException exception = Assert.Throws<ValidationException>(() =>
            new GetPropertyByUuidQuery(id)
        );

        List<string> messages = exception
            .Errors
            .Select(failure => failure.ErrorMessage)
            .ToList();

        Assert.Contains(expected, messages);
    }
}