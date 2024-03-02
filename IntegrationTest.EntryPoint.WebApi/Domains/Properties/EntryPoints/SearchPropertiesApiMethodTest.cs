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
using EntryPoint.WebApi.Domains.Commons;
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

[Collection("WebApi Collection Context")]
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
        ISearchPropertiesController controller = new SearchPropertiesController(
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

    [UseCulture("en-US")]
    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "GET /api/properties/search should return 500 Internal Server Error when use case throws an exception")]
    public async Task FailToSearchPropertiesReturningInternalServerError()
    {
        // Arrange
        SearchPropertiesQueryBuilder builder = new SearchPropertiesQueryBuilder();

        builder
            .WithTypes(new List<string> {"Apartment"})
            .WithTransaction("Rent")
            .WithState("Duckland")
            .WithCity("White Duck")
            .WithDistricts(new List<string> {"Downtown", "Alta Vista"})
            .WithMinBedrooms(0)
            .WithMaxBedrooms(3)
            .WithMinToilets(1)
            .WithMaxToilets(2)
            .WithMinGarages(1)
            .WithMaxGarages(1)
            .WithFromArea(100)
            .WithToArea(200)
            .WithMinBuiltArea(80)
            .WithMaxBuiltArea(160)
            .WithMinSellingPrice(100000)
            .WithToSellingPrice(200000)
            .WithFromRentalPrice(500)
            .WithToRentalPrice(900)
            .WithStatus("Active");

        SearchPropertiesQuery query = builder.Build();

        _useCaseMock
            .Setup(useCase => useCase.ExecuteAsync(query, PageRequestHelper.Of("0", "1", "updatedAt:DESC")))
            .ThrowsAsync(new Exception("Unexpected error"));

        SearchPropertiesRequest request = new SearchPropertiesRequest
        {
            Types = new List<string> {"Apartment"},
            Transaction = "Rent",
            Uf = "Duckland",
            City = "White Duck",
            Districts = new List<string> {"Downtown", "Alta Vista"},
            MinBedrooms = 0,
            MaxBedrooms = 3,
            MinToilets = 1,
            MaxToilets = 2,
            MinGarages = 1,
            MaxGarages = 1,
            MinArea = 100,
            MaxArea = 200,
            MinBuiltArea = 80,
            MaxBuiltArea = 160,
            MinPrice = 100000,
            MaxPrice = 200000,
            Status = "Active"
        };

        QPageRequest qPageRequest = new QPageRequest
        {
            PageNumber = "0",
            PageSize = "1",
            OrderBy = new List<string> {"updatedAt,Desc"}
        };

        // Act
        ObjectResult objectResult = await _invoker.InvokeAsync(() => _apiMethod.SearchProperties(request, qPageRequest));

        // Assert
        Assert.NotNull(objectResult);
        Assert.Equal((int) HttpStatusCode.InternalServerError, objectResult.StatusCode);

        JsonElement jsonElement = ApiMethodTestHelper.TryGetJsonElement(objectResult);

        const string errorMessage = "An unexpected error has occurred, please try again later!";

        JsonAssertHelper.AssertThat(jsonElement)
            .AndExpectThat(JsonFrom.Path("$.code"), Is<int>.EqualTo(500))
            .AndExpectThat(JsonFrom.Path("$.message"), Is<string>.EqualTo(errorMessage))
            .AndExpectThat(JsonFrom.Path("$.fields"), Is<object>.Empty());
    }

    [UseCulture("en-US")]
    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "GET /api/properties/search should return 206 when there is partial content")]
    public async Task SuccessfulToSearchPropertiesReturningPartialContent()
    {
        // Arrange
        SearchPropertiesQueryBuilder builder = new SearchPropertiesQueryBuilder();

        builder
            .WithTypes(new List<string> {"Apartment"})
            .WithTransaction("Rent")
            .WithState("Duckland")
            .WithCity("White Duck")
            .WithState("Duckland")
            .WithDistricts(new List<string> {"Downtown", "Alta Vista"})
            .WithMinBedrooms(0)
            .WithMaxBedrooms(3)
            .WithMinToilets(1)
            .WithMaxToilets(2)
            .WithMinGarages(1)
            .WithMaxGarages(1)
            .WithFromArea(100)
            .WithToArea(200)
            .WithMinBuiltArea(80)
            .WithMaxBuiltArea(160)
            .WithMinSellingPrice(0)
            .WithToSellingPrice(999999999)
            .WithFromRentalPrice(100000)
            .WithToRentalPrice(200000)
            .WithStatus("Active");

        SearchPropertiesQuery query = builder.Build();

        _useCaseMock
            .Setup(useCase => useCase.ExecuteAsync(query, PageRequestHelper.Of("0", "1", "updatedAt:DESC")))
            .ReturnsAsync(
                new Page<Property>(new List<Property>
                    {
                        new Property
                        {
                            Id = 1001,
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
                                State = "Duckland",
                                City = "White Duck",
                                District = "Downtown",
                                Address = "Good Life Street, 101",
                            },
                            Prices = new PropertyPrices
                            {
                                SellingPrice = 100000,
                                RentalTotalPrice = 1000,
                                RentalPrice = 700,
                                PriceByM2 = 100,
                                Discount = 100,
                                CondominiumFee = 90,
                            },
                            HashKey = "hash-key-1",
                            Ranking = 1,
                            Status = PropertyStatus.Active,
                            CreatedAt = DateTime.Parse("2023-01-01 23:59:59.999", CultureInfo.DefaultThreadCurrentCulture),
                            UpdatedAt = DateTime.Parse("2023-02-01 23:59:59.999", CultureInfo.DefaultThreadCurrentCulture),
                        }
                    },
                    new Pageable(0, 1, 1, 2, 2))
            );

        SearchPropertiesRequest request = new SearchPropertiesRequest
        {
            Types = new List<string> {"Apartment"},
            Transaction = "Rent",
            Uf = "Duckland",
            City = "White Duck",
            Districts = new List<string> {"Downtown", "Alta Vista"},
            MinBedrooms = 0,
            MaxBedrooms = 3,
            MinToilets = 1,
            MaxToilets = 2,
            MinGarages = 1,
            MaxGarages = 1,
            MinArea = 100,
            MaxArea = 200,
            MinBuiltArea = 80,
            MaxBuiltArea = 160,
            MinPrice = 100000,
            MaxPrice = 200000,
            Status = "Active"
        };

        QPageRequest qPageRequest = new QPageRequest
        {
            PageNumber = "0",
            PageSize = "1",
            OrderBy = new List<string> {"updatedAt,Desc"}
        };

        // Act
        ObjectResult objectResult = await _invoker.InvokeAsync(() => _apiMethod.SearchProperties(request, qPageRequest), "en-US");

        // Assert
        Assert.NotNull(objectResult);
        Assert.Equal((int) HttpStatusCode.PartialContent, objectResult.StatusCode);

        JsonElement jsonElement = ApiMethodTestHelper.TryGetJsonElement(objectResult);

        _outputHelper.WriteLine(jsonElement.ToString());

        JsonAssertHelper.AssertThat(jsonElement)
            .AndExpectThat(JsonFrom.Path("$.content"), Is<object>.Single())
            .AndExpectThat(JsonFrom.Path("$.content[0].id"), Is<long>.EqualTo(1001))
            .AndExpectThat(JsonFrom.Path("$.content[0].tenantId"), Is<int>.EqualTo(1))
            .AndExpectThat(JsonFrom.Path("$.content[0].transaction"), Is<string>.EqualTo("RENT"))
            .AndExpectThat(JsonFrom.Path("$.content[0].title"), Is<string>.EqualTo("Apartment for rent"))
            .AndExpectThat(JsonFrom.Path("$.content[0].introduction"), Is<string>.EqualTo("Apartment for rent..."))
            .AndExpectThat(JsonFrom.Path("$.content[0].url"), Is<string>.EqualTo("https://www.apartment-for-rent.com"))
            .AndExpectThat(JsonFrom.Path("$.content[0].refId"), Is<string>.EqualTo("REF 101"))
            .AndExpectThat(JsonFrom.Path("$.content[0].coverImage"), Is<string>.EqualTo("https://www.apartment-for-rent.com/image1.jpg"))
            .AndExpectThat(JsonFrom.Path("$.content[0].numberOfBedrooms"), Is<int>.EqualTo(2))
            .AndExpectThat(JsonFrom.Path("$.content[0].numberOfToilets"), Is<int>.EqualTo(1))
            .AndExpectThat(JsonFrom.Path("$.content[0].numberOfGarages"), Is<int>.EqualTo(1))
            .AndExpectThat(JsonFrom.Path("$.content[0].area"), Is<byte>.EqualTo(100))
            .AndExpectThat(JsonFrom.Path("$.content[0].builtArea"), Is<int>.EqualTo(200))
            .AndExpectThat(JsonFrom.Path("$.content[0].state"), Is<string>.EqualTo("Duckland"))
            .AndExpectThat(JsonFrom.Path("$.content[0].city"), Is<string>.EqualTo("White Duck"))
            .AndExpectThat(JsonFrom.Path("$.content[0].district"), Is<string>.EqualTo("Downtown"))
            .AndExpectThat(JsonFrom.Path("$.content[0].address"), Is<string>.EqualTo("Good Life Street, 101"))
            .AndExpectThat(JsonFrom.Path("$.content[0].sellingPrice"), Is<int>.EqualTo(100000))
            .AndExpectThat(JsonFrom.Path("$.content[0].rentalTotalPrice"), Is<int>.EqualTo(1000))
            .AndExpectThat(JsonFrom.Path("$.content[0].rentalPrice"), Is<int>.EqualTo(700))
            .AndExpectThat(JsonFrom.Path("$.content[0].priceByM2"), Is<int>.EqualTo(100))
            .AndExpectThat(JsonFrom.Path("$.content[0].discount"), Is<int>.EqualTo(100))
            .AndExpectThat(JsonFrom.Path("$.content[0].condominiumFee"), Is<int>.EqualTo(90))
            .AndExpectThat(JsonFrom.Path("$.content[0].ranking"), Is<int>.EqualTo(1))
            .AndExpectThat(JsonFrom.Path("$.content[0].status"), Is<string>.EqualTo("ACTIVE"))
            .AndExpectThat(JsonFrom.Path("$.content[0].createdAt"), Is<string>.EqualTo("2023-01-01T23:59:59.999"))
            .AndExpectThat(JsonFrom.Path("$.content[0].updatedAt"), Is<string>.EqualTo("2023-02-01T23:59:59.999"))
            .AndExpectThat(JsonFrom.Path("$.content[0].href"), Is<string>.EqualTo("http://localhost:7029/api/properties-search/v1/properties/1001"))
            .AndExpectThat(JsonFrom.Path("$.pageable.page"), Is<int>.EqualTo(0))
            .AndExpectThat(JsonFrom.Path("$.pageable.size"), Is<int>.EqualTo(1))
            .AndExpectThat(JsonFrom.Path("$.pageable.numberOfElements"), Is<int>.EqualTo(1))
            .AndExpectThat(JsonFrom.Path("$.pageable.totalOfElements"), Is<int>.EqualTo(2))
            .AndExpectThat(JsonFrom.Path("$.pageable.totalPages"), Is<int>.EqualTo(2));
    }

    [UseCulture("en-US")]
    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "GET /api/properties/search should return 204 when there is no content")]
    public async Task SuccessfulToSearchPropertiesReturningNoContent()
    {
        // Arrange
        SearchPropertiesQueryBuilder builder = new SearchPropertiesQueryBuilder();

        builder
            .WithTypes(new List<string> {"Apartment"})
            .WithTransaction("Rent")
            .WithState("Duckland")
            .WithCity("White Duck")
            .WithDistricts(new List<string> {"Downtown", "Alta Vista"})
            .WithMinBedrooms(0)
            .WithMaxBedrooms(3)
            .WithMinToilets(1)
            .WithMaxToilets(2)
            .WithMinGarages(1)
            .WithMaxGarages(1)
            .WithFromArea(100)
            .WithToArea(200)
            .WithMinBuiltArea(80)
            .WithMaxBuiltArea(160)
            .WithMinSellingPrice(0)
            .WithToSellingPrice(999999999)
            .WithFromRentalPrice(100000)
            .WithToRentalPrice(200000)
            .WithStatus("Active");

        SearchPropertiesQuery query = builder.Build();

        _useCaseMock
            .Setup(useCase => useCase.ExecuteAsync(query, PageRequestHelper.Of("0", "1", "updatedAt:DESC")))
            .ReturnsAsync(
                new Page<Property>(new List<Property>(), new Pageable(0, 1, 0, 0, 0))
            );

        SearchPropertiesRequest request = new SearchPropertiesRequest
        {
            Types = new List<string> {"Apartment"},
            Transaction = "Rent",
            Uf = "Duckland",
            City = "White Duck",
            Districts = new List<string> {"Downtown", "Alta Vista"},
            MinBedrooms = 0,
            MaxBedrooms = 3,
            MinToilets = 1,
            MaxToilets = 2,
            MinGarages = 1,
            MaxGarages = 1,
            MinArea = 100,
            MaxArea = 200,
            MinBuiltArea = 80,
            MaxBuiltArea = 160,
            MinPrice = 100000,
            MaxPrice = 200000,
            Status = "Active"
        };

        QPageRequest qPageRequest = new QPageRequest
        {
            PageNumber = "0",
            PageSize = "1",
            OrderBy = new List<string> {"updatedAt,Desc"}
        };

        // Act
        ObjectResult objectResult = await _invoker.InvokeAsync(() => _apiMethod.SearchProperties(request, qPageRequest), "en-US");

        // Assert
        Assert.NotNull(objectResult);
        Assert.Equal((int) HttpStatusCode.NoContent, objectResult.StatusCode);
    }
    
    [UseCulture("en-US")]
    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "GET /api/properties/search should return 204 when there is no content")]
    public async Task SuccessfulToSearchPropertiesReturningNoContent2()
    {
        // Arrange
        SearchPropertiesQueryBuilder builder = new SearchPropertiesQueryBuilder();

        builder
            .WithTypes(new List<string> {"Apartment"})
            .WithTransaction("Sale")
            .WithState("Duckland")
            .WithCity("White Duck")
            .WithDistricts(new List<string>(0))
            .WithMinBedrooms(0)
            .WithMaxBedrooms(3)
            .WithMinToilets(1)
            .WithMaxToilets(2)
            .WithMinGarages(1)
            .WithMaxGarages(1)
            .WithFromArea(100)
            .WithToArea(200)
            .WithMinBuiltArea(80)
            .WithMaxBuiltArea(160)
            .WithMinSellingPrice(100000)
            .WithToSellingPrice(200000)
            .WithFromRentalPrice(0)
            .WithToRentalPrice(999999999)
            .WithStatus("Active");

        SearchPropertiesQuery query = builder.Build();

        _useCaseMock
            .Setup(useCase => useCase.ExecuteAsync(query, PageRequestHelper.Of("0", "1", "updatedAt:DESC")))
            .ReturnsAsync(
                new Page<Property>(new List<Property>(), new Pageable(0, 1, 0, 0, 0))
            );

        SearchPropertiesRequest request = new SearchPropertiesRequest
        {
            Types = new List<string> {"Apartment"},
            Transaction = "Sale",
            Uf = "Duckland",
            City = "White Duck",
            Districts = new List<string>(0),
            MinBedrooms = 0,
            MaxBedrooms = 3,
            MinToilets = 1,
            MaxToilets = 2,
            MinGarages = 1,
            MaxGarages = 1,
            MinArea = 100,
            MaxArea = 200,
            MinBuiltArea = 80,
            MaxBuiltArea = 160,
            MinPrice = 100000,
            MaxPrice = 200000,
            Status = "Active"
        };

        QPageRequest qPageRequest = new QPageRequest
        {
            PageNumber = "0",
            PageSize = "1",
            OrderBy = new List<string> {"updatedAt,Desc"}
        };

        // Act
        ObjectResult objectResult = await _invoker.InvokeAsync(() => _apiMethod.SearchProperties(request, qPageRequest), "en-US");

        // Assert
        Assert.NotNull(objectResult);
        Assert.Equal((int) HttpStatusCode.NoContent, objectResult.StatusCode);
    }

    [UseCulture("en-US")]
    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "GET /api/properties/search should return 200 when there is only one page")]
    public async Task SuccessfulToSearchProperties()
    {
        // Arrange
        SearchPropertiesQueryBuilder builder = new SearchPropertiesQueryBuilder();

        builder
            .WithTypes(new List<string> {"Apartment"})
            .WithTransaction("Rent")
            .WithState("Duckland")
            .WithCity("White Duck")
            .WithDistricts(new List<string> {"Downtown", "Alta Vista"})
            .WithMinBedrooms(0)
            .WithMaxBedrooms(3)
            .WithMinToilets(1)
            .WithMaxToilets(2)
            .WithMinGarages(1)
            .WithMaxGarages(1)
            .WithFromArea(100)
            .WithToArea(200)
            .WithMinBuiltArea(80)
            .WithMaxBuiltArea(160)
            .WithMinSellingPrice(0)
            .WithToSellingPrice(999999999)
            .WithFromRentalPrice(100000)
            .WithToRentalPrice(200000)
            .WithStatus("Active");

        SearchPropertiesQuery query = builder.Build();

        _useCaseMock
            .Setup(useCase => useCase.ExecuteAsync(query, PageRequestHelper.Of("0", "1", "updatedAt:DESC")))
            .ReturnsAsync(
                new Page<Property>(new List<Property>
                    {
                        new Property
                        {
                            Id = 1001,
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
                                State = "Duckland",
                                City = "White Duck",
                                District = "Downtown",
                                Address = "Good Life Street, 101",
                            },
                            Prices = new PropertyPrices
                            {
                                SellingPrice = 100000,
                                RentalTotalPrice = 1000,
                                RentalPrice = 700,
                                PriceByM2 = 100,
                                Discount = 100,
                                CondominiumFee = 90,
                            },
                            HashKey = "hash-key-1",
                            Ranking = 1,
                            Status = PropertyStatus.Active,
                            CreatedAt = DateTime.Parse("2023-01-01 23:59:59.999", CultureInfo.DefaultThreadCurrentCulture),
                            UpdatedAt = DateTime.Parse("2023-02-01 23:59:59.999", CultureInfo.DefaultThreadCurrentCulture),
                        }
                    },
                    new Pageable(0, 1, 1, 1, 1))
            );

        SearchPropertiesRequest request = new SearchPropertiesRequest
        {
            Types = new List<string> {"Apartment"},
            Transaction = "Rent",
            Uf = "Duckland",
            City = "White Duck",
            Districts = new List<string> {"Downtown", "Alta Vista"},
            MinBedrooms = 0,
            MaxBedrooms = 3,
            MinToilets = 1,
            MaxToilets = 2,
            MinGarages = 1,
            MaxGarages = 1,
            MinArea = 100,
            MaxArea = 200,
            MinBuiltArea = 80,
            MaxBuiltArea = 160,
            MinPrice = 100000,
            MaxPrice = 200000,
            Status = "Active"
        };

        QPageRequest qPageRequest = new QPageRequest
        {
            PageNumber = "0",
            PageSize = "1",
            OrderBy = new List<string> {"updatedAt,Desc"}
        };

        // Act
        ObjectResult objectResult = await _invoker.InvokeAsync(() => _apiMethod.SearchProperties(request, qPageRequest), "en-US");

        // Assert
        Assert.NotNull(objectResult);
        Assert.Equal((int) HttpStatusCode.OK, objectResult.StatusCode);

        JsonElement jsonElement = ApiMethodTestHelper.TryGetJsonElement(objectResult);

        _outputHelper.WriteLine(jsonElement.ToString());

        JsonAssertHelper.AssertThat(jsonElement)
            .AndExpectThat(JsonFrom.Path("$.content"), Is<object>.Single())
            .AndExpectThat(JsonFrom.Path("$.content[0].id"), Is<long>.EqualTo(1001))
            .AndExpectThat(JsonFrom.Path("$.content[0].tenantId"), Is<int>.EqualTo(1))
            .AndExpectThat(JsonFrom.Path("$.content[0].transaction"), Is<string>.EqualTo("RENT"))
            .AndExpectThat(JsonFrom.Path("$.content[0].title"), Is<string>.EqualTo("Apartment for rent"))
            .AndExpectThat(JsonFrom.Path("$.content[0].introduction"), Is<string>.EqualTo("Apartment for rent..."))
            .AndExpectThat(JsonFrom.Path("$.content[0].url"), Is<string>.EqualTo("https://www.apartment-for-rent.com"))
            .AndExpectThat(JsonFrom.Path("$.content[0].refId"), Is<string>.EqualTo("REF 101"))
            .AndExpectThat(JsonFrom.Path("$.content[0].coverImage"), Is<string>.EqualTo("https://www.apartment-for-rent.com/image1.jpg"))
            .AndExpectThat(JsonFrom.Path("$.content[0].numberOfBedrooms"), Is<int>.EqualTo(2))
            .AndExpectThat(JsonFrom.Path("$.content[0].numberOfToilets"), Is<int>.EqualTo(1))
            .AndExpectThat(JsonFrom.Path("$.content[0].numberOfGarages"), Is<int>.EqualTo(1))
            .AndExpectThat(JsonFrom.Path("$.content[0].area"), Is<byte>.EqualTo(100))
            .AndExpectThat(JsonFrom.Path("$.content[0].builtArea"), Is<int>.EqualTo(200))
            .AndExpectThat(JsonFrom.Path("$.content[0].state"), Is<string>.EqualTo("Duckland"))
            .AndExpectThat(JsonFrom.Path("$.content[0].city"), Is<string>.EqualTo("White Duck"))
            .AndExpectThat(JsonFrom.Path("$.content[0].district"), Is<string>.EqualTo("Downtown"))
            .AndExpectThat(JsonFrom.Path("$.content[0].address"), Is<string>.EqualTo("Good Life Street, 101"))
            .AndExpectThat(JsonFrom.Path("$.content[0].sellingPrice"), Is<int>.EqualTo(100000))
            .AndExpectThat(JsonFrom.Path("$.content[0].rentalTotalPrice"), Is<int>.EqualTo(1000))
            .AndExpectThat(JsonFrom.Path("$.content[0].rentalPrice"), Is<int>.EqualTo(700))
            .AndExpectThat(JsonFrom.Path("$.content[0].priceByM2"), Is<int>.EqualTo(100))
            .AndExpectThat(JsonFrom.Path("$.content[0].discount"), Is<int>.EqualTo(100))
            .AndExpectThat(JsonFrom.Path("$.content[0].condominiumFee"), Is<int>.EqualTo(90))
            .AndExpectThat(JsonFrom.Path("$.content[0].ranking"), Is<int>.EqualTo(1))
            .AndExpectThat(JsonFrom.Path("$.content[0].status"), Is<string>.EqualTo("ACTIVE"))
            .AndExpectThat(JsonFrom.Path("$.content[0].createdAt"), Is<string>.EqualTo("2023-01-01T23:59:59.999"))
            .AndExpectThat(JsonFrom.Path("$.content[0].updatedAt"), Is<string>.EqualTo("2023-02-01T23:59:59.999"))
            .AndExpectThat(JsonFrom.Path("$.content[0].href"), Is<string>.EqualTo("http://localhost:7029/api/properties-search/v1/properties/1001"));
    }
}