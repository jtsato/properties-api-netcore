using System;
using Xunit;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Core.Domains.Properties.Query;
using Google.Cloud.Firestore;
using Infra.Firestore.Domain.Properties.Repository;
using IntegrationTest.Infra.Firestore.Commons.Connection;
using Xunit.Abstractions;
using Timestamp = Google.Cloud.Firestore.Timestamp;

namespace IntegrationTest.Infra.Firestore;

public class SearchPropertiesFilterBuilderTest
{
    private static readonly FirestoreDb FirestoreDb = FirestoreDb.Create("api", new FakeFirestoreClient());
    private static Query GetEmptyQuery() => FirestoreDb.Collection("properties");

    private readonly ITestOutputHelper _outputHelper;

    public SearchPropertiesFilterBuilderTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

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
        queryBuilder.WithFromCreatedAt("2023-02-28 00:00:00.001");
        queryBuilder.WithToCreatedAt("2023-04-30 23:59:59.999");
        queryBuilder.WithFromUpdatedAt("2023-02-28 00:00:00.001");
        queryBuilder.WithToUpdatedAt("2023-04-30 23:59:59.999");

        // Act
        IEnumerable<Filter> enumerable = SearchPropertiesFilterBuilder.BuildFromQuery(queryBuilder.Build());

        // Assert
        Filter[] filters = enumerable as Filter[] ?? enumerable.ToArray();

        _outputHelper.WriteLine($"Filters: {filters.Length}");

        Assert.True(filters.Length == 32);

        Assert.Equal(ToQuery(Filter.EqualTo("tenantId", 1)), ToQuery(filters[0]));
        Assert.Equal(ToQuery(Filter.EqualTo("type", "SALE")), ToQuery(filters[1]));
        Assert.Equal(ToQuery(Filter.EqualTo("transaction", "RENT")), ToQuery(filters[2]));
        Assert.Equal(ToQuery(Filter.EqualTo("refId", "REFID")), ToQuery(filters[3]));
        Assert.Equal(ToQuery(Filter.EqualTo("state", "SÃO PAULO")), ToQuery(filters[4]));
        Assert.Equal(ToQuery(Filter.EqualTo("city", "Campinas")), ToQuery(filters[5]));
        Assert.Equal(ToQuery(Filter.EqualTo("status", "ACTIVE")), ToQuery(filters[6]));
        
        Assert.Equal(ToQuery(Filter.InArray("district", new List<string> {"Vila Mariana", "Vila Madalena"}) ), ToQuery(filters[7]));
        
        Assert.Equal(ToQuery(Filter.GreaterThan("numberOfBedrooms", 1)), ToQuery(filters[8]));
        Assert.Equal(ToQuery(Filter.LessThan("numberOfBedrooms", 2)), ToQuery(filters[9]));
        Assert.Equal(ToQuery(Filter.GreaterThan("numberOfToilets", 3)), ToQuery(filters[10]));
        Assert.Equal(ToQuery(Filter.LessThan("numberOfToilets", 4)), ToQuery(filters[11]));
        Assert.Equal(ToQuery(Filter.GreaterThan("numberOfGarages", 5)), ToQuery(filters[12]));
        Assert.Equal(ToQuery(Filter.LessThan("numberOfGarages", 6)), ToQuery(filters[13]));
        Assert.Equal(ToQuery(Filter.GreaterThan("area", 100)), ToQuery(filters[14]));
        Assert.Equal(ToQuery(Filter.LessThan("area", 200)), ToQuery(filters[15]));
        Assert.Equal(ToQuery(Filter.GreaterThan("builtArea", 10)), ToQuery(filters[16]));
        Assert.Equal(ToQuery(Filter.LessThan("builtArea", 20)), ToQuery(filters[17]));
        Assert.Equal(ToQuery(Filter.GreaterThan("sellingPrice", (double) 100000)), ToQuery(filters[18]));
        Assert.Equal(ToQuery(Filter.LessThan("sellingPrice", (double) 200000)), ToQuery(filters[19]));
        Assert.Equal(ToQuery(Filter.GreaterThan("rentalTotalPrice", (double) 1000)), ToQuery(filters[20]));
        Assert.Equal(ToQuery(Filter.LessThan("rentalTotalPrice", (double) 2000)), ToQuery(filters[21]));
        Assert.Equal(ToQuery(Filter.GreaterThan("rentalPrice", (double) 1000)), ToQuery(filters[22]));
        Assert.Equal(ToQuery(Filter.LessThan("rentalPrice", (double) 2000)), ToQuery(filters[23]));
        Assert.Equal(ToQuery(Filter.GreaterThan("priceByM2", (double) 1000)), ToQuery(filters[24]));
        Assert.Equal(ToQuery(Filter.LessThan("priceByM2", (double) 2000)), ToQuery(filters[25]));
        Assert.Equal(ToQuery(Filter.GreaterThan("ranking", 1)), ToQuery(filters[26]));
        Assert.Equal(ToQuery(Filter.LessThan("ranking", 2)), ToQuery(filters[27]));
        
        Assert.Equal(ToQuery(Filter.GreaterThanOrEqualTo("createdAt", ToTimestamp("2023-02-28 00:00:00.001"))), ToQuery(filters[28]));
        Assert.Equal(ToQuery(Filter.LessThanOrEqualTo("createdAt", ToTimestamp("2023-04-30 23:59:59.999"))), ToQuery(filters[29]));
        Assert.Equal(ToQuery(Filter.GreaterThanOrEqualTo("updatedAt", ToTimestamp("2023-02-28 00:00:00.001"))), ToQuery(filters[30]));
        Assert.Equal(ToQuery(Filter.LessThanOrEqualTo("updatedAt", ToTimestamp("2023-04-30 23:59:59.999"))), ToQuery(filters[31]));
    }
    
    private static Query ToQuery(Filter filter)
    {
        return GetEmptyQuery().Where(filter);
    }
    
    private static Timestamp ToTimestamp(string date)
    {
        DateTime dateTime = DateTime.Parse(date, new CultureInfo("pt-BR")).ToUniversalTime();
        return Timestamp.FromDateTime(dateTime);
    }
}
