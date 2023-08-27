using Xunit;
using System.Collections.Generic;
using Core.Domains.Properties.Query;
using Google.Cloud.Firestore;
using Infra.Firestore.Domain.Properties.Repository;
using IntegrationTest.Infra.Firestore.Commons.Connection;
using Xunit.Abstractions;

namespace IntegrationTest.Infra.Firestore;

public class SearchPropertiesQueryBuilderTest
{
    private static readonly FirestoreDb FirestoreDb = FirestoreDb.Create("api", new FakeFirestoreClient());
    private static Query GetEmptyQuery() => FirestoreDb.Collection("properties");

    private readonly ITestOutputHelper _outputHelper;

    public SearchPropertiesQueryBuilderTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Trait("Category", "Database collection [NoContext]")]
    [Fact(DisplayName = "Successfully to build filter builder from query parameters")]
    public void SuccessfullyToBuildFilterBuilderFromQueryParameters()
    {
        // Arrange
        SearchPropertiesQueryBuilder queryBuilder = new SearchPropertiesQueryBuilder();
        queryBuilder.WithType("SALE");
        queryBuilder.WithTransaction("RENT");
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
        queryBuilder.WithFromPriceByM2(1000);
        queryBuilder.WithToPriceByM2(2000);
        queryBuilder.WithStatus("ACTIVE");

        // Act
        Query query = SearchPropertiesFirestoreQueryBuilder.BuildFromQuery(GetEmptyQuery(), queryBuilder.Build());

        // Assert
        _outputHelper.WriteLine($"query: {query}");
    }
}
