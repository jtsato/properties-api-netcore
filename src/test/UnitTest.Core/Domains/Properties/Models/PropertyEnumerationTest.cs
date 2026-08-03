using System.Linq;
using Core.Commons;
using Core.Domains.Properties.Models;
using Xunit;

namespace UnitTest.Core.Domains.Properties.Models;

public sealed class PropertyEnumerationTest
{
    [Trait("Category", "Core Business Tests")]
    [Fact(DisplayName = "Property types expose their expected identifiers")]
    public void PropertyTypesExposeTheirExpectedIdentifiers()
    {
        string[] expectedNames =
        [
            "ALL", "APARTMENT", "WAREHOUSE", "HOUSE", "COUNTRY_HOUSE", "FARM", "GARAGE",
            "LAND_DIVISION", "BUSINESS_PREMISES", "OFFICE", "TWO_STOREY_HOUSE", "LAND", "OTHER"
        ];

        string[] actualNames = [.. Enumeration<PropertyType>.GetAll().Select(type => type.Name)];

        Assert.Equal(expectedNames.Order(), actualNames.Order());
    }

    [Trait("Category", "Core Business Tests")]
    [Fact(DisplayName = "Property statuses expose their expected identifiers")]
    public void PropertyStatusesExposeTheirExpectedIdentifiers()
    {
        string[] actualNames = [.. Enumeration<PropertyStatus>.GetAll().Select(status => status.Name)];

        Assert.Equal(["ACTIVE", "ALL", "INACTIVE"], actualNames.Order());
    }
}
