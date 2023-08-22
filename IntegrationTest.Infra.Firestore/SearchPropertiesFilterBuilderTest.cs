using Infra.Firestore.Domain.Properties.Providers;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using Core.Domains.Properties.Query;
using Google.Cloud.Firestore;

namespace IntegrationTest.Infra.Firestore;

public class SearchPropertiesFilterBuilderTest
{
    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Successfully to build filter builder from query parameters")]
    public void SuccessfullyToBuildFilterBuilderFromQueryParameters()
    {
        // Arrange
        SearchPropertiesQueryBuilder queryBuilder = new SearchPropertiesQueryBuilder();
        queryBuilder.WithTenantId(1);
        queryBuilder.WithType("SALE");
        queryBuilder.WithTransaction("RENT");
        queryBuilder.WithRefId("REFID");
        queryBuilder.WithFromNumberOfBedrooms(1);
        queryBuilder.WithToNumberOfBedrooms(2);
        queryBuilder.WithFromNumberOfToilets(3);
        queryBuilder.WithToNumberOfToilets(4);
        queryBuilder.WithFromNumberOfGarages(5);
        queryBuilder.WithToNumberOfGarages(6);
        queryBuilder.WithFromArea(100);
        queryBuilder.WithToArea(200);
        queryBuilder.WithFromBuiltArea(10);
        queryBuilder.WithToBuiltArea(20);
        queryBuilder.WithState("SÃO PAULO");
        queryBuilder.WithCity("Campinas");
        queryBuilder.WithDistricts(new List<string> {"Vila Mariana", "Vila Madalena"});
        queryBuilder.WithFromSellingPrice(100000);
        queryBuilder.WithToSellingPrice(200000);
        queryBuilder.WithFromRentalTotalPrice(1000);
        queryBuilder.WithToRentalTotalPrice(2000);
        queryBuilder.WithFromRentalPrice(1000);
        queryBuilder.WithToRentalPrice(2000);
        queryBuilder.WithFromPriceByM2(1000);
        queryBuilder.WithToPriceByM2(2000);
        queryBuilder.WithFromRanking(1);
        queryBuilder.WithToRanking(2);
        queryBuilder.WithStatus("ACTIVE");
        queryBuilder.WithFromCreatedAt("2023-02-28");
        queryBuilder.WithToCreatedAt("2023-04-30");
        queryBuilder.WithFromUpdatedAt("2023-02-28");
        queryBuilder.WithToUpdatedAt("2023-04-30");

        // Act
        IEnumerable<Filter> enumerable = SearchPropertiesFilterBuilder.BuildFromQuery(queryBuilder.Build());

        // Assert
        Filter[] filters = enumerable as Filter[] ?? enumerable.ToArray();

        Assert.True(filters.Length == 14);

        Assert.Equal(filters[0], Filter.EqualTo("tenantId", 1));
        Assert.Equal(filters[1], Filter.EqualTo("type", "SALE"));
        Assert.Equal(filters[2], Filter.EqualTo("transaction", "RENT"));
        Assert.Equal(filters[3], Filter.EqualTo("refId", "REFID"));
        Assert.Equal(filters[4], Filter.EqualTo("state", "SÃO PAULO"));
        Assert.Equal(filters[5], Filter.EqualTo("city", "Campinas"));
        Assert.Equal(filters[6], Filter.EqualTo("status", "ACTIVE"));

        Assert.Equal(filters[7], Filter.InArray("district", new List<string>{ "Vila Mariana", "Vila Madalena" }));
    }
}