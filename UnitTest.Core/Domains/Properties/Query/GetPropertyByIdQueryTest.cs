using System.Collections.Generic;
using System.Linq;
using Core.Domains.Properties.Query;
using FluentValidation;
using Xunit;

namespace UnitTest.Core.Domains.Properties.Query;

public sealed class GetPropertyByIdQueryTest
{
    [Trait("Category", "Core Business Tests")]
    [Theory(DisplayName = "Fail to get property by id with null or empty id")]
    [InlineData(null, "ValidationPropertyIdIsNullOrEmpty")]
    public void FailToGetPropertyByIdWithNullOrEmptyId(string id, string expected)
    {
        // Arrange
        // Act
        // Assert
        ValidationException exception = Assert.Throws<ValidationException>(() =>
            new GetPropertyByIdQuery(id)
        );

        List<string> messages = exception
            .Errors
            .Select(failure => failure.ErrorMessage)
            .ToList();

        Assert.Contains(expected, messages);
    }
}