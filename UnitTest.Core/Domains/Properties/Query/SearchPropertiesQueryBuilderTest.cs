using System.Collections.Generic;
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
                    Type = "House",
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
                    RentalTotalPriceFrom = 1000,
                    RentalTotalPriceTo = 3000,
                    RentalPriceFrom = 800,
                    RentalPriceTo = 1500,
                    PriceByM2From = 50,
                    PriceByM2To = 100,
                    Status = "Active"
                }
            };
            
            yield return new object[]
            {
                new SearchPropertiesQueryBuilderTestData
                {
                    Type = "ALL",
                    Transaction = "SALE",
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
                    RentalTotalPriceFrom = 0,
                    RentalTotalPriceTo = 0,
                    RentalPriceFrom = 0,
                    RentalPriceTo = 0,
                    PriceByM2From = 0,
                    PriceByM2To = 0,
                    Status = "None"
                }
            };
        }

        [Trait("Category", "Database collection [NoContext]")]
        [MemberData(nameof(SearchPropertiesQueryTestData))]
        [Theory(DisplayName = "Successful to build search properties query with test data")]
        public void SuccessfulToBuildSearchPropertiesQueryWithTestData(SearchPropertiesQueryBuilderTestData testData)
        {
            SearchPropertiesQueryBuilder builder = new SearchPropertiesQueryBuilder()
                .WithType(testData.Type)
                .WithTransaction(testData.Transaction)
                .WithFromNumberOfBedrooms((byte)testData.NumberOfBedroomsFrom)
                .WithToNumberOfBedrooms((byte)testData.NumberOfBedroomsTo)
                .WithFromNumberOfToilets((byte)testData.NumberOfToiletsFrom)
                .WithToNumberOfToilets((byte)testData.NumberOfToiletsTo)
                .WithFromNumberOfGarages((byte)testData.NumberOfGaragesFrom)
                .WithToNumberOfGarages((byte)testData.NumberOfGaragesTo)
                .WithCity(testData.City)
                .WithState(testData.State)
                .WithDistricts(testData.Districts)
                .WithFromArea(testData.AreaFrom)
                .WithToArea(testData.AreaTo)
                .WithFromBuiltArea(testData.BuiltAreaFrom)
                .WithToBuiltArea(testData.BuiltAreaTo)
                .WithFromSellingPrice(testData.SellingPriceFrom)
                .WithToSellingPrice(testData.SellingPriceTo)
                .WithFromRentalTotalPrice(testData.RentalTotalPriceFrom)
                .WithToRentalTotalPrice(testData.RentalTotalPriceTo)
                .WithFromRentalPrice(testData.RentalPriceFrom)
                .WithToRentalPrice(testData.RentalPriceTo)
                .WithFromPriceByM2(testData.PriceByM2From)
                .WithToPriceByM2(testData.PriceByM2To)
                .WithStatus(testData.Status);

            SearchPropertiesQuery actual = builder.Build();

            Assert.Equal(testData.Type.ToUpper(), actual.Type);
            Assert.Equal(testData.Transaction.ToUpper(), actual.Advertise.Transaction);
            Assert.Equal((byte)testData.NumberOfBedroomsFrom, actual.Attributes.NumberOfBedrooms.From);
            Assert.Equal((byte)testData.NumberOfBedroomsTo, actual.Attributes.NumberOfBedrooms.To);
            Assert.Equal((byte)testData.NumberOfToiletsFrom, actual.Attributes.NumberOfToilets.From);
            Assert.Equal((byte)testData.NumberOfToiletsTo, actual.Attributes.NumberOfToilets.To);
            Assert.Equal((byte)testData.NumberOfGaragesFrom, actual.Attributes.NumberOfGarages.From);
            Assert.Equal((byte)testData.NumberOfGaragesTo, actual.Attributes.NumberOfGarages.To);
            Assert.Equal(testData.City, actual.Location.City);
            Assert.Equal(testData.State, actual.Location.State);
            Assert.Equal(testData.Districts, actual.Location.Districts);
            Assert.Equal(testData.AreaFrom, actual.Attributes.Area.From);
            Assert.Equal(testData.AreaTo, actual.Attributes.Area.To);
            Assert.Equal(testData.BuiltAreaFrom, actual.Attributes.BuiltArea.From);
            Assert.Equal(testData.BuiltAreaTo, actual.Attributes.BuiltArea.To);
            Assert.Equal(testData.SellingPriceFrom, actual.Prices.SellingPrice.From);
            Assert.Equal(testData.SellingPriceTo, actual.Prices.SellingPrice.To);
            Assert.Equal(testData.RentalTotalPriceFrom, actual.Prices.RentalTotalPrice.From);
            Assert.Equal(testData.RentalTotalPriceTo, actual.Prices.RentalTotalPrice.To);
            Assert.Equal(testData.RentalPriceFrom, actual.Prices.RentalPrice.From);
            Assert.Equal(testData.RentalPriceTo, actual.Prices.RentalPrice.To);
            Assert.Equal(testData.PriceByM2From, actual.Prices.PriceByM2.From);
            Assert.Equal(testData.PriceByM2To, actual.Prices.PriceByM2.To);
            Assert.Equal(testData.Status.ToUpper(), actual.Status);
        }
    }

    public class SearchPropertiesQueryBuilderTestData
    {
        public string Type { get; init; }
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
        public int RentalTotalPriceFrom { get; init; }
        public int RentalTotalPriceTo { get; init; }
        public int RentalPriceFrom { get; init; }
        public int RentalPriceTo { get; init; }
        public int PriceByM2From { get; init; }
        public int PriceByM2To { get; init; }
        public string Status { get; init; }
    }
}
