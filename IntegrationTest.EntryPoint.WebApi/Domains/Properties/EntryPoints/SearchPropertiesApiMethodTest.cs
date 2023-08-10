using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Commons.Paging;
using Core.Domains.Properties.Models;
using Core.Domains.Properties.Query;
using Core.Domains.Properties.UseCases;
using EntryPoint.WebApi.Commons;
using EntryPoint.WebApi.Commons.Models;
using EntryPoint.WebApi.Domains.Properties.EntryPoints;
using EntryPoint.WebApi.Domains.Properties.Models;
using IntegrationTest.EntryPoint.WebApi.Commons;
using IntegrationTest.EntryPoint.WebApi.Commons.Assertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.EntryPoint.WebApi.Domains.Properties.EntryPoints;

[Collection("WebApi Collection")]
public class SearchPropertiesApiMethodTest
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly ApiMethodInvoker _invoker;
    private readonly SearchPropertiesApiMethod _apiMethod;
    private readonly Mock<ISearchPropertiesUseCase> _useCaseMock;

    public SearchPropertiesApiMethodTest(ITestOutputHelper outputHelper, ApiMethodInvokerHolder apiMethodInvokerHolder)
    {
        _outputHelper = outputHelper;
        _invoker = apiMethodInvokerHolder.GetApiMethodInvoker();
        _useCaseMock = new Mock<ISearchPropertiesUseCase>(MockBehavior.Strict);
        SearchPropertiesController controller = new SearchPropertiesController(
            httpContextAccessor: BuildHttpContextAccessorMock().Object,
            useCase: _useCaseMock.Object
        );
        _apiMethod = new SearchPropertiesApiMethod(controller);
    }

    private static Mock<IHttpContextAccessor> BuildHttpContextAccessorMock()
    {
        Mock<IHttpContextAccessor> httpContextAccessorMock = new Mock<IHttpContextAccessor>(MockBehavior.Strict);

        httpContextAccessorMock
            .Setup(contextAccessor => contextAccessor.HttpContext.Request.Scheme)
            .Returns("http");

        httpContextAccessorMock
            .Setup(contextAccessor => contextAccessor.HttpContext.Request.Host)
            .Returns(new HostString("localhost", 7029));

        httpContextAccessorMock
            .Setup(contextAccessor => contextAccessor.HttpContext.Request.PathBase)
            .Returns("/api/properties-search");

        httpContextAccessorMock
            .Setup(contextAccessor => contextAccessor.HttpContext.Request.Path)
            .Returns("/v1/properties");

        return httpContextAccessorMock;
    }

    [Trait("Category", "Entrypoint (WebApi) Integration tests")]
    [Fact(DisplayName = "GET /api/properties/search should return 200")]
    public async Task SuccessfulToSearchProperties()
    {
        // Arrange
        SearchPropertiesQueryBuilder queryBuilder = new SearchPropertiesQueryBuilder();

        queryBuilder
            .WithType("Apartment")
            .WithTransaction("Rent")
            .WithRefId("REF 101")
            .WithCity("White Duck")
            .WithDistricts(new List<string> {"Downtown", "Alta Vista"})
            .WithFromNumberOfBedrooms(0)
            .WithToNumberOfBedrooms(3)
            .WithFromNumberOfToilets(1)
            .WithToNumberOfToilets(2)
            .WithFromNumberOfGarages(1)
            .WithToNumberOfGarages(1)
            .WithFromSellingPrice(100000)
            .WithToSellingPrice(200000)
            .WithFromRentalTotalPrice(1000)
            .WithToRentalTotalPrice(3000)
            .WithFromRentalPrice(1000)
            .WithToRentalPrice(2000)
            .WithFromPriceByM2(100)
            .WithToPriceByM2(200)
            .WithFromCreatedAt("2023-01-01")
            .WithToCreatedAt("2023-02-01")
            .WithFromUpdatedAt("2023-02-01")
            .WithToUpdatedAt("2023-03-01");

        SearchPropertiesQuery query = queryBuilder.Build();

        _useCaseMock
            .Setup(useCase => useCase.ExecuteAsync(query, PageRequestHelper.Of("0", "1", "UpdatedDate:Desc")))
            .ReturnsAsync(
                new Page<Property>(new List<Property>
                    {
                        new Property
                        {
                            Id = Guid.NewGuid().ToString(),
                            Type = PropertyType.Apartment,
                            Advertise = new PropertyAdvertise
                            {
                                TenantId = 1,
                                Transaction = Transaction.Rent,
                                Title = "Apartment for rent",
                                Description = "Apartment for rent",
                                Url = "https://www.apartment-for-rent.com",
                                RefId = "REF 101",
                                Images = new List<string>
                                {
                                    "https://www.apartment-for-rent.com/image1.jpg",
                                    "https://www.apartment-for-rent.com/image2.jpg"
                                }
                            },
                            Attributes = new PropertyAttributes
                            {
                                NumberOfBedrooms = 2,
                                NumberOfToilets = 1,
                                NumberOfGarages = 1,
                                Area = 100,
                                BuiltArea = 200,
                            },
                            Location = new PropertyLocation
                            {
                                City = "White Duck",
                                District = "Downtown",
                                Address = "Good Life Street, 101",
                            },
                            Prices = new PropertyPrices
                            {
                                SellingPrice = 100000,
                                RentalTotalPrice = 1000,
                                RentalPrice = 1000,
                                PriceByM2 = 100,
                                Discount = 100,
                                CondominiumFee = 90,
                            },
                            HashKey = "hash-key-1",
                            CreatedAt = DateTime.Parse("2023-01-01 23:59:59.999", new CultureInfo("pt-BR")),
                            UpdatedAt = DateTime.Parse("2023-02-01 23:59:59.999", new CultureInfo("pt-BR")),
                        }
                    },
                    new Pageable(0, 1, 1, 2, 2))
            );

        SearchPropertiesRequest request = new SearchPropertiesRequest
        {
            Type = "Apartment",
            Transaction = "Rent",
            RefId = "REF 101",
            City = "White Duck",
            Districts = new List<string> {"Downtown", "Alta Vista"},
            NumberOfBedroomsMin = 0,
            NumberOfBedroomsMax = 3,
            NumberOfToiletsMin = 1,
            NumberOfToiletsMax = 2,
            NumberOfGaragesMin = 1,
            NumberOfGaragesMax = 1,
            SellingPriceMin = 100000,
            SellingPriceMax = 200000,
            RentalTotalPriceMin = 1000,
            RentalTotalPriceMax = 3000,
            RentalPriceMin = 1000,
            RentalPriceMax = 2000,
            PriceByM2Min = 100,
            PriceByM2Max = 200,
            FromCreatedAt = "2023-01-01",
            ToCreatedAt = "2023-02-01",
            FromUpdatedAt = "2023-02-01",
            ToUpdatedAt = "2023-03-01"
        };

        QPageRequest qPageRequest = new QPageRequest
        {
            PageNumber = "0",
            PageSize = "1",
            OrderBy = new List<string> {"UpdatedDate,Desc"}
        };
        // Act
        ObjectResult objectResult = await _invoker.InvokeAsync(() => _apiMethod.SearchProperties(request, qPageRequest));

        // Assert
        Assert.NotNull(objectResult);
        Assert.Equal((int) HttpStatusCode.PartialContent, objectResult.StatusCode);

        JsonElement jsonElement = ApiMethodTestHelper.TryGetJsonElement(objectResult);

        _outputHelper.WriteLine(jsonElement.ToString());

        JsonAssertHelper.AssertThat(jsonElement)
            .AndExpectThat(JsonFrom.Path("$.content"), Is<object>.Single())
            .AndExpectThat(JsonFrom.Path("$.content[0].id"), Is<string>.NotEmpty())
            .AndExpectThat(JsonFrom.Path("$.content[0].tenantId"), Is<int>.EqualTo(1))
            .AndExpectThat(JsonFrom.Path("$.content[0].transaction"), Is<string>.EqualTo("Rent"))
            .AndExpectThat(JsonFrom.Path("$.content[0].title"), Is<string>.EqualTo("Apartment for rent"))
            .AndExpectThat(JsonFrom.Path("$.content[0].description"), Is<string>.EqualTo("Apartment for rent"))
            .AndExpectThat(JsonFrom.Path("$.content[0].url"), Is<string>.EqualTo("https://www.apartment-for-rent.com"))
            .AndExpectThat(JsonFrom.Path("$.content[0].refId"), Is<string>.EqualTo("REF 101"))
            .AndExpectThat(JsonFrom.Path("$.content[0].images"), Is<List<string>>.EqualTo(new List<string>
            {
                "https://www.apartment-for-rent.com/image1.jpg",
                "https://www.apartment-for-rent.com/image2.jpg"
            }))
            .AndExpectThat(JsonFrom.Path("$.content[0].numberOfBedrooms"), Is<int>.EqualTo(2))
            .AndExpectThat(JsonFrom.Path("$.content[0].numberOfToilets"), Is<int>.EqualTo(1))
            .AndExpectThat(JsonFrom.Path("$.content[0].numberOfGarages"), Is<int>.EqualTo(1))
            .AndExpectThat(JsonFrom.Path("$.content[0].area"), Is<byte>.EqualTo(100))
            .AndExpectThat(JsonFrom.Path("$.content[0].builtArea"), Is<int>.EqualTo(200))
            .AndExpectThat(JsonFrom.Path("$.content[0].city"), Is<string>.EqualTo("White Duck"))
            .AndExpectThat(JsonFrom.Path("$.content[0].district"), Is<string>.EqualTo("Downtown"))
            .AndExpectThat(JsonFrom.Path("$.content[0].address"), Is<string>.EqualTo("Good Life Street, 101"))
            .AndExpectThat(JsonFrom.Path("$.content[0].sellingPrice"), Is<int>.EqualTo(100000))
            .AndExpectThat(JsonFrom.Path("$.content[0].rentalTotalPrice"), Is<int>.EqualTo(1000))
            .AndExpectThat(JsonFrom.Path("$.content[0].rentalPrice"), Is<int>.EqualTo(1000))
            .AndExpectThat(JsonFrom.Path("$.content[0].priceByM2"), Is<int>.EqualTo(100))
            .AndExpectThat(JsonFrom.Path("$.content[0].discount"), Is<int>.EqualTo(100))
            .AndExpectThat(JsonFrom.Path("$.content[0].condominiumFee"), Is<int>.EqualTo(90))
            .AndExpectThat(JsonFrom.Path("$.content[0].createdAt"), Is<string>.EqualTo("2023-01-01T23:59:59.999"))
            .AndExpectThat(JsonFrom.Path("$.content[0].updatedAt"), Is<string>.EqualTo("2023-02-01T23:59:59.999"))
            .AndExpectThat(JsonFrom.Path("$.content[0].href"), Is<string>.StartWith("http://localhost:7029/api/properties-search/v1/properties/"))
            .AndExpectThat(JsonFrom.Path("$.pageable.page"), Is<int>.EqualTo(0))
            .AndExpectThat(JsonFrom.Path("$.pageable.size"), Is<int>.EqualTo(1))
            .AndExpectThat(JsonFrom.Path("$.pageable.numberOfElements"), Is<int>.EqualTo(1))
            .AndExpectThat(JsonFrom.Path("$.pageable.totalOfElements"), Is<int>.EqualTo(2))
            .AndExpectThat(JsonFrom.Path("$.pageable.totalPages"), Is<int>.EqualTo(2));
    }
}