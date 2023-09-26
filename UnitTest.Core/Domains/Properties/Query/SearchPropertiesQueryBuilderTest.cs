using System.Collections.Generic;
using System.Linq;
using Core.Domains.Properties.Query;
using Xunit;

namespace UnitTest.Core.Domains.Properties.Query
{
    public class SearchPropertiesQueryBuilderTests
    {
        public static IEnumerable<object[]> SearchPropertiesQueryTestData()
        {
            yield return new object[]
            {
                new SearchPropertiesQueryBuilderTestData
                {
                    Types = new List<string> {"House"},
                    Transaction = "Sale",
                    NumberOfBedroomsFrom = 2,
                    NumberOfBedroomsTo = 4,
                    NumberOfToiletsFrom = 1,
                    NumberOfToiletsTo = 3,
                    NumberOfGaragesFrom = 2,
                    NumberOfGaragesTo = 2,
                    City = "New York",
                    State = "NY",
                    Districts = new List<string> {"Downtown", "Midtown"},
                    AreaFrom = 150,
                    AreaTo = 300,
                    BuiltAreaFrom = 120,
                    BuiltAreaTo = 250,
                    SellingPriceFrom = 200000,
                    SellingPriceTo = 500000,
                    RentalPriceFrom = 800,
                    RentalPriceTo = 1500,
                    Status = "Active"
                }
            };

            yield return new object[]
            {
                new SearchPropertiesQueryBuilderTestData
                {
                    Types = null,
                    Transaction = null,
                    NumberOfBedroomsFrom = 0,
                    NumberOfBedroomsTo = 0,
                    NumberOfToiletsFrom = 0,
                    NumberOfToiletsTo = 0,
                    NumberOfGaragesFrom = 0,
                    NumberOfGaragesTo = 0,
                    City = "",
                    State = "",
                    Districts = new List<string>(),
                    AreaFrom = 0,
                    AreaTo = 0,
                    BuiltAreaFrom = 0,
                    BuiltAreaTo = 0,
                    SellingPriceFrom = 0,
                    SellingPriceTo = 0,
                    RentalPriceFrom = 0,
                    RentalPriceTo = 0,
                    Status = null
                }
            };
        }

        [Trait("Category", "Database collection [NoContext]")]
        [MemberData(nameof(SearchPropertiesQueryTestData))]
        [Theory(DisplayName = "Successful to build search properties query with test data")]
        public void SuccessfulToBuildSearchPropertiesQueryWithTestData(SearchPropertiesQueryBuilderTestData testData)
        {
            SearchPropertiesQueryBuilder builder = new SearchPropertiesQueryBuilder()
                .WithTypes(testData.Types)
                .WithTransaction(testData.Transaction)
                .WithMinBedrooms((byte) testData.NumberOfBedroomsFrom)
                .WithMaxBedrooms((byte) testData.NumberOfBedroomsTo)
                .WithMinToilets((byte) testData.NumberOfToiletsFrom)
                .WithMaxToilets((byte) testData.NumberOfToiletsTo)
                .WithMinGarages((byte) testData.NumberOfGaragesFrom)
                .WithMaxGarages((byte) testData.NumberOfGaragesTo)
                .WithCity(testData.City)
                .WithState(testData.State)
                .WithDistricts(testData.Districts)
                .WithFromArea(testData.AreaFrom)
                .WithToArea(testData.AreaTo)
                .WithMinBuiltArea(testData.BuiltAreaFrom)
                .WithMaxBuiltArea(testData.BuiltAreaTo)
                .WithMinSellingPrice(testData.SellingPriceFrom)
                .WithToSellingPrice(testData.SellingPriceTo)
                .WithFromRentalPrice(testData.RentalPriceFrom)
                .WithToRentalPrice(testData.RentalPriceTo)
                .WithStatus(testData.Status);

            SearchPropertiesQuery actual = builder.Build();

            Assert.Equal(testData.Types is null ? new List<string> {"ALL"} : testData.Types.Select(type => type.ToUpper()), actual.Types);
            Assert.Equal(testData.Transaction is null ? "ALL" : testData.Transaction.ToUpper(), actual.Advertise.Transaction);
            Assert.Equal(testData.Status is null ? "ALL" : testData.Status.ToUpper(), actual.Status);
            Assert.Equal((byte) testData.NumberOfBedroomsFrom, actual.Attributes.NumberOfBedrooms.From);
            Assert.Equal((byte) testData.NumberOfBedroomsTo, actual.Attributes.NumberOfBedrooms.To);
            Assert.Equal((byte) testData.NumberOfToiletsFrom, actual.Attributes.NumberOfToilets.From);
            Assert.Equal((byte) testData.NumberOfToiletsTo, actual.Attributes.NumberOfToilets.To);
            Assert.Equal((byte) testData.NumberOfGaragesFrom, actual.Attributes.NumberOfGarages.From);
            Assert.Equal((byte) testData.NumberOfGaragesTo, actual.Attributes.NumberOfGarages.To);
            Assert.Equal(testData.City, actual.Location.City);
            Assert.Equal(testData.State, actual.Location.State);
            Assert.Equal(testData.Districts, actual.Location.Districts);
            Assert.Equal(testData.AreaFrom, actual.Attributes.Area.From);
            Assert.Equal(testData.AreaTo, actual.Attributes.Area.To);
            Assert.Equal(testData.BuiltAreaFrom, actual.Attributes.BuiltArea.From);
            Assert.Equal(testData.BuiltAreaTo, actual.Attributes.BuiltArea.To);
            Assert.Equal(testData.SellingPriceFrom, actual.Prices.SellingPrice.From);
            Assert.Equal(testData.SellingPriceTo, actual.Prices.SellingPrice.To);
            Assert.Equal(testData.RentalPriceFrom, actual.Prices.RentalPrice.From);
            Assert.Equal(testData.RentalPriceTo, actual.Prices.RentalPrice.To);
        }
    }

    public class SearchPropertiesQueryBuilderTestData
    {
        public List<string> Types { get; init; }
        public string Transaction { get; init; }
        public int NumberOfBedroomsFrom { get; init; }
        public int NumberOfBedroomsTo { get; init; }
        public int NumberOfToiletsFrom { get; init; }
        public int NumberOfToiletsTo { get; init; }
        public int NumberOfGaragesFrom { get; init; }
        public int NumberOfGaragesTo { get; init; }
        public string City { get; init; }
        public string State { get; init; }
        public List<string> Districts { get; init; }
        public int AreaFrom { get; init; }
        public int AreaTo { get; init; }
        public int BuiltAreaFrom { get; init; }
        public int BuiltAreaTo { get; init; }
        public int SellingPriceFrom { get; init; }
        public int SellingPriceTo { get; init; }
        public int RentalPriceFrom { get; init; }
        public int RentalPriceTo { get; init; }
        public string Status { get; init; }
    }
}